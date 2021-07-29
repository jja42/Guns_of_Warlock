using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSwampMuffin : Enemy
{
    public GameObject projectilePrefab;
    protected override void ChasePlayer()
    {
        throw new System.NotImplementedException();
    }

    protected override void Idle()
    {
        return;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 2.0f;
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

    // Update is called once per frame
    protected override void Wander()
    {
        if (transform.position.x < targetpos.x)
        {
            transform.position = new Vector3(transform.position.x + movespeed * Time.deltaTime, transform.position.y, transform.position.z);
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
        player_spotted = DetectPlayer();
    }
}
