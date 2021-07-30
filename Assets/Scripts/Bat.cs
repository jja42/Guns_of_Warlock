using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    public float x_offset;
    public float y_offset;
    public float speed;
    private float moveTime;
    private float distance; //Distance between the positions.
    float lerpval;
    protected override void Start()
    {
        base.Start();
        
        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 3.0f;
        death_timer = .25f;
        targetpos = new Vector3(start_pos.x+x_offset, start_pos.y+y_offset);
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
            transform.position = Vector3.MoveTowards(transform.position, targetpos,movespeed*Time.deltaTime);
            if (Vector3.Distance(transform.position, targetpos) <= .1f)
            {
                targetpos = new Vector3(start_pos.x - x_offset, start_pos.y - y_offset);
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
