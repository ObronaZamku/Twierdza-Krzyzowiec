using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save save = Save.createSave();  // Zapis gry
            string json = JsonUtility.ToJson(save);
            File.WriteAllText("Save.txt", json);
        }
    }
}
