using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

[RequireComponent(typeof(GridTransform))]
public class TopDown_Movement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] float movementSpeed = 1.5f;
    [SerializeField] GridTransform m_gridTransform;
    [MinMaxSlider(-1000, 1000)]
    [SerializeField] private Vector2 _clampedZPostion = new Vector2(-250, 250);

    [Header("Zoom")]
    [SerializeField] float m_zoomAmount = 0.1f;
    
    [Tooltip("This variable is a a vector 2 with the X being the min and the Y being the max. Use the range slider to find the required range")]
    [SerializeField, MinMaxSlider(10, 100)] Vector2 m_heightRange = new Vector2();
    
    [Tooltip("This variable is a a vector 2 with the X being the min and the Y being the max. Use the range slider to find the required range")]
    [SerializeField, MinMaxSlider(10, 100)] Vector2 m_distanceRange = new Vector2();

    [Header("Camera")]
    [SerializeField] Transform m_cameraLookAtTarget;
    [SerializeField] Transform m_cameraObj;
    [SerializeField] float m_height = 10f;
    [SerializeField] float m_distance = 20f;
    [SerializeField] float m_angle = 45f;
    [SerializeField] float m_smoothingSpeed = 0.5f;

    private Vector3 _refVelocity;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        Vector3 _startingPosition = transform.position;

        _clampedZPostion.x = _startingPosition.z + transform.position.z;
        _clampedZPostion.y = _startingPosition.z - transform.position.z;
    }


    private void OnDrawGizmos() //Draws the line trace pointing to the froward direction of the player
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 5f));
    }

    #endregion

    #region HandleMovement Methods
    public void HandelMovement(Vector2 input) 
    {
        AddMovement(transform.forward, input.y, movementSpeed); //Adds movement on the forward and backwards vector
        AddMovement(transform.right, input.x, movementSpeed); //Adds movement on the left and right vector
    }

    public void HandelCameraPosition() 
    {
        if (!m_cameraLookAtTarget)
            return;

        Vector3 worldPosition = (Vector3.forward * -m_distance) + (Vector3.up * m_height);
        Vector3 rotatedVector = Quaternion.AngleAxis(m_angle, Vector3.up) * worldPosition;

        Vector3 flatTargetPosition = new Vector3(m_cameraLookAtTarget.position.x, 0, m_cameraLookAtTarget.position.z);
        Vector3 cameraPosition = flatTargetPosition + rotatedVector;

        m_cameraObj.transform.position = Vector3.SmoothDamp(m_cameraObj.transform.position, cameraPosition, ref _refVelocity, m_smoothingSpeed);
        m_cameraObj.transform.LookAt(flatTargetPosition);

        RotateToDirection(Quaternion.AngleAxis(m_angle, Vector3.up));
    }

    public void HandelCameraZoom(Vector2 input)
    {
        //Calc the new zoom variables 
        float newHeight = m_height + (m_zoomAmount * input.y);
        float newDistance = m_distance + (m_zoomAmount * input.y);

        //Clamp the zoom so that it does not go out of the max and min range 
        newHeight = Mathf.Clamp(newHeight, m_heightRange.x, m_heightRange.y);
        newDistance = Mathf.Clamp(newDistance, m_distanceRange.x, m_distanceRange.y);

        //Set the new zoom vairables 
        m_distance = newDistance;
        m_height = newHeight;

    }
    #endregion

    #region Helper Methods
    private void AddMovement(Vector3 direction, float inputValue, float multiplir)
    {
        transform.position += (direction * inputValue) * multiplir; //Adds movement in a direction and dependents on the input value

        //Clamp Position of player to not go over the edge
        if(transform.position.z >= _clampedZPostion.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _clampedZPostion.y);
        }
        else if(transform.position.z <= _clampedZPostion.x)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _clampedZPostion.x);
        }
    }

    public void RotateToDirection(Quaternion direction)
    {
        transform.rotation = direction; //Rotates the gameobject to face the inputed direction
    }

    #endregion


    private void Update()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, (transform.up * -1) * 100, out hit, 200f); //Set raycast down towards ground

        if (hit.collider != null) //Update the chunk position
        {
            Debug.DrawRay(transform.position, (transform.up * -1) * hit.distance, Color.red);

            TileType gridTrans = hit.collider.GetComponentInParent<TileType>();
            Chunk newChunk = hit.collider.GetComponentInParent<Chunk>();

            if(m_gridTransform.chunkPosition != newChunk.ChunkGridPosition)
            {
                m_gridTransform.chunkPosition = newChunk.ChunkGridPosition;
                m_gridTransform.OnChunkPositionChange.Invoke(newChunk);
            }

            if(m_gridTransform.gridPosition != gridTrans.gridTransform.gridPosition)
            {
                m_gridTransform.gridPosition = gridTrans.gridTransform.gridPosition;
                m_gridTransform.OnPositionChange.Invoke(gridTrans);
            }

        }
    }

}
