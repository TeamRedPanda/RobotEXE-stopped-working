using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IMovementActions
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    private Rigidbody2D _rb;

    private PlayerControls _playerControls;
    private bool _Jump;
    private float _movementInput;
    private bool _isGrounded;

    private static int _isWalkingHash = Animator.StringToHash("IsWalking");
    private static int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private static int _jumpHash = Animator.StringToHash("Jump");

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        _playerControls.Movement.SetCallbacks(this);
    }

    private void OnDisable()
    {
        _playerControls?.Dispose();
        _playerControls = null;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    private void CheckGrounded()
    {
        var size = new Vector2(0.5f, 0.05f);
        var center = (Vector2)transform.position - new Vector2(0f, 0.02f + size.y / 2f);
        var hit = Physics2D.BoxCast(center, size, 0, Vector2.down, 0f);
        _isGrounded = hit.collider != null;
        animator.SetBool(_isGroundedHash, _isGrounded);
    }

    private void HandleMovement()
    {
        animator.SetBool(_isWalkingHash, Mathf.Abs(_movementInput) >= 0.01f && _isGrounded);
        if (!_isGrounded)
            return;

        sprite.flipX = _movementInput < 0;
        _rb.velocity = Vector2.right * _movementInput;
    }

    private void HandleJump()
    {
        if (!_Jump)
            return;
        
        _Jump = false;
        
        if (!_isGrounded)
            return;
        
        animator.SetTrigger(_jumpHash);
        var currentVelocity = _rb.velocity;
        _rb.velocity = new Vector2(currentVelocity.x, 20f);
    }
    
    public void OnWalk(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _Jump = true;
        }
    }
}
