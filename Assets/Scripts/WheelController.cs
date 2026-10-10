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
    private float _currentSteering = 0f;

    public float _accelerationResponse = 300f;
    public float _brakeResponse = 300f;
    public float _steeringResponse = 300f;

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

    private float GetValueMoveTowards(InputActionReference ia, float force, float current, float response)
    {
        // i should get the input in update and keep only physics in fixedupdate

        float input = ia.action.ReadValue<float>();
        float targetforce = input * force;

        float result = Mathf.MoveTowards(current, targetforce, response * Time.fixedDeltaTime);
        return result;
    }

    private void FixedUpdate()
    {
        _currentAcceleration = GetValueMoveTowards(_throttleAction, _acceleration, _currentAcceleration, _accelerationResponse);
        _currentBrakeForce = GetValueMoveTowards(_brakeAction, _brakeForce, _currentBrakeForce, _brakeResponse);
        _currentSteering = GetValueMoveTowards(_steeringAction, _maxTurnAngle, _currentSteering, _steeringResponse);

        _frontRight.motorTorque = _currentAcceleration;
        _frontLeft.motorTorque = _currentAcceleration;

        _frontRight.brakeTorque = _currentBrakeForce;
        _frontLeft.brakeTorque = _currentBrakeForce;
        _backRight.brakeTorque = _currentBrakeForce;
        _backLeft.brakeTorque = _currentBrakeForce;

        _frontRight.steerAngle = _currentSteering;
        _frontLeft.steerAngle = _currentSteering;

        // turn less if goes fast (more if slow)
    }
}
