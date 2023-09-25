using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(effectCoRoutine());
    }


    IEnumerator effectCoRoutine()
    {

        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
