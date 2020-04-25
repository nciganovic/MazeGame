using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{

    public Enemy enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallReduceDamage(float amount, string shotPlace)
    {
        enemy.ReduceHealth(amount, shotPlace);
    }

   
}
