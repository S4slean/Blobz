using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int remainingLife;
    public int maxLife = 5;
    public bool spawnEnemiesOnDestruction = false;
    public int nbrOfEnemiesOnDestruction = 5;
    public float spawnRange;
    public GameObject goodGraph;
    public GameObject brokenGraph;


    private void Start()
    {
        remainingLife = maxLife;
    }

    public void ReceiveDamage(int dmg)
    {
        remainingLife -= dmg;
        if(remainingLife < 1)
        {
            Destruction();
        }
    }

    public void Repair()
    {
        remainingLife = maxLife;
        //remettre le bon graph
    }

    public void Destruction()
    {

        //Play destruction Fx
        //PlayDestruction Sound
        SwapGraph();
        if (spawnEnemiesOnDestruction)
        {
            for (int i = 0; i < nbrOfEnemiesOnDestruction; i++)
            {
                Blob blob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
                blob.Outpool();
                Vector2 circle = Random.insideUnitCircle;
                Vector3 circleProjection = new Vector3(circle.x, 0, circle.y).normalized;
                blob.transform.position = transform.position + circleProjection * spawnRange;

            }
        }

        //Play Destruction Anim
    }

    public void SwapGraph()
    {
        goodGraph.SetActive(!goodGraph.activeSelf);
        brokenGraph.SetActive(!brokenGraph.activeSelf);
    }
}
