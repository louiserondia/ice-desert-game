using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private InputActionReference _throttleAction;
    [SerializeField] private InputActionReference _handbrakeAction;
    [SerializeField] private InputActionReference _steeringAction;
    [SerializeField] private WheelCollider _frontRight;
    [SerializeField] private WheelCollider _frontLeft;
    [SerializeField] private WheelCollider _backRight;
    [SerializeField] private WheelCollider _backLeft;
    [SerializeField] private GameObject _frontLeftWheelVisual;
    [SerializeField] private GameObject _frontRightWheelVisual;

    private float _throttleInput;
    private float _handbrakeInput;
    private float _steeringInput;

    public float _acceleration = 500f;
    public float _handbrakeForce = 300f;
    public float _maxTurnAngle = 20f;
    private float _currentAcceleration = 0f;
    private float _currentBrakeForce = 0f;
    private float _currentSteering = 0f;

    public float _accelerationResponse = 300f;
    public float _accelerationBackwardsResponse = 150f;
    public float _handbrakeResponse = 300f;
    public float _steeringResponse = 20f;

    private void OnEnable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Enable();
        _throttleAction.action.Enable();
        _handbrakeAction.action.Enable();
        _steeringAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Disable();
        _throttleAction.action.Disable();
        _handbrakeAction.action.Disable();
        _steeringAction.action.Disable();
    }

    private float GetValueMoveTowards(InputActionReference ia, float force, float current, float response, float input)
    {
        // i should get the input in update and keep only physics in fixedupdate

        float targetforce = input * force;

        float result = Mathf.MoveTowards(current, targetforce, response * Time.fixedDeltaTime);
        return result;
    }

    private void Update()
    {
        _throttleInput = _throttleAction.action.ReadValue<float>();
        _handbrakeInput = _handbrakeAction.action.ReadValue<float>();
        _steeringInput = _steeringAction.action.ReadValue<float>();
    }


    private void FixedUpdate()
    {
        _currentAcceleration = GetValueMoveTowards(_throttleAction, _acceleration, _currentAcceleration, _accelerationResponse, _throttleInput);
        _currentBrakeForce = GetValueMoveTowards(_handbrakeAction, _handbrakeForce, _currentBrakeForce, _handbrakeResponse, _handbrakeInput);
        //_currentSteering = GetValueMoveTowards(_steeringAction, _maxTurnAngle, _currentSteering, _steeringResponse, _steeringInput);
        _currentSteering = _steeringInput * _maxTurnAngle;

        _frontRight.motorTorque = _currentAcceleration;
        _frontLeft.motorTorque = _currentAcceleration;

        _frontRight.brakeTorque = _currentBrakeForce;
        _frontLeft.brakeTorque = _currentBrakeForce;
        _backRight.brakeTorque = _currentBrakeForce;
        _backLeft.brakeTorque = _currentBrakeForce;

        _frontRight.steerAngle = _currentSteering;
        _frontLeft.steerAngle = _currentSteering;

        float targetAngle = Mathf.Clamp(_currentSteering, -1f, 1f) * _maxTurnAngle; 
        _frontLeftWheelVisual.transform.localRotation = Quaternion.Euler(0f, _currentSteering, 90f);
        _frontRightWheelVisual.transform.localRotation = Quaternion.Euler(0f, _currentSteering, 90f);

        // turn less if goes fast (more if slow)
    }
}
