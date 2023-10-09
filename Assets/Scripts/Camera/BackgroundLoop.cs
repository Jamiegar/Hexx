using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour, IInitalizable
{
    public GameObject m_backgroundObjects;
    private GameObject m_parentGameObject;

    public void Init()
    {
        m_parentGameObject = new GameObject("Background"); //Create a parent object for all background textures
        m_parentGameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z); 
        
        for (int i = 0; i < 1; i++)
        {
            LoadBackgroundObjects();
        }
    }

    private void LoadBackgroundObjects()
    {
        float objectwidth = m_backgroundObjects.GetComponent<SpriteRenderer>().bounds.size.x;
        int neededObj = 4;
        
        for(int i = 0; i <= neededObj; i++)
        {
            GameObject clone = Instantiate(m_backgroundObjects, m_parentGameObject.transform);
            clone.transform.position = new Vector3(objectwidth * i, m_parentGameObject.transform.position.y, m_parentGameObject.transform.position.z);
            clone.name = m_backgroundObjects.name + i;
        }
    }

    private void RepositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if(children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float objectRadius = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

            if(transform.position.x + Screen.width > lastChild.transform.position.x + objectRadius)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + objectRadius * 2, lastChild.transform.position.y, 
                    lastChild.transform.position.z);

            }
            else if(transform.position.x - Screen.width < firstChild.transform.position.x - objectRadius)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - objectRadius * 2, lastChild.transform.position.y,
                    lastChild.transform.position.z);
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i <= 1; i++)
        {
            RepositionChildObjects(m_parentGameObject);
        }
    }


}
