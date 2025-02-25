
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



public class ObjectPool<T> : Singleton<T> where T : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField]
    protected GameObject objectToSpawn;
    protected Queue<GameObject> objectPool;


    [Header(" Settings ")]
    [SerializeField]
    protected int poolSize;
    public bool InitAwake;

    
    protected override void Awake()
    {
        base.Awake();
        objectPool = new Queue<GameObject>();

    }
    private void Start()
    {
      
        if(InitAwake&& objectToSpawn!=null)
        {
            GameObject go;
            for (int i = objectPool.Count; i < poolSize; i++)
            {
                go = Instantiate(objectToSpawn);
                go.name = objectToSpawn.name + "_"+ objectPool.Count;
                go.SetActive(false);
                objectPool.Enqueue(go);
            }
        }
    }
    public virtual GameObject GetObject(GameObject prefab = null)
    {
        if (prefab == null)
        {
            prefab = objectToSpawn;
        }

        GameObject returnObj = null;
        if (objectPool.Count < poolSize)
        {
            returnObj = Instantiate(prefab) as GameObject;
            returnObj.name = prefab.name + "_" + objectPool.Count;
        }
        else
        {
            if (objectPool.Count <= 0)
                return null;
            returnObj = objectPool.Dequeue();

            returnObj.SetActive(true);
        }
        return returnObj;
    }
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}


