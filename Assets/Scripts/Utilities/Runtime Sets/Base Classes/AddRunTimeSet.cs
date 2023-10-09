using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AddRunTimeSet<T> : MonoBehaviour
{
    public T runTimeSet;
    protected abstract void OnEnable();
    protected abstract void OnDisable();
}
