using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{

    private string fileName = "Save.json";

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5)/* coś tam*/){
            SaveManager.SaveGame(fileName);
        }
        if(Input.GetKeyDown(KeyCode.F6)/* inne coś tam*/){
            LoadManager.LoadGame(fileName);
        }
    }
}
