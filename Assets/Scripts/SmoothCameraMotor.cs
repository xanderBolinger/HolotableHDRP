using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class SmoothCameraMotor : MonoBehaviour
{
    [SerializeField, Range(-1f, -90f)] float _topAngleLimit;
    [SerializeField, Range(1f, 90f)] float _bottomAngleLimit;
    [SerializeField, Range(1f, 10f)] float _shiftMoveSpeed;
    [SerializeField, Range(1f, 30f)] float _shiftMoveAcceleration;
    [SerializeField, Range(1f, 10f)] float _moveSpeed;
    [SerializeField, Range(1f, 30f)] float _moveAcceleration;
    [SerializeField, Range(1f, 30f)] float _moveDeceleration;

    [SerializeField, Range(1f, 3f)] float _lookSensitivity = 2.0f;
    [SerializeField] float _scrollSpeed = 20.0f;

    [SerializeField, Range(0.3f, 0.8f)] float _scrollSmoothTime = 0.5f;
    [SerializeField, Range(0.1f, 0.3f)] float _smoothTime = 0.2f;

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
        Quaternion rotation = Quaternion.Euler(0, _transform.eulerAngles.y, 0);
        Vector3 localDirection = Vector3.forward;
        Vector3 forward = rotation * localDirection;

        var forwardSpeed = MoveForwardAndBack();
        var sideWaysSpeed = MoveSideToSide();
        var zPos = forward.normalized * forwardSpeed;
        var xPos = _transform.right.normalized * sideWaysSpeed;

        var movement = zPos + xPos;

        _transform.position = _transform.position + movement;
    }

    float currentSpeed;
    float currentSpeedForward;

    float MoveForwardAndBack()
    {
        var maxSpeed = Input.GetKey(KeyCode.LeftShift) ? _shiftMoveSpeed : _moveSpeed;
        var acceleration = Input.GetKey(KeyCode.LeftShift) ? _shiftMoveAcceleration : _moveAcceleration;

        if ((Input.GetKey(KeyCode.W)) && (currentSpeedForward < maxSpeed))
            currentSpeedForward = currentSpeedForward - acceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.S)) && (currentSpeedForward > -maxSpeed))
            currentSpeedForward = currentSpeedForward + acceleration * Time.deltaTime;
        else
        {
            if (currentSpeedForward > _moveDeceleration * Time.deltaTime)
                currentSpeedForward = currentSpeedForward - _moveDeceleration * Time.deltaTime;
            else if (currentSpeedForward < -_moveDeceleration * Time.deltaTime)
                currentSpeedForward = currentSpeedForward + _moveDeceleration * Time.deltaTime;
            else
                currentSpeedForward = 0;
        }

        return currentSpeedForward * Time.deltaTime;
    }

    float MoveSideToSide()
    {
        var maxSpeed = Input.GetKey(KeyCode.LeftShift) ? _shiftMoveSpeed : _moveSpeed;
        var acceleration = Input.GetKey(KeyCode.LeftShift) ? _shiftMoveAcceleration : _moveAcceleration;

        if ((Input.GetKey(KeyCode.A)) && (currentSpeed < maxSpeed))
            currentSpeed = currentSpeed - acceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.D)) && (currentSpeed > -maxSpeed))
            currentSpeed = currentSpeed + acceleration * Time.deltaTime;
        else
        {
            if (currentSpeed > _moveDeceleration * Time.deltaTime)
                currentSpeed = currentSpeed - _moveDeceleration * Time.deltaTime;
            else if (currentSpeed < -_moveDeceleration * Time.deltaTime)
                currentSpeed = currentSpeed + _moveDeceleration * Time.deltaTime;
            else
                currentSpeed = 0;
        }

        return currentSpeed * Time.deltaTime;
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

        _transform.position = Vector3.SmoothDamp(pos, camera, ref _smoothVelocityHeight, _scrollSmoothTime);
    }

}
