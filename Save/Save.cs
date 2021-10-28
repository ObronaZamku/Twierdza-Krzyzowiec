using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class Save
{
    public List<SaveObjectWithChildren> saveObjects;

    public static Save createSave()
    {
        Save save = new Save();
        save.saveObjects = new List<SaveObjectWithChildren>();

        // All MonoBehaviours without parent or with parent not implementing SavableWithChildren
        List<Savable> li = new List<MonoBehaviour>(GameObject.FindObjectsOfType<MonoBehaviour>())
            .Where(obj => obj.transform.parent == null || obj.transform.parent.GetComponent<Savable>() == null)
            .OfType<Savable>()
            .ToList();
        
        foreach (Savable s in li)
            save.saveObjects.Add(toParentObject(s));

        return save; 
    }

    private static Save.SaveObjectWithChildren toParentObject(Savable savable)
    {
        Save.SaveObjectWithChildren saveObject = new SaveObjectWithChildren();
        MonoBehaviour mb = savable as MonoBehaviour;
        
        if(mb != null)
        {
            List<Savable> children1 = Save.SaveObjectWithChildren.GetSavableChildren(mb.transform);
            children1.ForEach(s => Debug.Log(s));
            List<Save.SaveObject> children = children1.ConvertAll(new Converter<Savable, Save.SaveObject>(toSaveObject));
            saveObject.children = children;
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
            saveObject.health = swh.health;
        }
        return saveObject;
    }

    private static Save.SaveObject toSaveObject(Savable savable)
    {
        Debug.Log(savable);
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
            saveObject.health = swh.health;
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
    }
    
    [System.Serializable]
    public class SaveObjectWithChildren : SaveObject {
        public List<SaveObject> children = new List<SaveObject>();

        public static List<Savable> GetSavableChildren(Transform transform){
            List<Savable> children = new List<Savable>();
            foreach(Transform child in transform){
                Debug.Log(child.gameObject.GetComponent<Savable>());
                children.Add(child.gameObject.GetComponent<Savable>());
            }
            children.ForEach(s => Debug.Log(s));
            return children.OfType<Savable>().ToList();
        }
    }
}