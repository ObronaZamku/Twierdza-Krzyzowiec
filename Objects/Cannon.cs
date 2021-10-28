using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Savable, Shootable
{

    public void Shoot(Vector3 targetPosition){
        Debug.Log("SHOOTING CANNON");
    }

    
    public float GetRange(){
        return 100f;
    }

    public float GetReload(){
        return 2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
