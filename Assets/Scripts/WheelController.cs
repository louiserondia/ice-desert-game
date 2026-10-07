using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private InputActionReference _throttleAction;
    [SerializeField] private InputActionReference _brakeAction;
    [SerializeField] private InputActionReference _steeringAction;
    [SerializeField] private WheelCollider _frontRight;
    [SerializeField] private WheelCollider _frontLeft;
    [SerializeField] private WheelCollider _backRight;
    [SerializeField] private WheelCollider _backLeft;

    public float _acceleration = 500f;
    public float _brakeForce = 300f;
    public float _maxTurnAngle = 20f;
    private float _currentAcceleration = 0f;
    private float _currentBrakeForce = 0f;
    private float _currentTurnAngle = 0f;


    private void OnEnable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Enable();
        _throttleAction.action.Enable();
        _brakeAction.action.Enable();
        _steeringAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Disable();
        _throttleAction.action.Disable();
        _brakeAction.action.Disable();
        _steeringAction.action.Disable();
    }


    private void FixedUpdate()
    {
        _currentAcceleration = _acceleration * _throttleAction.action.ReadValue<float>();
        _currentBrakeForce = _brakeForce * _brakeAction.action.ReadValue<float>();

        _frontRight.motorTorque = _currentAcceleration;
        _frontLeft.motorTorque = _currentAcceleration;

        _frontRight.brakeTorque = _currentBrakeForce;
        _frontLeft.brakeTorque = _currentBrakeForce;
        _backRight.brakeTorque = _currentBrakeForce;
        _backLeft.brakeTorque = _currentBrakeForce;

        _currentTurnAngle = _maxTurnAngle * _steeringAction.action.ReadValue<float>();
        _frontRight.steerAngle = _currentTurnAngle;
        _frontLeft.steerAngle = _currentTurnAngle;
    }
}
