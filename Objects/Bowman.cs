using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : Savable, Shootable
{

    public void Shoot(Vector3 targetPosition){
        Debug.Log("SHOOTING");
    }

    public float GetRange(){
        return 100f;
    }

    public float GetReload(){
        return 2f;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
