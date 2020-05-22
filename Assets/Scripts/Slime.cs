using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] float moveSpeed = .5f;

    bool facingleft = true;
    SpriteRenderer sprite = null;
    Rigidbody2D body = null;
    CapsuleCollider2D capCollider = null;
    BoxCollider2D boxCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        facingleft = true;
        sprite = GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (facingleft)
        {
            //sprite.flipX = false;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            body.velocity = new Vector2(-moveSpeed, body.velocity.y);


        }
        else
        {
            //sprite.flipX = true;
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            body.velocity = new Vector2(moveSpeed, body.velocity.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        facingleft = !facingleft;
    }
}
