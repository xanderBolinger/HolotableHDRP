using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class SmoothCameraMotor : MonoBehaviour
{
    [SerializeField, Range(-1f, -90f)] float _topAngleLimit;
    [SerializeField, Range(1f, 90f)] float _bottomAngleLimit;
    [SerializeField, Range(0.01f, 1f)] float shiftMoveSpeed;
    [SerializeField, Range(1f, 5f)] float _moveSpeed;
    [SerializeField, Range(1f, 10f)] float _moveAcceleration;
    [SerializeField, Range(1f, 10f)] float _moveDeceleration;

    [SerializeField]
    private float _lookSensitivity = 3.0f;
    [SerializeField]
    private float _scrollSpeed = 20.0f;

    [SerializeField]
    private float _smoothTimeMovement = 0.2f;

    [SerializeField, Range(0.5f, 0.9f)]
    private float _smoothTime = 0.2f;

    private float _rotationY;
    private float _rotationX;
    private Transform _transform;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocityHeight = Vector3.zero;
    private Vector3 _smoothVelocityRotation = Vector3.zero;

    private Vector3 _nextRotation = Vector3.zero;

    private void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        Move();

        SetRotation();

        UpdateRotation();

        Scroll();

    }

    private void Move()
    {
        var pos = _transform.position;
        
        Quaternion rotation = Quaternion.Euler(0, _transform.eulerAngles.y, 0);
        Vector3 localDirection = Vector3.forward;
        Vector3 forward = rotation * localDirection;

        var zPos = forward * MoveForwardAndBack();
        var xPos = _transform.right * MoveSideToSide();

        var movement = zPos + xPos;
        movement.y = pos.y;

        _transform.position = movement;
    }

    float currentSpeed;
    float currentSpeedForward;

    float MoveForwardAndBack()
    {
        if ((Input.GetKey(KeyCode.W)) && (currentSpeedForward < _moveSpeed))
            currentSpeedForward = currentSpeedForward - _moveAcceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.S)) && (currentSpeedForward > -_moveSpeed))
            currentSpeedForward = currentSpeedForward + _moveAcceleration * Time.deltaTime;
        else
        {
            if (currentSpeedForward > _moveDeceleration * Time.deltaTime)
                currentSpeedForward = currentSpeedForward - _moveDeceleration * Time.deltaTime;
            else if (currentSpeedForward < -_moveDeceleration * Time.deltaTime)
                currentSpeedForward = currentSpeedForward + _moveDeceleration * Time.deltaTime;
            else
                currentSpeedForward = 0;
        }

        return _transform.position.z + currentSpeedForward * Time.deltaTime;
    }

    float MoveSideToSide()
    {
        if ((Input.GetKey(KeyCode.A)) && (currentSpeed < _moveSpeed))
            currentSpeed = currentSpeed - _moveAcceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.D)) && (currentSpeed > -_moveSpeed))
            currentSpeed = currentSpeed + _moveAcceleration * Time.deltaTime;
        else
        {
            if (currentSpeed > _moveDeceleration * Time.deltaTime)
                currentSpeed = currentSpeed - _moveDeceleration * Time.deltaTime;
            else if (currentSpeed < -_moveDeceleration * Time.deltaTime)
                currentSpeed = currentSpeed + _moveDeceleration * Time.deltaTime;
            else
                currentSpeed = 0;
        }

        return _transform.position.x + currentSpeed * Time.deltaTime;
    }


    void SetRotation() {
        if (!Input.GetMouseButton(1))
            return;

        float mouseX = Input.GetAxis("Mouse X") * _lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _lookSensitivity;

        _rotationY += mouseX;
        _rotationX += -mouseY;

        _rotationX = Mathf.Clamp(_rotationX, _topAngleLimit, _bottomAngleLimit);

        _nextRotation = new Vector3(_rotationX, _rotationY);
    }

    void UpdateRotation() {
        _currentRotation = Vector3.SmoothDamp(_currentRotation, _nextRotation, ref _smoothVelocityRotation, _smoothTime);
        _transform.localEulerAngles = _currentRotation;
    }

    void Scroll() {
        var pos = _transform.position;

        Vector3 camera = new Vector3(pos.x, pos.y + Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed, pos.z);

        _transform.position = Vector3.SmoothDamp(pos, camera, ref _smoothVelocityHeight, _smoothTime);
    }

}
