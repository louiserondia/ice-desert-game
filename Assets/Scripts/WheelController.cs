using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelController : MonoBehaviour
{
    // Input Actions
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private InputActionReference _throttleAction;
    [SerializeField] private InputActionReference _brakeAction;
    [SerializeField] private InputActionReference _steeringAction;

    // Wheel Colliders
    [SerializeField] private WheelCollider _frontRight;
    [SerializeField] private WheelCollider _frontLeft;
    [SerializeField] private WheelCollider _backRight;
    [SerializeField] private WheelCollider _backLeft;

    // Wheel Visuals
    [SerializeField] private GameObject _frontLeftWheelVisual;
    [SerializeField] private GameObject _frontRightWheelVisual;

    // Car Settings
    public float _motorTorque = 1500f;
    public float _brakeTorque = 3000f;
    public float _maxSteerAngle = 30f;


    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Enable();
        _throttleAction.action.Enable();
        _steeringAction.action.Enable();
        _brakeAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Disable();
        _throttleAction.action.Disable();
        _steeringAction.action.Disable();
        _brakeAction.action.Disable();
    }

    private void Update()
    {
   
    }

    private float ReadInput(InputActionReference inputReference)
    {
        if (inputReference == null || inputReference.action == null)
            return 0f;

        return inputReference.action.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        float throttle = ReadInput(_throttleAction);
        float steering = ReadInput(_steeringAction);
        float brake = ReadInput(_brakeAction);

        //UnityEngine.Debug.Log();

        _frontRight.motorTorque = throttle * _motorTorque;
        _frontLeft.motorTorque = throttle * _motorTorque;

        float currentBrake = brake * _brakeTorque;
        _frontRight.brakeTorque = currentBrake;
        _frontLeft.brakeTorque = currentBrake;
        _backRight.brakeTorque = currentBrake;
        _backLeft.brakeTorque = currentBrake;

        _frontRight.steerAngle = steering * _maxSteerAngle;
        _frontLeft.steerAngle = steering * _maxSteerAngle;

        //float targetAngle = Mathf.Clamp(_currentSteering, -1f, 1f) * _maxTurnAngle; 
        _frontLeftWheelVisual.transform.localRotation = Quaternion.Euler(0f, _frontLeft.steerAngle, 90f);
        _frontRightWheelVisual.transform.localRotation = Quaternion.Euler(0f, _frontRight.steerAngle, 90f);

        // turn less if goes fast (more if slow)
    }
}
