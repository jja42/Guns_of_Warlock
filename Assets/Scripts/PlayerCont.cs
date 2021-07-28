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
    private GameObject npc;
    public GameObject q_mark;

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
        health = 3;
        invincibility_timer = 0;
        q_mark.SetActive(false);
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
                    render.flipX = false;
                }
                if (x < -0.01)
                {
                    render.flipX = true;
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

            if (rigidbody2d.velocity.y < 0) { //if falling, fall faster

                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (1.5f) * Time.deltaTime;

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
            NPCFind();
            if (Input.GetKeyDown(KeyCode.Return) && DialogueManager.instance.talking)
            {
                DialogueManager.instance.ExitStory();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return) && !DialogueManager.instance.talking)
                {
                    if (npc)
                    {
                        if (npc.name.Equals("ShopKeeper"))
                        {
                            Shop shop = npc.GetComponent<Shop>();
                            shop.ToggleShop();
                            Game_Manager.instance.shopping = !Game_Manager.instance.shopping;
                        }
                        else
                        {
                            DialogueManager.instance.PlayDialogue(npc.name);
                        }
                    }
                }
            }
            
        }
    }
    void NPCFind()
    {
        float radius = 0.7f;
        LayerMask npc_mask = LayerMask.GetMask("NPC");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, npc_mask);
        if (hitColliders.Length > 0)
        {
            if (hitColliders[0].gameObject != npc)
            {
                npc = hitColliders[0].gameObject;
                q_mark.SetActive(true);
                q_mark.transform.position = new Vector3(npc.transform.position.x, npc.transform.position.y + 1, npc.transform.position.z);
            }
        }
        else
        {
            npc = null;
            q_mark.SetActive(false);
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
