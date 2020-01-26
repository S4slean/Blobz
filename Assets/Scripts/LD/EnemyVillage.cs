using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVillage : MonoBehaviour
{

    SphereCollider boundaries;

    public float boundariesRange = 10;
    public float detectionRange = 20;

    public int splouchOnDestruction = 0;
    public int maxSoldiers = 20;
    public float spawnRange = 2;
    public float spawnRate = 4;

    [Tooltip("(in ticks")] public int repairDelay = 40;
    private int count = 0;

    public List<Destructible> buildings;
    List<Blob> blobs = new List<Blob>();
    bool alerted = false;

    private void Start()
    {
        boundaries = GetComponent<SphereCollider>();
        boundaries.radius = boundariesRange;

        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].village = this;
        }

        TickManager.doTick += OnTick;
    }

    private void OnDisable()
    {
        TickManager.doTick -= OnTick;
    }

    public void OnTick()
    {
        if (blobs.Count > maxSoldiers)
            return;

        if(count > spawnRate-1)
        {
            SpawnBlob();
            count = 0;
        }
        else
        {
            count++;
        }
    }

    public void SpawnBlob()
    {
        int rand = Random.Range(0, buildings.Count);
        Blob blob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
        blob.Outpool();
        blob.ChangeType(BlobManager.BlobType.mad);
        Vector2 circle = Random.insideUnitCircle;
        Vector3 circleProjection = new Vector3(circle.x, 0, circle.y).normalized;
        blob.transform.position = buildings[rand].transform.position +  circleProjection* spawnRange + Vector3.up;
        blob.tag = "Enemies";
        blob.SetOrigin(this);
        blobs.Add(blob);

        //Play Spawn Anim
    }

    public void RemoveBlobFromVillageList(Blob blob)
    {
        blobs.Remove(blob);
    }

    public void StopVillageRepair()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i].isRuin && !buildings[i].isVillageNexus)
            {
                buildings[i].PauseRepair();
            }
        }
    }

    public void RestartVillageRepair()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i].isRuin && !buildings[i].isVillageNexus)
            {
                buildings[i].RestartRepair();
            }
        }
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

    public void DeleteVillage()
    {
        //Anim
        
        Kill(); //Retire MOI !!!
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    //Pense à mettre le bon layer de collision pour ne détecter que les cells du joueur
    private void OnTriggerEnter(Collider other)
    {
        if (alerted)
            return;

        AlertBlobs();
    }

}
