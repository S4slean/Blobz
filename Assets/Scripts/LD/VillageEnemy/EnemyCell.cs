using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCell : Destructible
{
    public Animator domeAnim;

    public override void ReceiveDamage(int dmg)
    {
        remainingLife -= dmg;
        //Play damageAnim
        if (remainingLife < 1)
        {
            Destruction();
        }
        else
        {
            anim.Play("Bounce", -1, 0);
            domeAnim.Play("Bounce", -1, 0);
        }
    }

    public override void SpawnEnemies()
    {
        base.SpawnEnemies();
        domeAnim.Play("Bounce", -1, 0);
    }

    public override void Destruction()
    {
        QuestManager.instance.DestructionCheck(destructType);
        RessourceTracker.instance.EnergyVariation(splouchAtDestruction);

        if (!isReapairable)
        {
            if (spawnEnemiesOnDestruction)
                SpawnEnemies();


            anim.Play("Disappear");
            domeAnim.Play("Disapear");
            //Insert Anim and put Delete at the end
            //remove once anim is inserted
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

    public override void SwapGraph()
    {
        base.SwapGraph();
        domeAnim.SetBool("isRuin", isRuin);
    }
}
