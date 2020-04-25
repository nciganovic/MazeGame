using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public GameObject monster; 

    // Start is called before the first frame update
    void Start()
    {
        monster.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider hit) {

        if (hit.gameObject.tag == "Player") {
            Debug.Log("Trigger123");
            monster.SetActive(true);
        }

    }
}
