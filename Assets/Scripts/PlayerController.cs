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

    private static int _isWalkingHash = Animator.StringToHash("IsWalking");

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
        HandleMovement();
    }

    private void HandleMovement()
    {
        Debug.Log(_movementInput.ToString(CultureInfo.InvariantCulture));
        if (Mathf.Abs(_movementInput) <= 0.01f)
        {
            animator.SetBool(_isWalkingHash, false);
            return;
        }

        sprite.flipX = _movementInput < 0;
        animator.SetBool(_isWalkingHash, true);
        _rb.velocity = Vector2.right * _movementInput;
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<float>();
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                break;
            // case InputActionPhase.Canceled:
            //     _movementInput = Vector2.zero;
            //     break;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _Jump = true;
        }
    }
}
