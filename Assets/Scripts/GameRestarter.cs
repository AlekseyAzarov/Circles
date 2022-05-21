using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameRestarter : MonoBehaviour
{
    public Action TouchDetected;

    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop || Application.isEditor == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchDetected?.Invoke();
                TouchDetected = null;
                enabled = false;
            }
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TouchDetected?.Invoke();
                TouchDetected = null;
                enabled = false;
            }
        }
    }
}
