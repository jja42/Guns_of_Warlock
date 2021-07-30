using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    protected override void Start()
    {
        base.Start();
        
        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 3.0f;
        death_timer = .25f;
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Idle()
    {
        return;
    }

    protected override void Move()
    {
        if (movetimer <= 0)
        {
            if (transform.position.x < targetpos.x)
            {
                transform.position = new Vector3(transform.position.x + movespeed * Time.deltaTime, transform.position.y);
            }
            if (transform.position.x > targetpos.x)
            {
                transform.position = new Vector3(transform.position.x - movespeed * Time.deltaTime, transform.position.y);
            }
            if (Vector3.Distance(transform.position, targetpos) <= .1f)
            {
                targetpos = new Vector3(-targetpos.x, targetpos.y);
                render.flipX = !render.flipX;
                movetimer = movetimer_og;
            }
        }
        else
        {
            movetimer -= Time.deltaTime;
        }
    }

    protected override void AttackPlayer()
    {
        Move();
    }
}
