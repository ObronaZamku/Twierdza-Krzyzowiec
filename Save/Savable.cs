using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Savable
{
    
}

public interface SavableWithHealth : Savable
{
    public float Health {
        get;
        set;
    }
}
