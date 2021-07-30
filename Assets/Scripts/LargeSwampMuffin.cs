using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSwampMuffin : Enemy
{
    public GameObject projectilePrefab;
    public float x_offset;
    public float y_offset;
    public AudioClip shoot;
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
        attack_timer = .25f;
        aggro = true;
        targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
        playerdetectdist = 8;
    }

    // Update is called once per frame
    protected override void Move()
    {
        attack_timer = .25f;
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
    protected void Shoot()
    {
        GameObject shot = Instantiate(projectilePrefab);
        SpriteRenderer renderer = shot.GetComponent<SpriteRenderer>();
        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
        audioSource.PlayOneShot(shoot);
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
        shot = Instantiate(projectilePrefab);
        renderer = shot.GetComponent<SpriteRenderer>();
        shot_rigid = shot.GetComponent<Rigidbody2D>();
        if (!render.flipX)
        {
            shot_rigid.AddForce(new Vector2(500, 200));
            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y, -.1f);
        }
        else
        {
            renderer.flipX = true;
            shot_rigid.AddForce(new Vector2(-500, 200));
            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y, -.1f);
        }
        Destroy(shot, 2);
        attack_timer = 1f;
        shot = Instantiate(projectilePrefab);
        renderer = shot.GetComponent<SpriteRenderer>();
        shot_rigid = shot.GetComponent<Rigidbody2D>();
        if (!render.flipX)
        {
            shot_rigid.AddForce(new Vector2(500, 100));
            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y, -.1f);
        }
        else
        {
            renderer.flipX = true;
            shot_rigid.AddForce(new Vector2(-500, 100));
            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y, -.1f);
        }
        Destroy(shot, 2);
        attack_timer = 1f;
    }
}
