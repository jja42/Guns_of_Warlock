using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bichon : Enemy
{
    public float x_offset;
    public float y_offset;
    float jumpforce = 2.5f;
    public float jump_timer;
    bool init;
    GameObject player;
    protected override void AttackPlayer()
    {
        animator.SetBool("Moving", true);
        if (Mathf.Abs(transform.position.x - player.transform.position.x) < 2)
        {
            if (render.flipX)
                Jump(false);
            else
                Jump(true);
            return;
        }
        if(transform.position.x < player.transform.position.x)
        {
            transform.position += Vector3.right * Time.deltaTime * movespeed;
            render.flipX = false;
        }
        if(transform.position.x > player.transform.position.x)
        {
            transform.position += Vector3.left * Time.deltaTime * movespeed;
            render.flipX = true;
        }
    }

    protected override void Idle()
    {
        animator.SetBool("Moving", false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = 6;
        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 3.0f;
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
            if (player_spotted && !init)
            {
                init = true;
            }
        }
    }
    // Update is called once per frame
    protected override void Move()
    {
        if (init)
        {
            animator.SetBool("Moving", true);
            if (transform.position.x < targetpos.x)
            {
                transform.position += Vector3.right * Time.deltaTime * movespeed;
                render.flipX = false;
            }
            if(transform.position.x > targetpos.x)
            {
                transform.position += Vector3.left * Time.deltaTime * movespeed;
                render.flipX = true;
            }
            if (Mathf.Abs(transform.position.x - targetpos.x) <= .1f)
            {
                x_offset = -x_offset;
                y_offset = -y_offset;
                targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
                movetimer = movetimer_og;
            }
            if (DetectWall())
            {
                x_offset = -x_offset;
                y_offset = -y_offset;
                targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
            }
        }
    }
    protected void Jump(bool dir)
    {
        if(dir)
            rigidbod.velocity = new Vector2(jumpforce, jumpforce);
        else
            rigidbod.velocity = new Vector2(-jumpforce, jumpforce);
    }

    protected override bool DetectPlayer()
    {
        RaycastHit2D raycastHit;
        if (render.flipX) raycastHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y - boxCollider.bounds.extents.y / 2), Vector2.left, boxCollider.bounds.extents.x + playerdetectdist, player_layer);
        else
        {
            raycastHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y - boxCollider.bounds.extents.y / 2), Vector2.right, boxCollider.bounds.extents.x + playerdetectdist, player_layer);
        }
        Color Raycolor;

        if (raycastHit.collider != null)
        {
            Raycolor = Color.green;
        }
        else
        {
            Raycolor = Color.red;

        }
        if (render.flipX) Debug.DrawRay(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y - boxCollider.bounds.extents.y / 2), Vector2.left * (boxCollider.bounds.extents.x + playerdetectdist), Raycolor);
        if (!render.flipX) Debug.DrawRay(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y - boxCollider.bounds.extents.y / 2), Vector2.right * (boxCollider.bounds.extents.x + playerdetectdist), Raycolor);
        if (raycastHit.collider != null)
        {
            player = raycastHit.collider.gameObject;
        }
        return (raycastHit.collider != null);
    }
}
