using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linda : Enemy
{
    public GameObject projectilePrefab;
    public float x_offset;
    public float y_offset;
    public AudioClip shoot;
    float jumpforce = 3;
    public float jump_timer;
    public GameObject Blocks;
    public AudioSource music;
    public AudioClip boss_music;
    public AudioClip level_music;
    GameObject player;
    public Transform boss_trigger;
    bool active;
    int lives;
    bool reset;
    bool dir;
    bool in_air;
    float air_timer;
    float air_shot_timer = .25f;
    protected override void AttackPlayer()
    {
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", true);
        if (attack_timer <= 0)
            Shoot();
        else
        {
            attack_timer -= Time.deltaTime;
            Move();
        }
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
        player = GameObject.FindGameObjectWithTag("Player");
        movetimer_og = 1;
        movetimer = .1f;
        movespeed = 2.0f;
        death_timer = .25f;
        attack_timer = .1f;
        health = 30;
        targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
        playerdetectdist = 8;
        jump_timer = 4;
        air_timer = 5;
    }
    protected override void Update()
    {
        if (Data_Manager.instance.Flags[1])
        {
            Destroy(gameObject);
        }
        if (!active && !reset)
        {
            if (player.transform.position.x <= boss_trigger.position.x && player.transform.position.y > boss_trigger.position.y)
            {
                Blocks.SetActive(true);
                music.clip = boss_music;
                music.Play();
                active = true;
                lives = Game_Manager.instance.player_lives;
                player_spotted = true;
            }
        }
        if (active)
        {
            if (Game_Manager.instance.player_lives < lives)
            {
                active = false;
                reset = true;
                StartCoroutine(Reset());
            }
        }
        if (!Game_Manager.instance.paused && active && !in_air)
        {
            if (health > 0)
            {
                if (jump_timer <= 0 && grounded)
                {
                    Jump(dir);
                    jump_timer = 8;
                }
                else
                {
                    jump_timer -= Time.deltaTime;
                }
            }
            base.Update();
        }
        if (active && in_air)
        {
            if (air_timer <= 0)
            {
                in_air = false;
                rigidbod.gravityScale = 1;
            }
            else
            {
                AirMove();
                if (air_shot_timer <= 0)
                {
                    FlyingShoot();
                    air_shot_timer = .35f;
                }
                else
                {
                    air_shot_timer -= Time.deltaTime;
                }
                air_timer -= Time.deltaTime;
            }
        }
    }
    // Update is called once per frame
    protected override void Move()
    {
        if (active)
        {
            if (attack_timer > .5f)
            {
                animator.SetBool("Shooting", false);
                animator.SetBool("Moving", true);
            }
            if (transform.position.x < targetpos.x)
            {
                transform.position += Vector3.right * Time.deltaTime * movespeed;
                dir = true;
            }
            if (transform.position.x > targetpos.x)
            {
                transform.position += Vector3.left * Time.deltaTime * movespeed;
                dir = false;
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
            if (transform.position.x < player.transform.position.x)
            {
                render.flipX = false;
            }
            if (transform.position.x > player.transform.position.x)
            {
                render.flipX = true;
            }
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
            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y - 1f);
        }
        else
        {
            shot_rigid.AddForce(new Vector2(-500, 0));
            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y - 1f);
        }
        Destroy(shot, 2);
        shot = Instantiate(projectilePrefab);
        shot_rigid = shot.GetComponent<Rigidbody2D>();
        if (!render.flipX)
        {
            shot_rigid.AddForce(new Vector2(500, 200));
            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y - 1f);
        }
        else
        {
            shot_rigid.AddForce(new Vector2(-500, 200));
            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y - 1f);
        }
        Destroy(shot, 2);
        attack_timer = 1.5f;
    }
    protected void Jump(bool dir)
    {
        animator.SetTrigger("Jumping");
        if (dir)
            rigidbod.velocity = new Vector2(jumpforce, jumpforce * 2f);
        else
            rigidbod.velocity = new Vector2(-jumpforce, jumpforce * 2f);
        rigidbod.gravityScale = 0;
        air_timer = 5;
        in_air = true;
    }
    protected override void OnDeath()
    {
        music.clip = level_music;
        music.Play();
        Blocks.SetActive(false);
        Data_Manager.instance.Flags[1] = true;
        Game_Manager.instance.ActivatePopup();
        base.OnDeath();
    }

    protected override void TakeDamage(int damage)
    {
        if (active)
        {
            base.TakeDamage(damage);
        }
    }
    protected IEnumerator Reset()
    {
        music.Stop();
        in_air = false;
        rigidbod.gravityScale = 1;
        yield return new WaitForSeconds(2);
        music.clip = level_music;
        music.Play();
        Blocks.SetActive(false);
        transform.position = start_pos;
        player_spotted = false;
        health = 30;
        Idle();
        reset = false;
    }
    protected override bool DetectWall()
    {
        float extradist = .2f;
        RaycastHit2D raycastHit;
        if (!dir) raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.x + extradist, ground_layer);
        else
        {
            raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.x + extradist, ground_layer);
        }
        return (raycastHit.collider != null);
    }

    protected void FlyingShoot()
    {
        GameObject shot = Instantiate(projectilePrefab);
        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
        audioSource.PlayOneShot(shoot);
        shot_rigid.AddForce(new Vector2(0, -500));
        shot.transform.position = new Vector3(transform.position.x, transform.position.y - 1f);
        Destroy(shot, 2);
    }
    protected void AirMove()
    {
        if (transform.position.x < targetpos.x)
        {
            transform.position += Vector3.right * Time.deltaTime * movespeed * 2;
            dir = true;
        }
        if (transform.position.x > targetpos.x)
        {
            transform.position += Vector3.left * Time.deltaTime * movespeed * 2;
            dir = false;
        }
        if (Mathf.Abs(transform.position.x - targetpos.x) <= .1f)
        {
            x_offset = -x_offset;
            y_offset = -y_offset;
            targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
            render.flipX = !render.flipX;
            movetimer = movetimer_og;
        }
        if (transform.position.x < player.transform.position.x)
        {
            render.flipX = false;
        }
        if (transform.position.x > player.transform.position.x)
        {
            render.flipX = true;
        }
        if (DetectWall())
        {
            x_offset = -x_offset;
            y_offset = -y_offset;
            targetpos = new Vector3(start_pos.x + x_offset, start_pos.y + y_offset);
            render.flipX = !render.flipX;
        }
    }
}
