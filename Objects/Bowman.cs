using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : Savable, Shootable
{
    [Header("Range")]
    [SerializeField]
    private float range;

    [Header("Reload time")]
    [SerializeField]
    private float reloadTime;

    [Header("Ammunition")]
    [SerializeField]
    private GameObject missilePrefab;

    public float GetRange(){
        return range;
    }

    public float GetReload(){
        return reloadTime;
    }

    public GameObject GetAmmunition()
    {
        return missilePrefab;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
