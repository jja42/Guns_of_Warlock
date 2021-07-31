using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeadowMobster : Enemy
{
    public GameObject projectilePrefab;
    public float x_offset;
    public float y_offset;
    public AudioClip shoot;
    float jumpforce = 6;
    public float jump_timer;
    protected override void AttackPlayer()
    {
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", true);
        if (attack_timer <= 0)
            Shoot();
        else
        {
            attack_timer -= Time.deltaTime;
        }
        player_spotted = DetectPlayer();
    }

    protected override void Idle()
    {
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 2.0f;
        death_timer = .25f;
        attack_timer = .1f;
        aggro = true;
        targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
        playerdetectdist = 8;
        jump_timer = 1;
    }
    protected override void Update()
    {
        base.Update();
        if (!Game_Manager.instance.paused)
        {
            if (health > 0)
            {
                if(jump_timer <= 0 && grounded)
                {
                    Jump();
                    jump_timer = 2;
                }
                else
                {
                    jump_timer -= Time.deltaTime;
                }
            }
        }
    }
    // Update is called once per frame
    protected override void Move()
    {
        attack_timer = .1f;
        animator.SetBool("Shooting", false);
        animator.SetBool("Moving", true);
        if (!render.flipX)
        {
            transform.position += Vector3.right * Time.deltaTime * movespeed;
        }
        else
        {
            transform.position += Vector3.left * Time.deltaTime * movespeed;
        }
        if (Mathf.Abs(transform.position.x - targetpos.x) <= .1f)
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
    protected void Shoot()
    {
        GameObject shot = Instantiate(projectilePrefab);
        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
        audioSource.PlayOneShot(shoot);
        if (!render.flipX)
        {
            shot_rigid.AddForce(new Vector2(500, 0));
            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y);
        }
        else
        {
            shot_rigid.AddForce(new Vector2(-500, 0));
            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y);
        }
        Destroy(shot, 2);
        attack_timer = 1f;
    }
    protected void Jump()
    {
        rigidbod.velocity = new Vector2(rigidbod.velocity.x, jumpforce);
    }
}
