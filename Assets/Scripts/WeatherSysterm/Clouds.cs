using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Clouds : WeatherCondition
{
    [SerializeField]
    private List<GameObject> m_cloudPrefabs;

    [SerializeField]
    private Vector2 m_spawnSize = new Vector2(50, 50);

    [SerializeField]
    private int m_numberOfObjects = 5;
    public int NumberOfObjects 
    { 
        get { return m_numberOfObjects; }
        set { m_numberOfObjects = value; }
    }

    [SerializeField]
    private float m_cloudMovementSpeed = 10f;

    public float CloudsMovementSpeed
    {
        get { return m_cloudMovementSpeed; }
        set { m_cloudMovementSpeed = value; }
    }

    [SerializeField]
    private float m_fadeSpeed = 0.5f;

    private GameObject m_parentCloudObject;
    private List<GameObject> m_clouds;
    private BoxCollider m_boxCollider;
    private Bounds m_spawningBounds;
    private List<GameObject> m_cloundsToBeKilled;

    private void OnValidate() //This updates the size of the bounds box in edit time
    {
        if (m_boxCollider == null)
            m_boxCollider = GetComponent<BoxCollider>();

        m_boxCollider.size = new Vector3(m_spawnSize.x, 0, m_spawnSize.y);
    }

    public override void InitaliseWeather() //Set up Weather 
    {
        if (m_boxCollider == null) //Gets the box collider
            m_boxCollider = GetComponent<BoxCollider>();

        #region Initalise Lists
        m_cloundsToBeKilled = new List<GameObject>();
        m_clouds = new List<GameObject>();
        #endregion

        m_spawningBounds = m_boxCollider.bounds; //Store variable to bounds of box

        //Create a parent game object that all clounds will be a child of to keep the hierarchy clean
        m_parentCloudObject = new GameObject("Clouds"); 
        m_parentCloudObject.transform.position = transform.position;

        foreach (GameObject prefab in m_cloudPrefabs) //Spawn Clounds at initalisation
        {
            SpawnCloud(prefab);
        }
        
    }
    
    private void SpawnCloud(GameObject prefab)
    {
        int randomNum = Random.Range(1, m_numberOfObjects);

        for (int i = 0; i < randomNum; i++)
        {
            //Get random X & Z offset within the bounds of the box collider
            float offsetX = Random.Range(-m_spawningBounds.extents.x, m_spawningBounds.extents.x);
            float offsetZ = Random.Range(-m_spawningBounds.extents.z, m_spawningBounds.extents.z);

            GameObject go = Instantiate(prefab); //Spawn

            #region Set Alpha to be 0 so it is completely transparant 
            Renderer renderer = go.GetComponent<Renderer>();
            Color colour = renderer.material.color;
            renderer.material.color = new Color(colour.r, colour.g, colour.b, 0);
            #endregion

            #region Set Position and rotation
            go.transform.position = transform.position + new Vector3(offsetX, 0, offsetZ);
            go.transform.rotation = new Quaternion(0, Random.rotation.y, 0, 0);
            go.transform.SetParent(m_parentCloudObject.transform);
            #endregion

            m_clouds.Add(go);
        }
    }


    public override void UpdateWeather()
    {
        MoveClouds();
        DestroyClouds();
        CloudCountCheck();
    }

    private void CloudCountCheck()
    {
        if (m_clouds.Count >= m_numberOfObjects) //Check the number of clounds in the list
            return;

        int numberToSpawn = m_numberOfObjects - m_clouds.Count; //Work out the number of objects thta need to be spawned

        for(int i = 0; i < numberToSpawn; i++) //Loop the number of objects that need to be spawned
        {
            //Get a random clound to spawn
            int index = Random.Range(0, Mathf.Clamp(numberToSpawn, 0, m_cloudPrefabs.Count - 1));
            SpawnCloud(m_cloudPrefabs[index]); //Spawn Cloud
        }
    }


    private void MoveClouds()
    {
        foreach (GameObject obj in m_clouds) //Loop through all clouds
        {
            //Move clouds on the X 
            obj.transform.position += new Vector3((obj.transform.position.x * m_cloudMovementSpeed) * Time.deltaTime, 0, 0);

            Vector3 cloudPosition = obj.transform.position;

            if(KillCheck(cloudPosition) == true) //check if the clound is out of the bounds
            {
                FadeOutCloud(obj); //If true fade out cloud
            }
            else
            {
                FadeInCloud(obj); //If False fade In
            }

        }
    }

    private bool KillCheck(Vector3 cloudPosition)
    {
        Bounds bounds = m_boxCollider.bounds; //Get the bounds
        
        //Returns true if the clouds are outside the bounds  
        if (cloudPosition.x < -(bounds.extents.x - transform.position.x))
        {
            return true;
        }
        else if (cloudPosition.x > bounds.extents.x + transform.position.x)
        {
            return true;
        }
        else if (cloudPosition.z < -(bounds.extents.z - transform.position.z))
        {
            return true;
        }
        else if (cloudPosition.z > bounds.extents.z + transform.position.z)
        {
            return true;
        }
        return false;
    }

    private void FadeInCloud(GameObject cloud)
    {
        Renderer cloudRenderer = cloud.GetComponent<Renderer>(); //Get renderer

        if (cloudRenderer.material.color.a >= 1) //return if the alpha is already at 1
            return;

        Color cloudColour = cloudRenderer.material.color; //Store cloud colour
        float fadeAmount = cloudColour.a + (m_fadeSpeed * Time.deltaTime); //Work out fade amount

        //Set the new colour
        cloudColour = new Color(cloudColour.r, cloudColour.g, cloudColour.b, fadeAmount); 
        cloudRenderer.material.color = cloudColour;
    }


    private void FadeOutCloud(GameObject cloud)
    {
        Renderer cloudRenderer = cloud.GetComponent<Renderer>(); //Get renderer

        Color cloudColour = cloudRenderer.material.color; //Store cloud colour
        float fadeAmount = cloudColour.a - (m_fadeSpeed * Time.deltaTime);

        //Set colour
        cloudColour = new Color(cloudColour.r, cloudColour.g, cloudColour.b, fadeAmount);
        cloudRenderer.material.color = cloudColour;

        //if alpha is <= 0 then it is fadeded out or if it is not visible add to kill list
        if(cloudRenderer.material.color.a <= 0 || cloudRenderer.isVisible == false)
        {
            m_cloundsToBeKilled.Add(cloud);
        }
    }
    
    private void DestroyClouds()
    {
        if (m_cloundsToBeKilled.Count > 1)
        {
            foreach (GameObject obj in m_cloundsToBeKilled)
            {
                m_clouds.Remove(obj); //Remove object from list of clounds
                Destroy(obj);
            }
            m_cloundsToBeKilled.Clear(); //Clear list
        }
    }
}
