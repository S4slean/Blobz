using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]
public class PoolCustomInpector : Editor
{
    static ObjectPooler objPooler;


    private void OnEnable()
    {
        objPooler = target as ObjectPooler;
    }
    public override void OnInspectorGUI()
    {
        if (objPooler.poolItems.Count > 0)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Initialise Pools"))
            {
                GeneratePools();
            }
            if (GUILayout.Button("Clear All Pools"))
            {
                DeleteAllPools();
            }
            GUILayout.EndHorizontal();

        }
        else
        {
            EditorGUILayout.HelpBox("You have to create object's Pool", MessageType.Info);
        }

        base.OnInspectorGUI();
    }
    public static void GeneratePools()
    {
        if(objPooler == null)
        {
            objPooler = FindObjectOfType<ObjectPooler>();
        }
        Undo.RecordObject(objPooler, "CreateList");
        objPooler.pooledObjects = new List<PoolableObjects>();
        #region Ancien

        //foreach (ObjectPoolItem item in objPooler.poolItems)
        //{
        //    //nouveau parent
        //    if (!item.poolParent)
        //    {
        //        GameObject obj = new GameObject(item.objectToPool.name + "_Pool");
        //        obj.transform.SetParent(objPooler.transform);
        //        item.poolParent = obj;
        //    }
        #endregion
        for (int j = 0; j < objPooler.poolItems.Count; j++)
        {
            //nouveau parent
            if (!objPooler.poolItems[j].poolParent)
            {
                //nouveau parent
                GameObject obj = new GameObject(objPooler.poolItems[j].objectToPool.name + "_Pool");
                Undo.RegisterCreatedObjectUndo(obj, "CreatedObj");
                obj.transform.SetParent(objPooler.transform);
                ObjectPoolItem currentPoolItem = objPooler.poolItems[j];
                //objPooler.poolItems.Remove(objPooler.poolItems[j]);
                currentPoolItem.poolParent = obj;
                //objPooler.poolItems[j] = currentPoolItem;
                objPooler.poolItems[j] = currentPoolItem;
            }
            #region MyRegion

            #endregion
            //for (int i = 0; i < item.AmountToPool; i++)
            //{
            //    //GameObject obj = (GameObject)Instantiate(item.objectToPool, item.poolParent.transform);
            //    GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(item.objectToPool , item.poolParent.transform);
            //    PoolableObjects po = obj.GetComponent<PoolableObjects>();
            //    po.Inpool();
            //    objPooler.pooledObjects.Add(po);
            //}
            for (int i = 0; i < objPooler.poolItems[j].AmountToPool; i++)
            {
                //GameObject obj = (GameObject)Instantiate(item.objectToPool, item.poolParent.transform);
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(objPooler.poolItems[j].objectToPool, objPooler.poolItems[j].poolParent.transform);
                if (obj == null)
                {
                    
                    return;
                }

                PoolableObjects po = obj.GetComponent<PoolableObjects>();
                po.Inpool();
                objPooler.pooledObjects.Add(po);
            }
        }
    }
    private void DeleteAllPools()
    {
        for (int i = 0; i < objPooler.poolItems.Count; i++)
        {
            Undo.DestroyObjectImmediate(objPooler.poolItems[i].poolParent.gameObject);

        }
        Undo.RecordObject(objPooler, "SupressList");
        objPooler.pooledObjects.Clear();
        objPooler.poolItems.Clear();
    }
}
