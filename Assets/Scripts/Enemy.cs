using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int health;
    public float movetimer;
    protected float movetimer_og;
    protected float movespeed;
    protected Vector2 moveforce;
    protected Vector3 start_pos;
    protected bool player_spotted;
    protected Animator animator;
    protected BoxCollider2D boxCollider;
    protected CircleCollider2D circleCollider;
    protected Rigidbody2D rigidbod;
    protected bool look_dir;
    protected LayerMask player_layer;
    protected bool grounded;
    protected LayerMask ground_layer;
    protected bool flip;
    public float death_timer;
    protected AudioSource audioSource;
    public AudioClip impact;
    protected bool idle;
    protected Vector3 targetpos;
    protected SpriteRenderer render;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rigidbod = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        render = GetComponent<SpriteRenderer>();
        health = 100;
        movetimer_og = 200;
        movetimer = movetimer_og;
        moveforce = new Vector2(0f, 0f);
        movespeed = 2.0f;
        player_layer = LayerMask.GetMask("Player");
        ground_layer = LayerMask.GetMask("Ground");
        flip = false;
        death_timer = 20;
        start_pos = transform.position;
        idle = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!Game_Manager.instance.paused)
        {
            grounded = IsGrounded();
            if (grounded)
            {
                circleCollider.isTrigger = false;
            }
            else
            {
                circleCollider.isTrigger = true;
            }

            if (health <= 0)
            {
                OnDeath();
            }
            else
            {
                //player_spotted = DetectPlayer();
                if (player_spotted)
                {
                    ChasePlayer();
                }
                else
                {
                    if (movetimer == 0)
                    {
                        Wander();
                        movetimer = movetimer_og;
                    }
                    else
                    {
                        movetimer--;
                        Idle();
                    }
                }
            }
        }
    }

    protected abstract void TakeDamage();

    protected bool DetectPlayer()
    {
        float extradist = 4.0f;
        RaycastHit2D raycastHit;
        if (flip) raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.x + extradist, player_layer);
        else
        {
            raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.x + extradist, player_layer);
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
        if (flip) Debug.DrawRay(boxCollider.bounds.center, Vector2.left * (boxCollider.bounds.extents.y + extradist), Raycolor);
        if (!flip) Debug.DrawRay(boxCollider.bounds.center, Vector2.right * (boxCollider.bounds.extents.y + extradist), Raycolor);
        return (raycastHit.collider != null);
    }

    protected abstract void ChasePlayer();
    protected virtual void OnDeath()
    {
        boxCollider.enabled = false;
        render.enabled = false;
        if (death_timer <= 0) Destroy(gameObject);
        else
        {
            death_timer-=Time.deltaTime;
        }
    }

    protected abstract void Idle();

    protected abstract void Wander();

    protected bool IsGrounded()
    {
        float extraheight = .1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(circleCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + extraheight, ground_layer);
        Color Raycolor;

        if (raycastHit.collider != null)
        {
            Raycolor = Color.green;

        }
        else
        {
            Raycolor = Color.red;

        }
        Debug.DrawRay(circleCollider.bounds.center, Vector2.down * (circleCollider.bounds.extents.y + extraheight), Raycolor);
        return (raycastHit.collider != null);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            audioSource.PlayOneShot(impact);
            health -= 50;
            Destroy(collision.gameObject);
        }

    }
}
