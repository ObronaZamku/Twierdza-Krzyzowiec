using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Supplies", menuName = "Supplies")]
public class Supplies : ScriptableObject
{
    public float gold = 500;
    public float wood = 500;
    public float stone = 500;

    public void Reset()
    {
        gold = 500;
        wood = 500;
        stone = 500;
    }

    public bool IsEnough(Costs costs)
    {
        return gold > costs.gold && wood > costs.wood && stone > costs.stone;
    }
}

