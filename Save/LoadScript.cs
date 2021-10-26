using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            string json = File.ReadAllText("Save.txt"); // Wczytanie gry
            Save save = JsonUtility.FromJson<Save>(json);
            instantiateObjects(save);
        }
    }

    void instantiateObjects(Save save)
    {
        foreach(Save.SaveObject s in save.saveObjects)
        {
            GameObject go = (GameObject)Instantiate(Resources.Load(s.type), s.position, s.rotation);
            go.transform.localScale = s.scale;
            SavableWithHealth swh = go.GetComponent<SavableWithHealth>();
            if (swh != null)
            {
                swh.Health = s.health;
            }
        }
    }
}
