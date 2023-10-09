using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected IPossessable m_possessedObject;

    public void Possess(IPossessable obj)
    {
        if (obj == null)
            return;

        if (m_possessedObject != null)
            m_possessedObject.UnPossess();

        m_possessedObject = obj;
        m_possessedObject.Possess();
    }

    public void UnPossess()
    {
        if (m_possessedObject != null)
            m_possessedObject.UnPossess();
    }

}
