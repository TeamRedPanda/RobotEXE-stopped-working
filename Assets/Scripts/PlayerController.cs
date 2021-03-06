using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, PlayerControls.IMovementActions
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    [Header("Channels")] [SerializeField] private FloatChannelAsset playerSpeedChannel;

    [SerializeField] private FloatChannelAsset jumpStrengthChannel;

    [SerializeField] private PlayerState playerState;

    private Rigidbody2D _rb;

    private PlayerControls _playerControls;
    private bool _Jump;
    private float _movementInput;
    private bool _isGrounded;

    private float _speed = -5;
    [SerializeField] private float _speedMultiplier = 20;

    private static int _isWalkingHash = Animator.StringToHash("IsWalking");
    private static int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private static int _jumpHash = Animator.StringToHash("Jump");
    private static int _dieHash = Animator.StringToHash("Die");
    private static int _cheerHash = Animator.StringToHash("Cheer");

    private float _jumpStrength;
    [SerializeField] private float _jumpStrengthMultiplier = 5;

    [SerializeField] private UnityEvent _onPlayerDeath;
    [SerializeField] private UnityEvent _onPlayerWin;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerSpeedChannel.Emit((_speed / _speedMultiplier + 1f) * 0.5f);
    }

    void OnEnable()
    {
        _playerControls = new PlayerControls();

        if (playerState.CanMove)
            EnableWalk();
        if (playerState.CanJump)
            EnableJump();

        _playerControls.Movement.SetCallbacks(this);
        playerSpeedChannel.OnData += UpdateSpeed;
        jumpStrengthChannel.OnData += UpdateJumpStrength;
    }

    private void OnDisable()
    {
        _playerControls?.Dispose();
        _playerControls = null;
        playerSpeedChannel.OnData -= UpdateSpeed;
        jumpStrengthChannel.OnData -= UpdateJumpStrength;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            animator.SetTrigger(_dieHash);
            StartCoroutine(DelayedUnityEvent(_onPlayerDeath, 2f));
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            animator.SetTrigger(_cheerHash);
            _playerControls.Movement.Disable();
            StartCoroutine(DelayedUnityEvent(_onPlayerWin, 3f));
        }
    }

    private IEnumerator DelayedUnityEvent(UnityEvent @event, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        @event?.Invoke();
    }

    public void EnableWalk()
    {
        _playerControls.Movement.Walk.Enable();
    }

    public void EnableJump()
    {
        _playerControls.Movement.Jump.Enable();
    }

    private void UpdateSpeed(float speed)
    {
        _speed = (speed - 0.5f) * _speedMultiplier;
    }

    private void UpdateJumpStrength(float strength)
    {
        _jumpStrength = strength * _jumpStrengthMultiplier;
    }

    private void CheckGrounded()
    {
        var size = new Vector2(0.5f, 0.05f);
        var center = (Vector2) transform.position - new Vector2(0f, 0.02f + size.y / 2f);
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
        var verticalVelocity = _rb.velocity * Vector2.up;
        _rb.velocity = _movementInput * _speed * Vector2.right + verticalVelocity;
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
        _rb.velocity = new Vector2(currentVelocity.x, _jumpStrength);
    }

    public void Cheer()
    {
        animator.SetTrigger(_cheerHash);
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
