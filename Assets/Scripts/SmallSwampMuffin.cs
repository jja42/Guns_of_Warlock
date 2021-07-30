using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSwampMuffin : Enemy
{
    public GameObject projectilePrefab;
    protected override void AttackPlayer()
    {
        if(attack_timer <= 0)
            Shoot();
        else
        {
            attack_timer -= Time.deltaTime;
        }
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
        death_timer = .25f;
        attack_timer = 1f;
    }

    // Update is called once per frame
    protected override void Move()
    {
        //if (transform.position.x < targetpos.x)
        //{
        //    transform.position = new Vector3(transform.position.x + movespeed * Time.deltaTime, transform.position.y, transform.position.z);
        //}
        //if (transform.position.x > targetpos.x)
        //{
        //    transform.position = new Vector3(transform.position.x - movespeed * Time.deltaTime, transform.position.y, transform.position.z);
        //}
        //if (Vector3.Distance(transform.position, targetpos) <= .1f)
        //{
        //    render.flipX = !render.flipX;
        //    idle = true;
        //}
        //player_spotted = DetectPlayer();
    }
    protected void Shoot()
    {
        GameObject shot = Instantiate(projectilePrefab);
        SpriteRenderer renderer = shot.GetComponent<SpriteRenderer>();
        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
        if (!render.flipX)
        {
            shot_rigid.AddForce(new Vector2(500, 0));
            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y, -.1f);
        }
        else
        {
            renderer.flipX = true;
            shot_rigid.AddForce(new Vector2(-500, 0));
            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y, -.1f);
        }
        Destroy(shot, 2);
        attack_timer = 1f;
        player_spotted = DetectPlayer();
    }
}
