/*
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//TODO : add bootstrap 
public class GlobalObjectManager : Singleton<GlobalObjectManager>
{
    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        //_lastRegistedID= PlayerPrefs.GetInt("GlobalID", 0);
    }

   // public int GetID() => _lastRegistedID++;
    public void RegisterObject(int id, GameObject obj)
    {
        _objects[id] = obj;
    }

    public void DeRegisterObject(int id)
    {
        _objects.Remove(id);
    }

    public GameObject GetObject(int id)
    {
        return _objects[id];
    }
    
}
*/

