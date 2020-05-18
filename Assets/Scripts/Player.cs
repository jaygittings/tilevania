using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    Rigidbody2D body;
    SpriteRenderer sprite;
    bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //between +1 and -1
        bool jumpButton = Input.GetButton("Fire1");
        if (jumpButton && !isJumping)
        {
            //Debug.Log("jumped");
            isJumping = true;
            body.AddForce(new Vector2(0f, jumpForce));
        }

        if(controlThrow != 0)
        {
            sprite.flipX = (controlThrow < 0);
            var deltaX = controlThrow * Time.deltaTime * moveSpeed;
            //var newX = transform.position.x + (deltaX);
            //transform.position = new Vector2(newX, transform.position.y);
            body.velocity = new Vector2(controlThrow * moveSpeed, body.velocity.y);
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.position.y < transform.position.y)
        {
            isJumping = false;
        }
    }
}
