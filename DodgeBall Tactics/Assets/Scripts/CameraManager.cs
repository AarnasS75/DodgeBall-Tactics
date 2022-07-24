using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
        print(typeof(ThrowAction));

    }

    private void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ThrowAction:
                ShowActionCamera();
                break;
        }
    }  
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ThrowAction:
                HideActionCamera();
                break;
        }
    }

}
