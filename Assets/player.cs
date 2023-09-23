using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
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

    public bool isAnim = true;
    public bool playerIsAttacking = false;


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
        var PlayerMovement = new Vector2(Input.GetAxis("Horizontal") * 1, Input.GetAxis("Vertical") * 1);

        transform.rotation = Quaternion.identity; // keep rotation as initial

        rb.velocity = (100 * PlayerMovement * Time.deltaTime);

        bool isPlayerMoving = PlayerMovement.x != 0 || PlayerMovement.y != 0;


        if (isPlayerMoving) {
            angle = Mathf.Atan2(PlayerMovement.y, PlayerMovement.x) * Mathf.Rad2Deg;
            angle += 180.0f;
        }

        if (Input.GetButtonDown("Fire1") && !playerIsAttacking)
        {
            StartCoroutine(attack());

            if (hasProjectlie) {
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
            else {
                var itemsInActiveHitbox = activeHitbox.GetComponent<playerAttackHitbox>().inHitbox;
                if (itemsInActiveHitbox.Count != 0) {
                    foreach (var item in itemsInActiveHitbox)
                    {
                        if (item.GetComponent<damageable>()) {
                            item.GetComponent<damageable>().damage(10);
                        }
                    }
                }
            }

        }

       //Debug.Log("Angle:" + angle);
    
            playAnim(isPlayerMoving, ((int)currentAngleDirection)); 
            setHitbox((int)currentAngleDirection);
    }

    void setAngleDirection() {
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

    void playAnim(bool isMoving, int id) {
        if (playerIsAttacking)
            a.Play(attacks[id].name);
        else if (isMoving)
            a.Play(anims[id].name);
        else
            a.Play(stills[id].name);
    }

    void setHitbox(int id) {
        activeHitbox = hitboxes[id];
        foreach (var hitbox in hitboxes)
        {
            hitbox.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
        activeHitbox.gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 100);
    }

    IEnumerator attack() {
        playerIsAttacking = true;
        yield return new WaitForSeconds(hasProjectlie ? 1f : 0.1f);
        playerIsAttacking = false;
    }
}
