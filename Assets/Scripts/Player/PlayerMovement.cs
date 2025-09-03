using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    private Animator animator;
    string currentAnimationState;
    
    private Rigidbody2D rb;

    private Camera cam;
    InputAction moveAction;


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        cam = Camera.main;
        animator = transform.Find("PlayerBody").GetComponent<Animator>();
        currentAnimationState = "Idle";

        var health = GetComponent<Health>();
        health.Death += OnPlayerDeath;
        
        rb = GetComponent<Rigidbody2D>();
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
        LookAt();
    }

    public void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        var moveX = moveValue.x;
        var moveY = moveValue.y;
        
        if (moveX == 0 && moveY == 0) { return; }
        
        Vector2 move = new Vector2(moveX, moveY) * (speed * Time.deltaTime);
        if (move.magnitude > 0)
            ChangeAnimationState("BodyWalking");
        else
            ChangeAnimationState("Idle");
        rb.MovePosition(rb.position + move);
        
    }

    private void LookAt()
    {
        var mousePosition = Input.mousePosition;
        var worldPos = cam.ScreenToWorldPoint(mousePosition);
        var angleRad = Mathf.Atan2(worldPos.y - transform.position.y, worldPos.x - transform.position.x);
        angleRad += Mathf.PI / 2;
        var angleDeg = angleRad * 180 / Mathf.PI ;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
}
