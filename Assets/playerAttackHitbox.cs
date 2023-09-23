using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class playerAttackHitbox : MonoBehaviour
{

    public List<GameObject> inHitbox = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter2D(Collider2D other)
	{
        Debug.Log("TRIGGER ITEM");
        if (!inHitbox.Contains(other.gameObject)) {
            Debug.Log("ADDED ITEM");
            inHitbox.Add(other.gameObject);
        }

	}

	void OnTriggerExit2D(Collider2D other)
	{
        if (inHitbox.Contains(other.gameObject))
        {
            Debug.Log("ADDED ITEM");
            inHitbox.Remove(other.gameObject);
        }

	}
}
