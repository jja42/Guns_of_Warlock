using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCont : MonoBehaviour
{
    private float x;
    private float y;
    private float walkspeed = 3.5f;
    private Rigidbody2D rigidbody2d;
    private Vector2 jumpforce;
    private BoxCollider2D boxCollider;
    public LayerMask ground_layer;
    public bool grounded;
    private Animator animator;
    public GameObject Fireball;
    private float shot_timer;
    public int health;
    public float invincibility_timer;
    public Slider health_bar;
    private SpriteRenderer render;
    private AudioSource audioSource;
    public AudioClip jump;
    public AudioClip shoot;
    public AudioClip hurt;
    public int idle_timer;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        jumpforce = new Vector2(0, 400);
        grounded = false;
        animator = GetComponent<Animator>();
        shot_timer = 0;
        health = 100;
        invincibility_timer = 0;
        idle_timer = 400;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game_Manager.instance.paused)
        {
            //health_bar.value = health;
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            grounded = IsGrounded();
            // Horizontal Movement
            if (x != 0)
            {
                animator.SetBool("Running", true);
                if (x > 0.01)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                if (x < -0.01)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                transform.position = new Vector3(transform.position.x + x * walkspeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                animator.SetBool("Running", false);
            }

            // Vertical Movement
            if (Input.GetKeyDown(KeyCode.Z) && grounded)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(jump);
                rigidbody2d.AddForce(jumpforce);
            }

            //Shooting
            if (shot_timer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    shot_timer = .25f;
                    audioSource.Stop();
                    audioSource.PlayOneShot(shoot);
                    GameObject shot = Instantiate(Fireball);
                    shot.transform.eulerAngles = transform.eulerAngles;
                    Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
                    if (shot.transform.eulerAngles.y == 0)
                    {
                        shot_rigid.AddForce(new Vector2(500, 0));
                        shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y, -.1f);
                    }
                    if (shot.transform.eulerAngles.y == 180)
                    {
                        shot_rigid.AddForce(new Vector2(-500, 0));
                        shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y, -.1f);
                    }
                    Destroy(shot, 2);
                }
            }
            else
            {
                shot_timer -= Time.deltaTime;
            }

            //Invincibility
            if (invincibility_timer > 0)
            {
                if (invincibility_timer % 2 == 0) render.enabled = false;
                else
                {
                    render.enabled = true;
                }
                invincibility_timer-= Time.deltaTime;
            }
        }
    }

    private bool IsGrounded()
    {
        float extraheight = .04f;
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + extraheight, ground_layer);
        Color Raycolor;

        if (raycastHit.collider != null)
        {
            Raycolor = Color.green;
            animator.SetBool("Jumping", false);
        }
        else
        {
            Raycolor = Color.red;
            animator.SetBool("Jumping", true);
        }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + extraheight), Raycolor);
        return (raycastHit.collider != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Enemy enemy = (Enemy)collision.gameObject.GetComponent(typeof(Enemy));
            if (invincibility_timer <= 0)
            {
                health -= 20;
                invincibility_timer = 1.2f;
                audioSource.PlayOneShot(hurt);
            }
        }
    }
}
