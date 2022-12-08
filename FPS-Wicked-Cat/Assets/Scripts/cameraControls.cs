using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int sensHor; 
    [SerializeField] int sensVert;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [SerializeField] bool invertX;

    float xRotation;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        if (invertX)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY; 
        }

        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}