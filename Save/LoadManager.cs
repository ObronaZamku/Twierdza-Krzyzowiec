using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadManager
{
    public static void LoadGame(string fileName)
    {
        string json = File.ReadAllText(fileName); // Wczytanie gry
        Save save = JsonUtility.FromJson<Save>(json);
        instantiateObjects(save.saveObjects);
    }

    private static void instantiateObjects(List<Save.SaveObjectWithChildren> saveObjects)
    {
        foreach (Save.SaveObjectWithChildren s in saveObjects)
        {
            Object prefab = Resources.Load("prefabs/" + s.type);
            if (prefab != null)
            {
                GameObject go = (GameObject)GameObject.Instantiate(prefab, s.position, s.rotation);
                go.transform.localScale = s.scale;
                SavableWithHealth swh = go.GetComponent<SavableWithHealth>();
                if (swh != null)
                {
                    swh.health = s.health;
                    swh.maxHealth = s.maxHealth;
                }
                instantiateObjects(s.children, go.transform);
            }
        }
    }

    private static void instantiateObjects(List<Save.SaveObject> saveObjects, Transform parent)
    {
        foreach (Save.SaveObject s in saveObjects)
        {
            Object prefab = Resources.Load("prefabs/" + s.type);
            if (prefab != null)
            {
                GameObject go = (GameObject)GameObject.Instantiate(prefab, s.position, s.rotation);
                SavableWithHealth swh = go.GetComponent<SavableWithHealth>();
                if (swh != null)
                {
                    swh.health = s.health;
                    swh.maxHealth = s.maxHealth;
                }
                go.transform.SetParent(parent, true);
                go.transform.localScale = s.scale;
            }
        }
    }
}