using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeadowSlime : Enemy
{
    public float x_offset;
    public float y_offset;
    public float speed;
    protected override void Start()
    {
        base.Start();

        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 2.0f;
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
        }
        else
        {
            transform.position += Vector3.left * Time.deltaTime * movespeed;
        }
        if (Vector3.Distance(transform.position, targetpos) <= .1f)
        {
            x_offset = -x_offset;
            y_offset = -y_offset;
            targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
            render.flipX = !render.flipX;
            movetimer = movetimer_og;
        }
        if (DetectWall())
        {
            x_offset = -x_offset;
            y_offset = -y_offset;
            targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
            render.flipX = !render.flipX;
        }
    }

    protected override void AttackPlayer()
    {
        Move();
    }
}
