using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageable : MonoBehaviour
{

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(this.gameObject);
    }

    public void damage(int amount) {
        StartCoroutine(damageCoRoutine());
        health -= amount;
    }

    IEnumerator damageCoRoutine()
    {
        if (this.gameObject.GetComponent<SpriteRenderer>()) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        yield return new WaitForSeconds(0.05f);
        if (this.gameObject.GetComponent<SpriteRenderer>())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }

    }
}
