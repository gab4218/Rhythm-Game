using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CameraFovOverride : MonoBehaviour
{
    [SerializeField] private Camera[] _cams;
    [SerializeField] private float _fov;


    private void Update()
    {
        UpdateFOV();
    }

    private void UpdateFOV()
    {
        float scaler = _fov / Screen.width;        

        float fov = 0.5f * Screen.height * scaler;
        
        foreach (var cam in _cams)
        {
            if (cam.orthographic)
            {
                cam.orthographicSize = fov;
            }
            else
            {
                cam.fieldOfView = fov;
            }
        }
    }

}
