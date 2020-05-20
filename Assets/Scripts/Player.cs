using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const string GROUND_LAYER = "Ground";
    const string LADDER_LAYER = "Ladders";

    [SerializeField] Animator animator = null;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbSpeed = 3f;

    Rigidbody2D body;
    SpriteRenderer sprite;
    BoxCollider2D playerCollider;
    float distToGround;
    bool isClimbing;
    float startGravity;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        isClimbing = false;
        startGravity = body.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        Run();
        Jump();
        Climb();
    }

    private void Animate()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //between +1 and -1
        if (IsOnLadder())
        {
            Debug.Log("climb");
            animator.SetTrigger("Climb");
        }
        else if(controlThrow != 0)
        {
            Debug.Log("walk");
            animator.SetTrigger("Walk");
        }
        else
        {
            Debug.Log("idle");
            animator.SetTrigger("Idle");
        }
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //between +1 and -1
        if (controlThrow != 0)
        {
            sprite.flipX = (controlThrow < 0);
            body.velocity = new Vector2(controlThrow * moveSpeed, body.velocity.y);
        }
    }

    private void Jump()
    {
        bool jumpButton = Input.GetButton("Fire1");
        if (jumpButton && IsGrounded())
        {
            body.velocity = Vector2.up * jumpForce;
        }
    }

    private void Climb()
    {
        if (IsOnLadder())
        {
            float controlThrow = Input.GetAxis("Vertical"); //between +1 and -1
            body.velocity = new Vector2(body.velocity.x, controlThrow * climbSpeed);
            body.gravityScale = 0;
        }
        else
        {
            body.gravityScale = startGravity;
        }
    }

    private bool IsOnLadder()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask(LADDER_LAYER));
    }

    private bool IsGrounded()
    {
        var distToGround = playerCollider.bounds.extents.y;
        //Debug.DrawRay((Vector2)(transform.position), Vector2.down, Color.green);
        return Physics2D.Raycast((Vector2)(transform.position), Vector2.down, distToGround + 0.5f, LayerMask.GetMask(GROUND_LAYER));
    }

 }
