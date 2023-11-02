using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class SmoothCameraMotor : MonoBehaviour
{
    [SerializeField, Range(-20f, -90f)] float bottomAngleLimit;
    [SerializeField, Range(70f, 90f)] float topAngleLimit;
    [SerializeField, Range(0.01f, 1f)] float shiftMoveSpeed;
    [SerializeField, Range(1f, 5f)] float _moveSpeed;

    [SerializeField]
    private float _lookSensitivity = 3.0f;
    [SerializeField]
    private float _scrollSpeed = 20.0f;

    private float _rotationY;
    private float _rotationX;
    private Transform _transform;
    private float _nextDistanceFromTarget;

    [SerializeField]
    private float _distanceFromTarget = 10.0f;

    [SerializeField]
    private float _minDistanceFromTarget = 5.0f;
    [SerializeField]
    private float _maxDistanceFromTarget = 5.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocityHeight = Vector3.zero;
    private Vector3 _smoothVelocityRotation = Vector3.zero;

    private Vector3 _nextRotation = Vector3.zero;


    [SerializeField]
    private float _smoothTimeMovement = 0.2f;
    [SerializeField, Range(0.5f,0.9f)]
    private float _smoothTime = 0.2f;

    private void Awake()
    {
        _transform = transform;
        _nextDistanceFromTarget = _distanceFromTarget;
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
        var originalY = _transform.position.y;
        var movement = _transform.position;

        if (Input.GetKey(KeyCode.W))
            movement += _transform.forward * Time.deltaTime * _moveSpeed;

        if (Input.GetKey(KeyCode.S))
            movement -= _transform.forward * Time.deltaTime * _moveSpeed;

        if (Input.GetKey(KeyCode.A))
            movement -= _transform.right * Time.deltaTime * _moveSpeed;

        if (Input.GetKey(KeyCode.D))
            movement += _transform.right * Time.deltaTime * _moveSpeed;

        float originalMagnitude = movement.magnitude;
        
        movement.y = originalY;
        //movement = movement.normalized * originalMagnitude;

        _transform.position = movement;
    }

    void SetRotation() {
        if (!Input.GetMouseButton(1))
            return;

        float mouseX = Input.GetAxis("Mouse X") * _lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _lookSensitivity;

        _rotationY += mouseX;
        _rotationX += -mouseY;

        _rotationX = Mathf.Clamp(_rotationX, bottomAngleLimit, topAngleLimit);

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

    void UpdateDistanceToTarget()
    {
        _nextDistanceFromTarget += -Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed;
        if (_nextDistanceFromTarget < _minDistanceFromTarget)
            _nextDistanceFromTarget = _minDistanceFromTarget;
        else if (_nextDistanceFromTarget > _maxDistanceFromTarget)
            _nextDistanceFromTarget = _maxDistanceFromTarget;

        _distanceFromTarget = Mathf.Lerp(_distanceFromTarget,
            _nextDistanceFromTarget, 1 * Time.deltaTime);
    }

}
