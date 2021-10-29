using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Supplies", menuName = "Supplies")]
public class Supplies : ScriptableObject
{
    public float gold;
    public float wood;
    public float stone;

    public void Reset()
    {
        gold = 1000;
        wood = 1000;
        stone = 1000;
    }

    public bool IsEnough(Costs costs)
    {
        return gold > costs.gold && wood > costs.wood && stone > costs.stone;
    }
}

