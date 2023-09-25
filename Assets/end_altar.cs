using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_altar : MonoBehaviour
{

    public AnimationClip glow;
    public Animator a;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<player>().soulJuice >= player.GetComponent<player>().soulJuiceGoal) {
            a.Play(glow.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("TRIGGER ITEM");
        if (player.GetComponent<player>().soulJuice >= player.GetComponent<player>().soulJuiceGoal && (other.gameObject == player))
        {
            player.GetComponent<player>().damage(300);
        }

    }
}
