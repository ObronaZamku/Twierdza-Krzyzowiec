using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SuppliesManager : MonoBehaviour
{
    [Header("Supplies")]
    [SerializeField]
    private Supplies supplies;

    [Header("Costs")]
    [SerializeField]
    private Costs wallCosts;
    [SerializeField]
    private Costs repairCosts;
    [SerializeField]
    private Costs bowmanCosts;
    [SerializeField]
    private Costs barrelCosts;
    [SerializeField]
    private Costs cannonCosts;

    [SerializeField]
    private Costs upgradeCosts;

    [Header("Images")]
    [SerializeField]
    private RawImage goldImage;

    [SerializeField]
    private RawImage woodImage;

    [SerializeField]
    private RawImage stoneImage;

    [Header("Simulation")]
    [SerializeField]
    private float simulateRatio;

    private Text goldText;
    private Text woodText;

    private Text stoneText;

    // Start is called before the first frame update
    void Start()
    {
        supplies.Reset();
        goldText = goldImage.GetComponentInChildren<Text>();
        woodText = woodImage.GetComponentInChildren<Text>();
        stoneText = stoneImage.GetComponentInChildren<Text>();
        upgradeCosts.gold = 2600;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = ((int)supplies.gold).ToString();
        woodText.text = ((int)supplies.wood).ToString();
        stoneText.text = ((int)supplies.stone).ToString();
    }

    void FixedUpdate()
    {
        SimulateSupplies();
    }

    void SimulateSupplies()
    {
        supplies.gold = supplies.gold + (simulateRatio * Time.fixedDeltaTime);
        supplies.stone = supplies.stone + (simulateRatio * Time.fixedDeltaTime);
        supplies.wood = supplies.wood + (simulateRatio * Time.fixedDeltaTime);
    }

    public void ConstructionRepaired(int cost)
    {
        if (cost < supplies.wood && cost < supplies.stone)
        {
            supplies.wood = supplies.wood - cost / 8;
            supplies.stone = supplies.stone - cost / 8;
        }
    }

    public void WallBuilt(float distance)
    {
        supplies.gold = supplies.gold - (distance * wallCosts.gold);
        supplies.stone = supplies.stone - (distance * wallCosts.stone);
        supplies.wood = supplies.wood - (distance * wallCosts.wood);
    }

    public void PerformTransaction(string tag)
    {
        switch (tag)
        {
            case "Barrel":
                supplies.gold = supplies.gold - barrelCosts.gold;
                supplies.stone = supplies.stone - barrelCosts.stone;
                supplies.wood = supplies.wood - barrelCosts.wood;
                break;
            case "Bowman":
                supplies.gold = supplies.gold - bowmanCosts.gold;
                supplies.stone = supplies.stone - bowmanCosts.stone;
                supplies.wood = supplies.wood - bowmanCosts.wood;
                break;
            case "Cannon":
                supplies.gold = supplies.gold - cannonCosts.gold;
                supplies.stone = supplies.stone - cannonCosts.stone;
                supplies.wood = supplies.wood - cannonCosts.wood;
                break;
            case "Upgrade":
                supplies.gold = supplies.gold - upgradeCosts.gold;
                supplies.stone = supplies.stone - upgradeCosts.stone;
                supplies.wood = supplies.wood - upgradeCosts.wood;
                break;
            default:
                break;
        }
    }

    public void GetReward(AIStats stats)
    {
        supplies.gold = supplies.gold + stats.gold;
        supplies.stone = supplies.stone + stats.stone;
        supplies.wood = supplies.wood + stats.wood;
    }

    public void PhaseReward(int phaseCount)
    {
        supplies.gold = supplies.gold + 600 * phaseCount;
        supplies.stone = supplies.stone + 600 * phaseCount;
        supplies.wood = supplies.wood + 600 * phaseCount;
    }

    public bool EnoughForWall(float distance)
    {
        return ((distance * wallCosts.stone < supplies.stone) && (distance * wallCosts.wood < supplies.wood));
    }

    public bool CanBuild(string tag)
    {
        switch (tag)
        {
            case "Wall":
                return supplies.IsEnough(wallCosts);
            case "Barrel":
                return supplies.IsEnough(barrelCosts);
            case "Bowman":
                return supplies.IsEnough(bowmanCosts);
            case "Cannon":
                return supplies.IsEnough(cannonCosts);
            case "Upgrade":
                return supplies.IsEnough(upgradeCosts);
            default:
                return false;
        }
    }

}
