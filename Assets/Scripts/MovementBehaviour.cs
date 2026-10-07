using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _jumpStrength = 10.0f;

    private Rigidbody _rigidbody;

    private Vector3 _desiredMovementDirection = Vector3.zero;

    private bool _grounded = false;
    private const float GROUND_CHECK_DIST =0.2f;

    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void FixedUpdate()
    {
        HandleMovement();

        _grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DIST, LayerMask.GetMask("Ground"));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void HandleMovement()
    {
        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed; // fixed update already uses the delta time, not necessary

        // maintain vertical velocity as it was otherwise graviy would be stripped out -> 30min30 video 8 pas compris
        movement.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = movement;

        // set velocity for fast snappy mvt, for more real physics mvt, apply force eg for car race
    }

    public void Jump()
    {
        if (_grounded)
            _rigidbody.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
    }
}