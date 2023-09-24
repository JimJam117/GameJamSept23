using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour
{
    public Sprite[] expolode_sprites;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public bool hasTimedOut = false;
    public List<GameObject> inHitbox = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(timeout());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!inHitbox.Contains(other.gameObject))
        {
            
            inHitbox.Add(other.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (inHitbox.Contains(other.gameObject))
        {
            
            inHitbox.Remove(other.gameObject);
        }

    }

    IEnumerator timeout()
    {
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector2(0, 0);

        foreach (var item in inHitbox)
        {
            if (item.GetComponent<enemy>()) {
                item.GetComponent<enemy>().damage(25);
            } 
        }

        sr.sprite = expolode_sprites[0];
        yield return new WaitForSeconds(0.2f);
        sr.sprite = expolode_sprites[1];
        yield return new WaitForSeconds(0.2f);
        sr.sprite = expolode_sprites[2];
        yield return new WaitForSeconds(0.2f);
        sr.sprite = expolode_sprites[3];
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
