using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Animator animator;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }
    private void FixedUpdate()
    {
        Move();
        Animate();
    }
    void Move()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.fixedDeltaTime);
    }
    void OnMove(InputValue moveValue)
    {
        movementInput = moveValue.Get<Vector2>();
    }
    
    void Animate()
    {
        if (movementInput != Vector2.zero)
        {
            animator.SetFloat("xMove", movementInput.x);
            animator.SetFloat("yMove", movementInput.y);
        }
        animator.SetFloat("magniMove", movementInput.magnitude);
    }

}
