using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVillage : MonoBehaviour
{


    public float boundariesRange = 10;
    public float detectionRange = 20;
    public int maxSoldiers = 20;
    public int soldierCount = 0;
    public float spawnRange = 2;
    public int nbrOfBuildings = 6;

    public Destructible[] buildings;
    List<Blob> blobs = new List<Blob>();
    bool alerted = false;

    public void SpawnBlob()
    {
        int rand = Random.Range(0, nbrOfBuildings);
        Blob blob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
        blob.Outpool();
        Vector2 circle = Random.insideUnitCircle;
        Vector3 circleProjection = new Vector3(circle.x, 0, circle.y).normalized;
        blob.transform.position = buildings[rand].transform.position +  circleProjection* spawnRange ;
        blob.SetOrigin(this);

        //Play Spawn Anim
    }

    public void AlertBlobs()
    {
        for (int i = 0; i < blobs.Count; i++)
        {
            boundariesRange = detectionRange;
            //Play Angry Anim
        }
    }

    public void EnrageBlobs()
    {
        for (int i = 0; i < blobs.Count; i++)
        {
            blobs[i].knowsNexus = true;
        }
    }

    //Pense à mettre le bon layer de collision pour ne détecter que les cells du joueur
    private void OnTriggerEnter(Collider other)
    {
        if (alerted)
            return;

        AlertBlobs();
    }

}
