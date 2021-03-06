using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ButtonManager : MonoBehaviour
{


    public UnityEvent upgradeEvent;

    [Header("Buttons")]
    [SerializeField]
    private Button repairButton;

    [SerializeField]
    private Button wallButton;
    [SerializeField]
    private Button cannonButton;
    [SerializeField]
    private Button bowmanButton;
    [SerializeField]
    private Button barrelButton;

    [SerializeField]
    private Button endBuildingButton;

    [SerializeField]
    private Button starButton;

    [HideInInspector]
    public static Modes currentMode;

    [SerializeField]
    private Texture2D cursorTexture;

    private SuppliesManager suppliesManager;

    private GamePhases phases;

    private bool maxUpgradeState = false;

    void Start()
    {
        repairButton.onClick.AddListener(OnClickRepair);
        wallButton.onClick.AddListener(OnClickWall);
        cannonButton.onClick.AddListener(OnClickCannon);
        bowmanButton.onClick.AddListener(OnClickBowman);
        barrelButton.onClick.AddListener(OnClickBarrel);
        endBuildingButton.onClick.AddListener(OnClickEndBuilding);
        starButton.onClick.AddListener(OnClickStar);
        suppliesManager = GetComponent<SuppliesManager>();
        phases = GetComponent<GamePhases>();
    }

    void Update()
    {
        CheckButtons();
        if (currentMode != Modes.REPAIR && phases.isBuildingPhase)
        {
            ResetCursor();
        }
    }
    public void OnClickRepair()
    {
        currentMode = Modes.REPAIR;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void OnClickWall()
    {
        currentMode = Modes.WALL;
    }

    void OnClickCannon()
    {
        currentMode = Modes.CANNON;
    }

    void OnClickBowman()
    {
        currentMode = Modes.BOWMAN;
    }

    void OnClickBarrel()
    {
        currentMode = Modes.BARREL;
    }

    void OnClickEndBuilding()
    {
        currentMode = Modes.SIEGE;
        ButtonTurnOff();
    }

    void OnClickStar()
    {
        upgradeEvent.Invoke();
    }

    public void ButtonTurnOff()
    {
        if (currentMode == Modes.SIEGE)
        {
            endBuildingButton.interactable = false;
            wallButton.interactable = false;
            repairButton.interactable = false;
            cannonButton.interactable = false;
            bowmanButton.interactable = false;
            barrelButton.interactable = false;
            starButton.interactable = false;
        }
    }

    public void ButtonTurnOn()
    {
        endBuildingButton.interactable = true;
        wallButton.interactable = true;
        repairButton.interactable = true;
        cannonButton.interactable = true;
        bowmanButton.interactable = true;
        barrelButton.interactable = true;
        starButton.interactable = true;
    }

    private void CheckButtons()
    {
        if (currentMode != Modes.SIEGE)
        {
            if (!suppliesManager.CanBuild("Wall"))
            {
                wallButton.interactable = false;
            }
            else
            {
                wallButton.interactable = true;
            }
            if (!suppliesManager.CanBuild("Barrel"))
            {
                barrelButton.interactable = false;
            }
            else
            {
                barrelButton.interactable = true;
            }
            if (!suppliesManager.CanBuild("Bowman"))
            {
                bowmanButton.interactable = false;
            }
            else
            {
                bowmanButton.interactable = true;
            }
            if (!suppliesManager.CanBuild("Cannon"))
            {
                cannonButton.interactable = false;
            }
            else
            {
                cannonButton.interactable = true;
            }
            if (!suppliesManager.CanBuild("Upgrade") || maxUpgradeState)
            {
                starButton.interactable = false;
            }
            else
            {
                starButton.interactable = true;
            }
        }
    }

    public void DisableStarButton()
    {
        starButton.interactable = false;
        maxUpgradeState = true;
    }

    public static void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}

public enum Modes
{
    REPAIR, WALL, CANNON, BOWMAN, BARREL, SIEGE, DEFAULT
}
