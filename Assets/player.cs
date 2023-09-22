using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    CharacterController cc;
    Animator a;
    SpriteRenderer sr;
    public float angle = 0;

    public AnimationClip[] anims;
    public AnimationClip[] stills;
    public bool isAnim = true;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        a = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var PlayerMovement = new Vector2(Input.GetAxis("Horizontal") * 1, Input.GetAxis("Vertical") * 1);


        cc.Move(transform.rotation * PlayerMovement * Time.deltaTime);

        bool isPlayerMoving = PlayerMovement.x != 0 || PlayerMovement.y != 0;


        if (isPlayerMoving) {
            angle = Mathf.Atan2(PlayerMovement.y, PlayerMovement.x) * Mathf.Rad2Deg;
            angle += 180.0f;
        }

       //Debug.Log("Angle:" + angle);
        if (angle >= 247.5f && angle <= 292.5f)
            playAnim(isPlayerMoving, 0); // up
        else if (angle < 247.5f && angle >= 202.5f)
            playAnim(isPlayerMoving, 1); // right up
        else if (angle < 202.5f && angle >= 157.5f)
            playAnim(isPlayerMoving, 2); // right
        else if (angle < 157.5f && angle >= 112.5f)
            playAnim(isPlayerMoving, 3); // right down
        else if (angle < 112.5f && angle >= 67.5f)
            playAnim(isPlayerMoving, 4); // down

        else if (angle < 67.5f && angle >= 22.5f)
            playAnim(isPlayerMoving, 5); // left down
        else if ((angle < 22.5f && angle >= 0f) || (angle > 337.5f && angle < 360f))
            playAnim(isPlayerMoving, 6); // left
        else if (angle < 337.5f && angle >= 292.5f)
            playAnim(isPlayerMoving, 7); // left up


    }


    void playAnim(bool isMoving, int id) {
        if (isMoving)
            a.Play(anims[id].name);
        else
            a.Play(stills[id].name);
    }
}
