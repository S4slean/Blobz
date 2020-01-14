using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : PoolableObjects, PlayerAction
{
    [Header("Refs")]
    public GameObject goodGraph;
    public GameObject brokenGraph;
    public enum DestructType {none, enemyBuilding, enemyVillage, rock, mushroom, bush, tree , crystal, bigCrystal, barricade, target1, target2,target3, target4, target5, target6, target7, target8, target9, target10 };

    [Header("General")]
    public DestructType destructType = DestructType.none;
    private int remainingLife;
    public int maxLife = 5;
    private int count = 0;
    public bool isReapairable = false;
    public int repairDelay = 10;
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


    #region REPAIR
    public void Repair()
    {
        remainingLife = maxLife;
        isRuin = false;
        SwapGraph();

        if (isVillageNexus)
        {
            village.RestartVillageRepair();
            UIManager.Instance.HideVillageSelection();
        }
        TickManager.doTick -= RepairTick;
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
    public void RepairTick()
    {
        if (count > repairDelay)
        {
            Repair();
            Debug.Log("repairTick");
            count = 0;
        }
        else
        {
            count++;
        }
    }
    #endregion

    #region DESTRUCTION
    public void ReceiveDamage(int dmg)
    {
        remainingLife -= dmg;
        //Play damageAnim
        if (remainingLife < 1)
        {
            Destruction();
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
    public void Destruction()
    {

        QuestManager.instance.DestructionCheck(destructType);
        RessourceTracker.instance.EnergyVariation(splouchAtDestruction);

        if (!isReapairable)
        {
            if (spawnEnemiesOnDestruction)
                SpawnEnemies();

            //Insert Anim and put Delete at the end
            Delete(); //remove once anim is inserted
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
    public void Delete()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        TickManager.doTick -= RepairTick;
    }
    #endregion
    public void SwapGraph()
    {
        goodGraph.SetActive(!goodGraph.activeSelf);
        brokenGraph.SetActive(!brokenGraph.activeSelf);
    }





    #region PLAYER_ACTIONS


    public void OnLeftClickDown(RaycastHit hit)
    {

    }

    public void OnShortLeftClickUp(RaycastHit hit)
    {

        if (canBeDestroyedByClick && !isRuin)
        {
            ReceiveDamage(1);
            RessourceTracker.instance.EnergyVariation(splouchAtClick);
        }
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

    #endregion
}
