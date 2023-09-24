using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    Animator a;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public float angle = 0;

    public Rigidbody2D projectile;
    public bool hasProjectlie = false;

    public AnimationClip[] anims;
    public AnimationClip[] stills;
    public AnimationClip[] attacks;
    public GameObject[] hitboxes;
    public GameObject activeHitbox;

    public GameObject soulOrb;
    public GameObject healthOrb;

    public int health = 100;

    public bool isAnim = true;
    public bool enemyIsAttacking = false;
    public bool attackCooldown = false;
    public bool isDying = false;


    public enum AngleDirection
    {
        UP = 0,
        RIGHT_UP = 1,
        RIGHT = 2,
        RIGHT_DOWN = 3,
        DOWN = 4,
        LEFT_DOWN = 5,
        LEFT = 6,
        LEFT_UP = 7
    }

    public AngleDirection currentAngleDirection = AngleDirection.UP;

    // Start is called before the first frame update
    void Start()
    {
        activeHitbox = hitboxes[0];
        rb = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        setAngleDirection();
        transform.rotation = Quaternion.identity; // keep rotation as initial

        if (health <= 0 && !isDying)
        {
            StartCoroutine("deathCoRoutine");
        }
        else if (isDying) {
            if (this.gameObject.GetComponent<SpriteRenderer>())
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            
            var player = GameObject.FindWithTag("Player");
            var diff_x = player.transform.position.x - this.gameObject.transform.position.x;
            var diff_y = player.transform.position.y - this.gameObject.transform.position.y;

            var dist = Vector2.Distance(player.transform.position, this.gameObject.transform.position);

            var attack_dist_reached = dist < 1.7f;

            var enemyMovement = attack_dist_reached ? new Vector2(0, 0) : new Vector2(diff_x, diff_y);

            rb.velocity = (20 * enemyMovement * Time.deltaTime);

            bool isEnemyMoving = enemyMovement.x != 0 || enemyMovement.y != 0;


            if (isEnemyMoving)
            {
                angle = Mathf.Atan2(enemyMovement.y, enemyMovement.x) * Mathf.Rad2Deg;
                angle += 180.0f;
            }

            if (attack_dist_reached && !enemyIsAttacking && !attackCooldown)
            {
                StartCoroutine(attack());

                if (hasProjectlie)
                {
                    Rigidbody2D clone = Instantiate(projectile, transform.position, transform.rotation);

                    Vector2 p_velocity = new Vector2(0, 0);
                    switch (currentAngleDirection)
                    {
                        case AngleDirection.UP:
                            p_velocity = new Vector2(0, 2);
                            break;
                        case AngleDirection.RIGHT_UP:
                            p_velocity = new Vector2(1.5f, 1.5f);
                            break;
                        case AngleDirection.RIGHT:
                            p_velocity = new Vector2(2, 0);
                            break;
                        case AngleDirection.RIGHT_DOWN:
                            p_velocity = new Vector2(1.5f, -1.5f);
                            break;
                        case AngleDirection.DOWN:
                            p_velocity = new Vector2(0, -2);
                            break;
                        case AngleDirection.LEFT_DOWN:
                            p_velocity = new Vector2(-1.5f, -1.5f);
                            break;
                        case AngleDirection.LEFT:
                            p_velocity = new Vector2(-2, 0);
                            break;
                        case AngleDirection.LEFT_UP:
                            p_velocity = new Vector2(-1.5f, 1.5f);
                            break;
                        default:
                            break;
                    }
                    clone.velocity = p_velocity;
                }
                else
                {
                    var itemsInActiveHitbox = activeHitbox.GetComponent<playerAttackHitbox>().inHitbox;
                    if (itemsInActiveHitbox.Count != 0)
                    {
                        foreach (var item in itemsInActiveHitbox)
                        {
                            if (item.GetComponent<player>())
                            {
                                item.GetComponent<player>().damage(10);
                            }
                        }
                    }
                }

            }

            //Debug.Log("Angle:" + angle);

            playAnim(isEnemyMoving, ((int)currentAngleDirection));
            setHitbox((int)currentAngleDirection);
        }


       
    }

    public void damage(int amount)
    {
        if (!isDying) {
            StartCoroutine(damageCoRoutine());
            health -= amount;
        }

    }

    IEnumerator damageCoRoutine()
    {
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        yield return new WaitForSeconds(0.05f);
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }

    }
    IEnumerator deathCoRoutine()
    {
        isDying = true;
        Instantiate(healthOrb, new Vector2(transform.position.x, transform.position.y - 1), transform.rotation);
        Instantiate(healthOrb, new Vector2(transform.position.x, transform.position.y + 1), transform.rotation);
        Instantiate(healthOrb, new Vector2(transform.position.x, transform.position.y), transform.rotation);
        Instantiate(soulOrb, new Vector2(transform.position.x - 1, transform.position.y), transform.rotation);
        Instantiate(soulOrb, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation); 
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);

    }


    void setAngleDirection()
    {
        if (angle >= 247.5f && angle <= 292.5f)
        {
            this.currentAngleDirection = AngleDirection.UP;
        }
        else if (angle < 247.5f && angle >= 202.5f)
        {
            this.currentAngleDirection = AngleDirection.RIGHT_UP;
        }
        else if (angle < 202.5f && angle >= 157.5f)
        {
            this.currentAngleDirection = AngleDirection.RIGHT;
        }
        else if (angle < 157.5f && angle >= 112.5f)
        {
            this.currentAngleDirection = AngleDirection.RIGHT_DOWN;
        }
        else if (angle < 112.5f && angle >= 67.5f)
        {
            this.currentAngleDirection = AngleDirection.DOWN;
        }
        else if (angle < 67.5f && angle >= 22.5f)
        {
            this.currentAngleDirection = AngleDirection.LEFT_DOWN;
        }

        else if ((angle < 22.5f) || (angle >= 337.5f))
        {
            this.currentAngleDirection = AngleDirection.LEFT;
        }

        else if (angle < 337.5f && angle >= 292.5f)
        {
            this.currentAngleDirection = AngleDirection.LEFT_UP;
        }
    }

    void playAnim(bool isMoving, int id)
    {
        if (enemyIsAttacking)
            a.Play(attacks[id].name);
        else if (isMoving)
            a.Play(anims[id].name);
        else
            a.Play(stills[id].name);
    }

    void setHitbox(int id)
    {
        activeHitbox = hitboxes[id];
        foreach (var hitbox in hitboxes)
        {
            hitbox.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
        //activeHitbox.gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 100);
    }

    IEnumerator attack()
    {
        enemyIsAttacking = true;
        attackCooldown = false;
        yield return new WaitForSeconds(hasProjectlie ? 1f : 0.1f);
        enemyIsAttacking = false;
        attackCooldown = true;
        yield return new WaitForSeconds(hasProjectlie ? 1.2f : 0.9f);
        attackCooldown = false;
    }
}
