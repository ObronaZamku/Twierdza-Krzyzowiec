using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class Save
{
    public List<SaveObject> saveObjects;

    public static Save createSave()
    {
        Save save = new Save();
        save.saveObjects = new List<SaveObject>();

        List<Savable> li = new List<Savable>(GameObject.FindObjectsOfType<MonoBehaviour>().OfType<Savable>());
        foreach (Savable s in li)
            save.saveObjects.Add(toSaveObject(s));

        return save;
    }

    private static Save.SaveObject toSaveObject(Savable savable)
    {
        Save.SaveObject saveObject = new SaveObject();
        MonoBehaviour mb = savable as MonoBehaviour;
        if(mb != null)
        {
            saveObject.position = mb.transform.position;
            saveObject.scale = mb.transform.localScale;
            saveObject.rotation = mb.transform.rotation;
            GameObject go = mb.gameObject;
            string name = go.name;
            if (name.Substring(Math.Max(0, name.Length - 7)) == "(Clone)")
                saveObject.type = name.Substring(0, name.Length - 7);
            else
                saveObject.type = name;
        }
        SavableWithHealth swh = savable as SavableWithHealth;
        if(swh != null)
        {
            saveObject.health = swh.Health;
        }
        return saveObject;
    }

    [System.Serializable]
    public class SaveObject
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public string type;
        public float health;

        public SaveObject() { }

        public SaveObject(Vector3 position, Vector3 scale, Quaternion rotation, string type, float health)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.type = type;
            this.health = health;
        }
    }
}