using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const string GROUND_LAYER = "Ground";
    const string LADDER_LAYER = "Ladders";
    const string INTERACTABLE_LAYER = "Interactables";
    const int ENEMY_LAYER = 12;
    const int HAZARDS_LAYER = 13;

    [SerializeField] Animator animator = null;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float damageVelocity = 5f;

    Door exitDoor = null;
    Collider2D exitDoorCollider;
    Rigidbody2D body;
    SpriteRenderer sprite;
    BoxCollider2D playerCollider;
    float distToGround;
    bool isClimbing;
    float startGravity;
    bool isDead;
    bool isJumping;
    bool isLeaving;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        //isClimbing = false;
        startGravity = body.gravityScale;
        isDead = false;
        isJumping = false;
        isLeaving = false;
        exitDoor = FindObjectOfType<Door>();
        if(exitDoor != null)
            exitDoorCollider = exitDoor.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !isLeaving)
        {
            Animate();
            Run();
            Jump();
            Climb();
            Interact();
        }
    }


    private void Animate()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //between +1 and -1
        if (IsOnLadder())
        {
            animator.SetTrigger("Climb");
        }
        else if(!IsGrounded())
        {
            animator.SetTrigger("Jump");
        }
        else if(controlThrow != 0)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //between +1 and -1
        if (controlThrow != 0)
        {
            //sprite.flipX = (controlThrow < 0);
            var facingSign = Mathf.Sign(controlThrow) == Mathf.Sign(transform.localScale.x) ? 1 : -1;
            transform.localScale = new Vector3(facingSign * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            body.velocity = new Vector2(controlThrow * moveSpeed, body.velocity.y);
        }
    }

    private void Jump()
    {
        bool jumpButton = Input.GetButton("Fire1");
        if (jumpButton && (IsGrounded() || IsOnLadder()) )
        {
            body.velocity = Vector2.up * jumpForce;
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    private void Climb()
    {
        if (IsOnLadder() && !isJumping)
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

    private void Interact()
    {
        if (IsAtDoor() && Input.GetAxis("Vertical") > 0)
        {
            isLeaving = true;
            animator.SetTrigger("Exit");
            body.velocity = new Vector2(0f, 0f);
            exitDoor.Exit();
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

    private bool IsAtDoor()
    {
        if (exitDoorCollider == null) return false;
        return playerCollider.IsTouching(exitDoorCollider);        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == ENEMY_LAYER)
        {
            isDead = true;
            var heading = transform.position - collision.transform.position;
            var direction = heading / heading.magnitude;
            body.velocity = new Vector2(damageVelocity, damageVelocity) * direction;
            animator.SetTrigger("Hurt");
            StartCoroutine(Restart());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == HAZARDS_LAYER)
        {
            isDead = true;
            animator.SetTrigger("Hurt");
            StartCoroutine(Restart());
        }
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        FindObjectOfType<Session>().PlayerDied();
    }

}
