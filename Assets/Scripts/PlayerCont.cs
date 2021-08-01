using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCont : MonoBehaviour
{
    private float x;
    private float y;
    bool db_jump;
    private float walkspeed = 3.5f;
    private Rigidbody2D rigidbody2d;
    public int jumpforce;
    private BoxCollider2D boxCollider;
    public LayerMask ground_layer;
    public bool grounded;
    private Animator animator;
    public GameObject AcidShot;
    public GameObject WaterBolt;
    public GameObject Fireball;
    GameObject projectile;
    private float shot_timer;
    public float invincibility_timer;
    private SpriteRenderer render;
    private AudioSource audioSource;
    public AudioClip jump;
    public AudioClip shoot;
    public AudioClip death;
    public AudioClip coin_collect;
    public AudioClip[] hurt = new AudioClip[11];
    private GameObject npc;
    public GameObject q_mark;
    public GameObject coin;
    public AnimatorOverrideController shotty_animator;
    public Vector3 startpos;
    bool dead;
    Color col;
    void Start()
    {
        q_mark = Instantiate(q_mark);
        coin = Instantiate(coin);
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        grounded = false;
        animator = GetComponent<Animator>();
        shot_timer = 0;
        invincibility_timer = 0;
        q_mark.SetActive(false);
        coin.SetActive(false);
        startpos = transform.position;
        if (Game_Manager.instance.CanGetPosition())
        {
            transform.position = Game_Manager.instance.GetPosition();
        }
        db_jump = true;
        col = render.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Data_Manager.instance.Flags[4])
        {
            projectile = AcidShot;
        }
        if(Data_Manager.instance.Flags[4] && !Data_Manager.instance.Flags[8])
        {
            projectile = WaterBolt;
        }
        if (Data_Manager.instance.Flags[8])
        {
            projectile = Fireball;
        }
        if (!Game_Manager.instance.paused && !dead)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
               StartCoroutine(Respawn());
            }
            if (Game_Manager.instance.invisible)
            {
                invincibility_timer = 2;
            }
            if (Data_Manager.instance.Flags[5])
            {
                animator.runtimeAnimatorController = shotty_animator;
            }
            if(Game_Manager.instance.player_health <= 0 || transform.position.y < -10)
            {
                StartCoroutine(Respawn());
            }
            //health_bar.value = health;
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            grounded = IsGrounded();
            if (grounded)
                db_jump = true;
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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (grounded)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(jump);
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpforce);
                }
                if(!grounded && db_jump && Data_Manager.instance.Flags[9])
                { 
                    audioSource.Stop();
                    audioSource.PlayOneShot(jump);
                    db_jump = false;
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpforce);
                }
            }

            if (rigidbody2d.velocity.y < 0 && rigidbody2d.velocity.y > -10) { //if falling, fall faster, to an extent
                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (1.5f) * Time.deltaTime;
            }

                //Shooting
                if (shot_timer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    if (Data_Manager.instance.Flags[5])
                    {
                        shot_timer = .25f;
                        audioSource.Stop();
                        audioSource.PlayOneShot(shoot);
                        GameObject shot = Instantiate(projectile);
                        SpriteRenderer renderer = shot.GetComponent<SpriteRenderer>();
                        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
                        if (!render.flipX)
                        {
                            shot_rigid.AddForce(new Vector2(500, 0));
                            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y);
                        }
                        else
                        {
                            renderer.flipX = true;
                            shot_rigid.AddForce(new Vector2(-500, 0));
                            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y);
                        }
                        Destroy(shot, 2);
                        shot = Instantiate(projectile);
                        renderer = shot.GetComponent<SpriteRenderer>();
                        shot_rigid = shot.GetComponent<Rigidbody2D>();
                        if (!render.flipX)
                        {
                            shot_rigid.AddForce(new Vector2(500, 100));
                            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y);
                        }
                        else
                        {
                            renderer.flipX = true;
                            shot_rigid.AddForce(new Vector2(-500, 100));
                            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y);
                        }
                        Destroy(shot, 2);
                        shot = Instantiate(projectile);
                        renderer = shot.GetComponent<SpriteRenderer>();
                        shot_rigid = shot.GetComponent<Rigidbody2D>();
                        if (!render.flipX)
                        {
                            shot_rigid.AddForce(new Vector2(500, 50));
                            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y);
                        }
                        else
                        {
                            renderer.flipX = true;
                            shot_rigid.AddForce(new Vector2(-500, 50));
                            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y);
                        }
                        Destroy(shot, 2);
                    }
                    else
                    {
                        shot_timer = .25f;
                        audioSource.Stop();
                        audioSource.PlayOneShot(shoot);
                        GameObject shot = Instantiate(projectile);
                        SpriteRenderer renderer = shot.GetComponent<SpriteRenderer>();
                        Rigidbody2D shot_rigid = shot.GetComponent<Rigidbody2D>();
                        if (!render.flipX)
                        {
                            shot_rigid.AddForce(new Vector2(500, 0));
                            shot.transform.position = new Vector3(transform.position.x + .8f, transform.position.y);
                        }
                        else
                        {
                            renderer.flipX = true;
                            shot_rigid.AddForce(new Vector2(-500, 0));
                            shot.transform.position = new Vector3(transform.position.x - .8f, transform.position.y);
                        }
                        Destroy(shot, 2);
                    }
                }
            }
            else
            {
                shot_timer -= Time.deltaTime;
            }

            //Invincibility
            if (invincibility_timer > 0)
            {
                if (!Game_Manager.instance.invisible)
                {
                    render.enabled = !render.enabled;
                }
                else
                {
                    render.color = new Color(col.r, col.g, col.b, .5f);
                }
                invincibility_timer-= Time.deltaTime;
                gameObject.layer = LayerMask.NameToLayer("Invulnerable");
            }
            else
            {
                render.color = col;
                render.enabled = true;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
            NPCFind();
            if (Input.GetKeyDown(KeyCode.Return) && DialogueManager.instance.talking)
            {
                if (DialogueManager.instance.more_dialogue)
                {
                    DialogueManager.instance.ContinueStory();
                }
                else
                {
                    DialogueManager.instance.ExitStory();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return) && !DialogueManager.instance.talking)
                {
                    if (npc)
                    {
                        if (npc.CompareTag("Shop"))
                        {
                            Game_Manager.instance.ToggleShop();
                        }
                        else
                        {
                            if (npc.CompareTag("Exit"))
                            {
                                string resultString = Regex.Match(npc.name, @"\d+").Value;
                                int scene_num = int.Parse(resultString);
                                Game_Manager.instance.SetSpawn();
                                Game_Manager.instance.LoadScene(scene_num);
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
                if (!(npc.CompareTag("Shop") || npc.CompareTag("Exit")))
                {
                    q_mark.SetActive(true);
                    coin.SetActive(false);
                    q_mark.transform.position = new Vector3(npc.transform.position.x, npc.transform.position.y + 1, npc.transform.position.z);
                }
                else
                {
                    if (npc.CompareTag("Shop"))
                    {
                        coin.SetActive(true);
                        q_mark.SetActive(false);
                        coin.transform.position = new Vector3(npc.transform.position.x + .35f, npc.transform.position.y + 1, npc.transform.position.z);
                    }
                }
            }
        }
        else
        {
            npc = null;
            q_mark.SetActive(false);
            coin.SetActive(false);
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
            if (collision.gameObject.CompareTag("Spikes"))
            {
                StartCoroutine(Respawn());
                return;
            }
            //Enemy enemy = (Enemy)collision.gameObject.GetComponent(typeof(Enemy));
            if (invincibility_timer <= 0)
            {   
                Game_Manager.instance.player_health -= 1;
                invincibility_timer = 1.2f;
                if (render.flipX)
                {
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x + 4, 3);
                }
                else
                {
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x - 4, 3);
                }
                int rand_index = Random.Range(0, 10);
                audioSource.PlayOneShot(hurt[rand_index]);
            }
        }
        if (collision.gameObject.CompareTag("Exit") && collision.gameObject.layer != LayerMask.NameToLayer("NPC"))
        {
            string resultString = Regex.Match(collision.gameObject.name, @"\d+").Value;
            int scene_num = int.Parse(resultString);
            Game_Manager.instance.LoadScene(scene_num);
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.parent = collision.gameObject.transform;
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            Game_Manager.instance.money += 20;
            audioSource.PlayOneShot(coin_collect);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")){
            transform.parent = null;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Juice") && !Data_Manager.instance.Flags[7])
        {
            Game_Manager.instance.player_health -= 1;
        }
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Game_Manager.instance.SetSpawn();
            startpos = collision.gameObject.transform.position + Vector3.up;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Enemy enemy = (Enemy)collision.gameObject.GetComponent(typeof(Enemy));
            if (invincibility_timer <= 0)
            {
                Game_Manager.instance.player_health -= 1;
                invincibility_timer = 1.2f;
                if (render.flipX)
                {
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x + 4, 3);
                }
                else
                {
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x - 4, 3);
                }
                int rand_index = Random.Range(0, 10);
                audioSource.PlayOneShot(hurt[rand_index]);
            }
        }
    }

    public IEnumerator Respawn()
    {
        Game_Manager.instance.player_lives -= 1;
        Game_Manager.instance.player_health = 3;
        if (Game_Manager.instance.Greg)
        {
            boxCollider.enabled = false;
            render.enabled = false;
            audioSource.PlayOneShot(hurt[10]);
            Game_Manager.instance.Greg = false;
            yield return new WaitForSeconds(hurt[10].length/2);
            Game_Manager.instance.LoadScene(5);
            yield return new WaitForSeconds(2);
        }
        else
        {
            audioSource.PlayOneShot(death);
        }
        boxCollider.enabled = false;
        render.enabled = false;
        dead = true;
        yield return new WaitForSeconds(death.length/2);
        invincibility_timer = 1.2f;
        boxCollider.enabled = true;
        render.enabled = true;
        transform.position = startpos;
        CameraController.instance.gameObject.transform.position = new Vector3(transform.position.x, 0, -10);
        rigidbody2d.velocity = Vector2.zero;
        dead = false;
    }
}
