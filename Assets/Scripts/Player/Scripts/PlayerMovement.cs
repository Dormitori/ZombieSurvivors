using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    private Animator animator;
    string currentAnimationState;

    private Camera cam;
    InputAction moveAction;
    InputAction lookAction;


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        cam = Camera.main;
        animator = GetComponent<Animator>();
        currentAnimationState = "Idle";

        var health = GetComponent<Health>();
        health.Death += OnPlayerDeath;
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) { return; }

        animator.Play(newState);

        currentAnimationState = newState;
    }

    private void OnPlayerDeath(string obj)
    {
        enabled = false;
    }

    // Update is called once per frame
    public void Update()
    {
        move();
        lookAt();
    }

    private void move()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveValue.x, moveValue.y, 0);
        if (move.magnitude > 0)
            ChangeAnimationState("BodyWalking");
        else
            ChangeAnimationState("Idle");
        transform.position += move * Time.deltaTime * speed;
    }

    private void lookAt()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePosition);
        worldPos = new Vector3(worldPos.x, worldPos.y, 0);
        transform.up =  transform.position - worldPos;
    }
}
