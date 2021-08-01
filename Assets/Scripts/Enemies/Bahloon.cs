using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bahloon : Enemy
{
    public float x_offset;
    public float y_offset;
    public float speed;
    protected override void Start()
    {
        base.Start();
        health = 6;
        movetimer_og = .1f;
        movetimer = .1f;
        movespeed = 4.0f;
        death_timer = .25f;
        targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
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
        if (!render.flipX)
        {
            transform.position += Vector3.right * Time.deltaTime * movespeed;
            transform.position += Vector3.down * Mathf.Sin(Time.time*4) * Time.deltaTime * 5f;
        }
        else
        {
            transform.position += Vector3.left * Time.deltaTime * movespeed;
            transform.position += Vector3.down * Mathf.Sin(Time.time*4) * Time.deltaTime * 5f;
        }
        if (Mathf.Abs(transform.position.x - targetpos.x) <= .1f)
        {
            x_offset = -x_offset;
            y_offset = -y_offset;
            targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
            render.flipX = !render.flipX;
            movetimer = movetimer_og;
        }
    }

    protected override void AttackPlayer()
    {
        Move();
    }
}
