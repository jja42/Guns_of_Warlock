using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherTrucker : Enemy
{
    public GameObject projectilePrefab;
    float jumpforce = 8;
    public float jump_timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        death_timer = .25f;
        attack_timer = 1f;
        jump_timer = 1;
        health = 16;
    }
    protected override void Update()
    {
        if (!Game_Manager.instance.paused)
        {
            grounded = IsGrounded();
            if (health > 0)
            {
                if (jump_timer <= 0 && grounded)
                {
                    animator.SetBool("Shooting", false);
                    Jump();
                    jump_timer = 4;
                }
                else
                {
                    jump_timer -= Time.deltaTime;
                }
                if (attack_timer <= 0 && grounded)
                {
                    animator.SetBool("Shooting", true);
                    Shoot();
                }
                else
                {
                    attack_timer -= Time.deltaTime;
                }
            }
            else
            {
                OnDeath();
            }
        }
    }
    // Update is called once per frame
    protected override void Move()
    {
        return;
    }
    protected void Shoot()
    {
        GameObject shot = Instantiate(projectilePrefab);
        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
        shot_rigid.AddForce(new Vector2(500, 0));
        shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y);
        Destroy(shot, 2);

        shot = Instantiate(projectilePrefab);
        shot_rigid = shot.GetComponent<Rigidbody2D>();
        shot_rigid.AddForce(new Vector2(-500, 0));
        shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y);
        Destroy(shot, 2);

        shot = Instantiate(projectilePrefab);
        shot_rigid = shot.GetComponent<Rigidbody2D>();
        shot_rigid.AddForce(new Vector2(0, 500));
        shot.transform.position = new Vector3(transform.position.x, transform.position.y +.8f);
        Destroy(shot, 2);

        attack_timer = 5f;
    }
    protected void Jump()
    {
        rigidbod.velocity = new Vector2(rigidbod.velocity.x, jumpforce);
    }

    protected override void AttackPlayer()
    {
        return;
    }

    protected override void Idle()
    {
        return;
    }
}
