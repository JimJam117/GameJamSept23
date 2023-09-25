using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    Animator a;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public float angle = 0;

    public int health = 100;
    public int soulJuice = 0;
    public int soulJuiceGoal = 100;

    public int currentEnemyCount = 0;
    public int desiredEnemyCount = 2;

    public int spawn_altar_index = 0;
    public GameObject[] spawn_altars;
    public bool spawnCooldown = false;

    public Rigidbody2D projectile;
    public bool hasProjectlie = false;

    public AnimationClip[] anims;
    public AnimationClip[] stills;
    public AnimationClip[] attacks;
    public GameObject[] hitboxes;
    public GameObject activeHitbox;

    public bool isAnim = true;
    public bool playerIsAttacking = false;
    public bool isAscending = false;

    public Sprite redTwist, stdTwist, none_sprite, upgrading_transition, you_died, return_to_main_menu_btn, next_level_btn;


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

    public void returnToMainMenu() {
        Destroy(this.gameObject);
    }

    public void nextLevel()
    {
        Destroy(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {


        if (soulJuice >= soulJuiceGoal && health > 0)
        {
            GameObject.FindWithTag("threshold_msg").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }

        else if (soulJuice >= soulJuiceGoal && health <= 0 && !isAscending)
        {
            isAscending = true;
            StartCoroutine(ascend());
        }

        else if (soulJuice < soulJuiceGoal && health <= 0) {
            StartCoroutine(death());
        }


        var twist = GameObject.FindWithTag("twist").transform;
        twist.rotation = Quaternion.Euler(twist.eulerAngles.x, twist.eulerAngles.y, twist.eulerAngles.z + 1);



        currentEnemyCount = GameObject.FindGameObjectsWithTag("enemy").Length;
        spawn_altars = GameObject.FindGameObjectsWithTag("spawn_altar");

        if (currentEnemyCount < desiredEnemyCount) {
            spawn_altar_index++;
            if (spawn_altar_index >= spawn_altars.Length) {
                spawn_altar_index = 0;
            }
            //Debug.LogError(spawn_altar_index);
            spawn_altars[spawn_altar_index].GetComponent<spawn_altar>().spawn();
        }

        GameObject.FindWithTag("healthUI").GetComponent<Text>().text = health.ToString();
        GameObject.FindWithTag("soulJuiceUI").GetComponent<Text>().text = soulJuice.ToString();
        GameObject.FindWithTag("soulJuiceNeededUI").GetComponent<Text>().text = soulJuiceGoal.ToString();

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
                        if (item != null && item.GetComponent<enemy>()) {
                            item.GetComponent<enemy>().damage(10);
                        }
                    }
                }
            }

        }

       //Debug.Log("Angle:" + angle);
    
            playAnim(isPlayerMoving, ((int)currentAngleDirection)); 
            setHitbox((int)currentAngleDirection);
    }

    public void damage(int amount)
    {
        StartCoroutine(damageCoRoutine());
        health -= amount;
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

    public void addHealth(int amount) {
        StartCoroutine(addHealthCoRoutine());
        health += amount;
    }

    IEnumerator addHealthCoRoutine()
    {
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        }
        yield return new WaitForSeconds(0.05f);
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }

    }
    public void addSoulJuice(int amount)
    {
        StartCoroutine(addSoulJuiceCoRoutine());
        soulJuice += amount;
    }

    IEnumerator addSoulJuiceCoRoutine()
    {
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        }
        yield return new WaitForSeconds(0.05f);
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }

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
        //activeHitbox.gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 100);
    }

    IEnumerator attack() {
        playerIsAttacking = true;
        yield return new WaitForSeconds(hasProjectlie ? 1f : 0.1f);
        playerIsAttacking = false;
    }


    IEnumerator ascend()
    {
        GameObject.FindWithTag("twist").GetComponent<Image>().sprite = stdTwist;
        GameObject.FindWithTag("upgrading_transition").GetComponent<Image>().sprite = upgrading_transition;
        GameObject.FindWithTag("you_died").GetComponent<Image>().sprite = none_sprite;
        GameObject.FindWithTag("reincarnate_button").GetComponent<Image>().sprite = next_level_btn;
        GameObject.FindWithTag("return_to_main_menu_button").GetComponent<Image>().sprite = none_sprite;

        Destroy(GameObject.FindWithTag("return_to_main_menu_button"));
        GameObject.FindWithTag("reincarnate_button").GetComponent<Button>().interactable = true;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator death()
    {
        GameObject.FindWithTag("twist").GetComponent<Image>().sprite = redTwist;
        GameObject.FindWithTag("upgrading_transition").GetComponent<Image>().sprite = none_sprite;
        GameObject.FindWithTag("you_died").GetComponent<Image>().sprite = you_died;

        GameObject.FindWithTag("reincarnate_button").GetComponent<Image>().sprite = none_sprite;
        GameObject.FindWithTag("return_to_main_menu_button").GetComponent<Image>().sprite = return_to_main_menu_btn;

        Destroy(GameObject.FindWithTag("reincarnate_button"));
        GameObject.FindWithTag("return_to_main_menu_button").GetComponent<Button>().interactable = true;
        yield return new WaitForSeconds(1f);
    }


}
