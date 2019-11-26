using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    /*
    #region AncienSystem
    //REFERENCe global pour tout le reste
    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> poolItems;
    public List<GameObject> pooledObjects;

    private void Awake()
    {
        SharedInstance = this;
    }
    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in poolItems)
        {
            //nouveau parent
            if (!item.poolParent)
            {
                GameObject obj = new GameObject(item.objectToPool.name + "_Pool");
                obj.transform.SetParent(transform);
                item.poolParent = obj;
            }

            for (int i = 0; i < item.AmountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool, item.poolParent.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in poolItems)
        {
            if (item.objectToPool.tag == tag)
            {
                Debug.LogError("need More" + item.objectToPool, transform);
                if (item.canExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, item.poolParent.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
    #endregion
    */

    //REFERENCe global pour tout le reste
    public static ObjectPooler poolingSystem;
    public List<ObjectPoolItem> poolItems;
    public List<PoolableObjects> pooledObjects;



    private void Awake()
    {
        poolingSystem = this;
    }
    private void GeneratePool()
    {
        pooledObjects = new List<PoolableObjects>();


        for (int j = 0; j < poolItems.Count; j++)
        {
            //nouveau parent
            if (!poolItems[j].poolParent)
            {
                GameObject obj = new GameObject(poolItems[j].objectToPool.name + "_Pool");
                obj.transform.SetParent(transform);
                ObjectPoolItem currentPoolItem = poolItems[j];
                poolItems.Remove(poolItems[j]);
                currentPoolItem.poolParent = obj;
                poolItems[j] = currentPoolItem;
            }

            for (int i = 0; i < poolItems[j].AmountToPool; i++)
            {

                GameObject obj = (GameObject)Instantiate(poolItems[j].objectToPool, poolItems[j].poolParent.transform);
                
                PoolableObjects po = obj.GetComponent<PoolableObjects>();
                po.Inpool();
                pooledObjects.Add(po);
            }
        }
    }
    public PoolableObjects GetPooledObject<T>()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].canBePool && pooledObjects[i] is T)
            {
                return pooledObjects[i];
            }
        }

        //permet de rajouter des élments dans liste si il n'y en a pas assez ( en test ) 
        foreach (ObjectPoolItem item in poolItems)
        {
            if (item.objectToPool.GetComponent<PoolableObjects>() is T)
            {
                Debug.LogError("need More" + item.objectToPool, transform);
                if (item.canExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, item.poolParent.transform);
                    PoolableObjects po = obj.GetComponent<PoolableObjects>();
                    po.Inpool();
                    pooledObjects.Add(po);
                    return po;
                }
            }
        }
        return null;
    }

    public PoolableObjects GetPooledObject(System.Type type)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].canBePool && pooledObjects[i].GetType() == type)
            {
                return pooledObjects[i];
            }
        }

        //permet de rajouter des élments dans liste si il n'y en a pas assez ( en test ) 
        foreach (ObjectPoolItem item in poolItems)
        {
            if (item.objectToPool.GetComponent<PoolableObjects>().GetType() == type)
            {
                Debug.LogError("need More" + item.objectToPool, transform);
                if (item.canExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, item.poolParent.transform);
                    PoolableObjects po = obj.GetComponent<PoolableObjects>();
                    po.Inpool();
                    pooledObjects.Add(po);
                    return po;
                }
            }
        }
        return null;
    }
}

[System.Serializable] //permet d'avoir des insatnce deditable dans l'inspector
public struct ObjectPoolItem
{
    public GameObject objectToPool;
    public int AmountToPool;
    public bool canExpand;
    public GameObject poolParent;

}

