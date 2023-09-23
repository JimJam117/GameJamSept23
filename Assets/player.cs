using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    Animator a;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public float angle = 0;

    public AnimationClip[] anims;
    public AnimationClip[] stills;
    public AnimationClip[] attacks;
    public GameObject[] hitboxes;
    public GameObject activeHitbox;

    public bool isAnim = true;
    public bool playerIsAttacking = false;

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

       //Debug.Log("Angle:" + angle);
        if (angle >= 247.5f && angle <= 292.5f)
        {
            playAnim(isPlayerMoving, 0); // up
            setHitbox(0);
    }
        else if (angle < 247.5f && angle >= 202.5f)
        {
            playAnim(isPlayerMoving, 1); // right up
            setHitbox(1);
    }
        else if (angle < 202.5f && angle >= 157.5f)
        {
            playAnim(isPlayerMoving, 2); // right
            setHitbox(2);
    }
        else if (angle < 157.5f && angle >= 112.5f)
        {
            playAnim(isPlayerMoving, 3); // right down
            setHitbox(3);
    }
        else if (angle < 112.5f && angle >= 67.5f)
        {
            playAnim(isPlayerMoving, 4); // down
            setHitbox(4);
    }
        else if (angle < 67.5f && angle >= 22.5f)
        {
            playAnim(isPlayerMoving, 5); // left down
            setHitbox(5);
    }

        else if ((angle < 22.5f) || (angle >= 337.5f))
        {
            playAnim(isPlayerMoving, 6); // left
            setHitbox(6);
    }

        else if (angle < 337.5f && angle >= 292.5f) {
            playAnim(isPlayerMoving, 7); // left up
            setHitbox(7);
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
        yield return new WaitForSeconds(0.1f);
        playerIsAttacking = false;
    }
}
