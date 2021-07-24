using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        render = GetComponent<SpriteRenderer>();
        health = 50;
        movetimer_og = 1;
        movetimer = .1f;
        moveforce = new Vector2(0f, 0f);
        movespeed = 3.0f;
        player_layer = LayerMask.GetMask("Player");
        ground_layer = LayerMask.GetMask("Ground");
        if (transform.eulerAngles.y == 180)
        {
            flip = true;
        }
        else
        {
            flip = false;
        }
        death_timer = .25f;
        start_pos = transform.position;
        idle = true;
    }
    protected override void Update()
    {
        if (!Game_Manager.instance.paused)
        {
            if (health <= 0)
            {
                OnDeath();
            }
            else
            {
                //player_spotted = DetectPlayer();
                if (player_spotted)
                {
                    ChasePlayer();
                }
                else
                {
                    if (movetimer <= 0)
                    {
                        idle = false;
                        if (flip)
                        {
                            transform.eulerAngles = new Vector3(0, 180, 0);
                            targetpos = new Vector3(transform.position.x - 10, transform.position.y);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            targetpos = new Vector3(transform.position.x + 10, transform.position.y);
                        }
                        movetimer = movetimer_og;
                    }
                    if (idle)
                    {
                        movetimer -= Time.deltaTime;
                        Idle();
                    }
                    else
                    { 
                        Wander();
                    }
                }
            }
        }
    }
    protected override void ChasePlayer()
    {
        throw new System.NotImplementedException();
    }

    protected override void Idle()
    {
        return;
    }

    protected override void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    protected override void Wander()
    {
        if(transform.position.x < targetpos.x)
        {
            transform.position = new Vector3 (transform.position.x + movespeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (transform.position.x > targetpos.x)
        {
            transform.position = new Vector3(transform.position.x - movespeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (Vector3.Distance(transform.position, targetpos) <= .1f)
        {
            flip = !flip;
            idle = true;
        } 
    }
}
