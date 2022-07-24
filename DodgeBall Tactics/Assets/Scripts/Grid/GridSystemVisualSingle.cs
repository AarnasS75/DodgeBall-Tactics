using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;

    public void Show(Material meterial)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = meterial;
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
