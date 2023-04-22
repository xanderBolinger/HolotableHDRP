using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{

    public float moveSpeed = 5f;
    float speedH = 2.0f;
    float speedV = 2.0f;
    float yaw = 0.0f;
    float pitch = 20f;
    bool rotating = false;

    void Update()
    {
        translateCamera();

        rotateCamera();

    }

    public void translateCamera() {
        transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
        float y = transform.position.y;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
            //transform.position = Vector3.Lerp(transform.position, )
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * moveSpeed;
        }
        
        if (Input.GetKey(KeyCode.A)) {
            transform.position -= transform.right * Time.deltaTime * moveSpeed;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }


        float x = transform.position.x;
        float z = transform.position.z;

        transform.position = new Vector3(x, y, z);

        Vector3 camera = new Vector3(x, y + Input.GetAxis("Mouse ScrollWheel") * 30, z);
        
        transform.position = Vector3.Lerp(transform.position, camera, moveSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }


    public void rotateCamera() {
        if (Input.GetKeyDown(KeyCode.Mouse1))
            rotating = true;
        else if (Input.GetKeyUp(KeyCode.Mouse1))
            rotating = false;

        if (!rotating)
            return;

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        // Limit rotation range
        //yaw = Mathf.Clamp(yaw, -30f, 30f);
        //the rotation range
        pitch = Mathf.Clamp(pitch, -20f, 90f);
        //the rotation range

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

}
