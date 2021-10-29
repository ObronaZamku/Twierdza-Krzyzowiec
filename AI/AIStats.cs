using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "AI", menuName = "AI")]
public class AIStats : ScriptableObject
{
    public int health;
    public int attack;

    public float gold;
    public float stone;
    public float wood;
}
