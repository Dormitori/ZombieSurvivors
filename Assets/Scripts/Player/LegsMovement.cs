using UnityEngine;
using UnityEngine.InputSystem;

public class LegsMovement : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] private float rotationSpeed = 2f;
    
    private Animator _animator;
    private InputAction _moveAction;
    private SpriteRenderer _spriteRenderer;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator.Play("Legs");
    }

    void Update()
    {
        var moveValue = _moveAction.ReadValue<Vector2>();
        PlayLegsAnimation(moveValue);
        RotateLegs(moveValue);
    }

    private void RotateLegs(Vector2 moveValue)
    {
        var targetRotation = Vector2.Angle(moveValue, new Vector2(0, -1)) * Mathf.Sign(moveValue.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetRotation, rotationSpeed * Time.deltaTime));
    }

    private void PlayLegsAnimation(Vector2 moveValue)
    {
        if (moveValue.x != 0 || moveValue.y != 0)
            _spriteRenderer.enabled = true;
        else
            _spriteRenderer.enabled = false;

    }
}
