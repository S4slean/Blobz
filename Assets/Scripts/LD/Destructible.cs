using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : PoolableObjects, PlayerAction
{
    [Header("Refs")]
    public GameObject goodGraph;
    public GameObject brokenGraph;

    [Header("General")]
    private int remainingLife;
    public int maxLife = 5;
    private int count = 0;
    public bool isReapairable = false;
    [HideInInspector] public bool isRuin = false;

    [Header("Enemies")]
    public bool isVillageNexus = false;
    [HideInInspector] public EnemyVillage village;
    public bool spawnEnemiesOnDestruction = false;
    public int nbrOfEnemiesOnDestruction = 5;
    public float spawnRange;

    [Header("Clickable")]
    public bool canBeDestroyedByClick = false;
    public int splouchAtDestruction = 50;
    public int splouchAtClick = 5;



    private void Start()
    {
        remainingLife = maxLife;
    }

    public void ReceiveDamage(int dmg)
    {
        remainingLife -= dmg;
        if (remainingLife < 1)
        {
            Destruction();
        }
    }

    public void Repair()
    {
        TickManager.doTick -= RepairTick;
        remainingLife = maxLife;
        isRuin = false;

        if (isVillageNexus)
        {
            village.RestartVillageRepair();
            UIManager.Instance.HideVillageSelection();
        }
        //remettre le bon graph
    }

    public void PauseRepair()
    {
        TickManager.doTick -= RepairTick;
    }

    public void RestartRepair()
    {
        TickManager.doTick += RepairTick;
    }

    public void Destruction()
    {
        if (!isReapairable)
        {
            if (spawnEnemiesOnDestruction)
                SpawnEnemies();

            //Insert Anim and put Delete at the end
            Destroy(gameObject); //remove once anim is inserted
        }
        else
        {

            if (isRuin)
                return;

            isRuin = true;
            //Play destruction Fx
            //PlayDestruction Sound
            SwapGraph();
            if (spawnEnemiesOnDestruction)
            {
                SpawnEnemies();
            }

            if (isVillageNexus)
            {
                village.StopVillageRepair();
                UIManager.Instance.DisplayVillageSelection(village);
            }

            TickManager.doTick += RepairTick;
            //Play Destruction Anim
        }
    }

    public void SpawnEnemies()
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

    public void RepairTick()
    {
        if (count > village.repairDelay)
        {
            Repair();
            count = 0;
        }
        else
        {
            count++;
        }
    }


    public void SwapGraph()
    {
        goodGraph.SetActive(!goodGraph.activeSelf);
        brokenGraph.SetActive(!brokenGraph.activeSelf);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        TickManager.doTick -= RepairTick;
    }








    public void OnLeftClickDown(RaycastHit hit)
    {

    }

    public void OnShortLeftClickUp(RaycastHit hit)
    {
        ReceiveDamage(1);
        Debug.Log("Aie");
    }

    public void OnLeftClickHolding(RaycastHit hit)
    {

    }

    public void OnLongLeftClickUp(RaycastHit hit)
    {

    }

    public void OnDragStart(RaycastHit hit)
    {

    }

    public void OnLeftDrag(RaycastHit hit)
    {
    }

    public void OnDragEnd(RaycastHit hit)
    {

    }

    public void OnShortRightClick(RaycastHit hit)
    {

    }

    public void OnRightClickWhileHolding(RaycastHit hit)
    {

    }

    public void OnRightClickWhileDragging(RaycastHit hit)
    {

    }

    public void OnmouseIn(RaycastHit hit)
    {

    }

    public void OnMouseOut(RaycastHit hit)
    {

    }

    public void OnSelect()
    {

    }

    public void OnDeselect()
    {

    }

    public void StopAction()
    {

    }
}
