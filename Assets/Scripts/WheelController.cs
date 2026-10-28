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
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _brakeTorque;
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _idleBrakeTorque = 150f;


    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Transform startPosition = GameObject.Find("StartPosition").transform;
      
        // reset position, to put in start and call after dead and pressed E, in general player manager
        rb.transform.position = startPosition.position;
        rb.rotation = startPosition.rotation;
        transform.SetPositionAndRotation(startPosition.position, startPosition.rotation);
        Physics.SyncTransforms();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        }
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

        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        float motorForce = 0;
        float brakeForce = 0;

        // is in movement (forward or backward)
        if (Math.Abs(throttle) > 0)
        {
            // verify if trying to go opposite direction, then brake before actually changing the torque
            if (forwardSpeed * throttle < -0.5f) 
            {
                brakeForce = _brakeTorque;
            } 
            else
            {
                motorForce = _motorTorque * throttle;    
            }
        }
        // no input, will slow down with time
        else 
        {
            brakeForce = _idleBrakeTorque;
        }

        // handbrake overwrites (priority)
        if (brake > 0)
            brakeForce = brake * _brakeTorque;

        // 4 wheel drive
        float motorPerWheel = motorForce / 4;
        _frontRight.motorTorque = motorPerWheel;
        _frontLeft.motorTorque = motorPerWheel;
        _backRight.motorTorque = motorPerWheel;
        _backLeft.motorTorque = motorPerWheel;

        _frontRight.brakeTorque = brakeForce;
        _frontLeft.brakeTorque = brakeForce;
        _backRight.brakeTorque = brakeForce;
        _backLeft.brakeTorque = brakeForce;

        // turn less if goes fast (more if slow) (to add if time verify)
        _frontRight.steerAngle = steering * _maxSteerAngle;
        _frontLeft.steerAngle = steering * _maxSteerAngle;

        _frontLeftWheelVisual.transform.localRotation = Quaternion.Euler(0f, _frontLeft.steerAngle, 90f);
        _frontRightWheelVisual.transform.localRotation = Quaternion.Euler(0f, _frontRight.steerAngle, 90f);

    }
}
