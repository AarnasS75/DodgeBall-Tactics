using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
    }

    public Vector2 GetMouseScreenPosition()
    {
        return Input.mousePosition;
    }
    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }
}
