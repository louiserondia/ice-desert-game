using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] // to modify in editor, better than to put public
    private InputActionAsset _inputAsset;

    [SerializeField]
    private InputActionReference _movementAction;

    private InputAction _jumpAction;
    private InputAction _shootAction;

    protected override void Awake()
    {
        base.Awake();

        if (_inputAsset == null) return;

        _jumpAction = _inputAsset.FindActionMap("Gameplay").FindAction("Jump");
        _shootAction = _inputAsset.FindActionMap("Gameplay").FindAction("Shoot");

        _jumpAction.performed += HandleJumpInput; // binds to callback
    }

    private void OnEnable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Disable();
    }

    void Update()
    {
        HandleMovementInput();
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (_attackBehaviour == null || _shootAction == null) return;

        if (_shootAction.IsPressed()) _attackBehaviour.Attack();
    }

    private void HandleMovementInput()
    {
        if (_movementBehaviour == null || _movementAction == null) return;

        float movementInput = _movementAction.action.ReadValue<float>();
        Vector3 movement = movementInput * Vector3.right;

        _movementBehaviour.DesiredMovementDirection = movement;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (_movementBehaviour == null) return;

        _movementBehaviour.Jump();
    }

    protected void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
    }
}
