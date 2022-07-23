using System.Collections;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }

    [SerializeField] private LayerMask groundMask;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundMask))
        {
            return hit.point;
        }
        else
        {
            return Vector3.negativeInfinity;
        }
    }
}