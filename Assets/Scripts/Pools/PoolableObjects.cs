using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjects : MonoBehaviour
{
    public bool canBePool;


    public void Outpool()
    {
        canBePool = false;
        gameObject.SetActive(true);
    }

    public virtual void Inpool()
    {
        //Vector3 pos = transform.position;
        //pos =Vector3.Lerp(transform.position , ObjectPooler.poolingSystem.transform.position , 0.01f);
        //transform.position = pos;
        //transform.position = ObjectPooler.poolingSystem.transform.position;
        canBePool = true;
        gameObject.SetActive(false);
        
    }
    protected IEnumerator DesactiveGameObject (float delay)
    {
        //A changé mais c'est pour test
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
       
    }
}
