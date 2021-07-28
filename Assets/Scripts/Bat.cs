using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    protected override void Start()
    {
        base.Start();
        
        health = 3;
        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 3.0f;
        if (transform.eulerAngles.y == 180)
        {
            flip = true;
        }
        else
        {
            flip = false;
        }
        death_timer = .25f;
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
