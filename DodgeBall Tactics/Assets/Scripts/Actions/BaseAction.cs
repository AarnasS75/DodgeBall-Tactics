using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    
    protected Unit unit;
    protected bool isActive;

    // virtual to let children override

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
}
