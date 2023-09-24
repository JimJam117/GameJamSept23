using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_orb : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<player>())
        {

            other.GetComponent<player>().addHealth(10);
            Destroy(this.gameObject);
        }

    }
}
