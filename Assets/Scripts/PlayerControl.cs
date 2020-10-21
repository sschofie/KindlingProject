﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//? Dan comments
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    private Rigidbody2D rb;
    private CircleCollider2D cc; //? maybe keep as circle, should not matter much
    private float speed = 7f;
    private float jumpVelo = 17f;
    private float jumpForce = 500f;
    private Boolean hasRejump = true; //?

    private Vector3 origScale;
    private bool isjumpedin = false;
    private bool canjumpin = false;
    private bool isjumping = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        origScale = transform.localScale;
    }

    void Update()
    {
        float horzIn = Input.GetAxis("Horizontal");
        if(!isjumpedin) {
            transform.position = transform.position + new Vector3(horzIn * speed * Time.deltaTime, 0, 0);
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpVelo;
        }
        else if (hasRejump && Input.GetKeyDown(KeyCode.Space))
        {
            hasRejump = false;
            rb.velocity = Vector2.up * jumpVelo;
        }
        //? relic of old game
        /*else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.velocity = (Vector2.up * jumpVelo * -1);
        }*/

        if(canjumpin && Input.GetKeyDown(KeyCode.DownArrow)) {
            this.gameObject.transform.localScale = new Vector3(0,0,0);
            isjumpedin = true;
        }

        if(isjumpedin && Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("Jumped out");
            this.gameObject.transform.localScale = origScale;
            isjumpedin = false;
            canjumpin = false;
            transform.parent = null;
        }

        if(transform.parent != null) {
            transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        }

        if (IsGrounded())
        {
            refreshJump();
        }
    }

    //private void OnTriggerEnter2D(Collider2D collider)

    public void refreshJump() { hasRejump = true; }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.CircleCast(cc.bounds.center, cc.radius, Vector2.down, .01f, groundMask);
        //Debug.Log(raycastHit2D.collider);
        return raycastHit2D.collider != null;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if("CanJumpIn".Equals(col.gameObject.tag)) {
            Debug.Log("Iscolliding");
            if(!isjumpedin) {
                canjumpin = true;
            }
            if(isjumpedin) {
                transform.SetParent(col.transform);
            }
        }
    }
}
