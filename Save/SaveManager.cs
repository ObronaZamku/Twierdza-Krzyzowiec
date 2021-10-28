using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager
{
    public static void SaveGame(string fileName){
        Save save = Save.createSave();  // Zapis gry
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(fileName, json);
    }
}
