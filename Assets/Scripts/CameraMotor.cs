using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float shiftMoveSpeed = 10f;

    float speedH = 2.0f;
    float speedV = 2.0f;
    float yaw = 0.0f;
    float pitch = 20f;
    bool rotating = false;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void Update()
    {

        translateCamera();

        rotateCamera();

    }

    public void translateCamera()
    {
        _transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
        float y = _transform.position.y;

        var moveSpeed = Input.GetKey(KeyCode.LeftShift) ? shiftMoveSpeed : this.moveSpeed;

        if (Input.GetKey(KeyCode.W))
        {
            _transform.position += _transform.forward * Time.deltaTime * moveSpeed;
            //_transform.position = Vector3.Lerp(_transform.position, )
        }

        if (Input.GetKey(KeyCode.S))
        {
            _transform.position -= _transform.forward * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _transform.position -= _transform.right * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _transform.position += _transform.right * Time.deltaTime * moveSpeed;
        }


        float x = _transform.position.x;
        float z = _transform.position.z;

        _transform.position = new Vector3(x, y, z);

        Vector3 camera = new Vector3(x, y + Input.GetAxis("Mouse ScrollWheel") * 60, z);

        _transform.position = Vector3.Lerp(_transform.position, camera, moveSpeed * Time.deltaTime);
        _transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }


    public void rotateCamera()
    {
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

        _transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

}
