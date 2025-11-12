using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float _speed;
    [SerializeField] float _sens;
    float _mouseX, _mouseY;
    float _rotX;
    float _rotY;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sens;
        _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sens;
        _rotX -= _mouseY;
        _rotY += _mouseX;
        _rotX = Mathf.Clamp(_rotX, -90f, 90f);

        transform.rotation = Quaternion.Euler(_rotX, _rotY, 0);

        Vector3 dir;
        dir = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward;
        if (Input.GetKey(KeyCode.Space))
        {
            dir += Vector3.up;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            dir -= Vector3.up;
        }

        transform.position += dir.normalized * _speed * Time.deltaTime;
    }
}
