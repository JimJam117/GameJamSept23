using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soul_orb : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<player>())
        {

            other.GetComponent<player>().addSoulJuice(10);
            Destroy(this.gameObject);
        }

    }
}
