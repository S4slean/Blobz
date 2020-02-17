using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobManager : MonoBehaviour
{
    public enum BlobType { normal, explorateur, soldier, mad, coach, aucun };


    public static List<Blob> blobList = new List<Blob>();
    public static BlobManager instance;



    [Header("Normal variables")]
    public Material normalMat;
    [SerializeField] [Range(0, 128)] private int ticksBeforeMad = 4;



    [Header("Explo variables")]
    public Material exploMat;
    public int exploLifeSpan = 10;
    [SerializeField] [Range(0, 1000)] private int exploTicksBtwnJumps = 3;
    [Range(0, 1000)] public float exploJumpForce = 7;
    public float resourcesDetectionRadius = 3;
    public float exploFlagRadius = 5;

    //[SerializeField][Range(0, 10000)] private int energyGain = 10;


    [Header("Soldier variables")]
    public Material soldierMat;
    public int soldierLifeSpan = 10;
    [SerializeField] [Range(0, 1000)] private int soldierTicksBtwnJumps = 4;
    [Range(0, 1000)] public float soldierJumpForce = 5;
    public float soldierFlagRadius = 5;
    [SerializeField] [Range(0, 1000)] private float attackRange = 20;
    [SerializeField] [Range(0, 1000)] private float detectionRadius = 10;
    [SerializeField] [Range(0, 1000)] private int lifeTimeReductionOnAttack = 1;
    private Transform targetTransform;


    [Header("Enemy variables")]
    public Material angryMat;
    public bool enemiesHaveLifeTime = false;
    public int enemyLifeSpan = 10;
    [SerializeField] [Range(0, 100)] private int standardEnemyTicksBtwnJumps = 4;
    [SerializeField] [Range(0, 100)] private int AngryEnemyTicksBtwnJumps = 2;
    [Range(0, 1000)] public float enemyJumpForce = 5;
    [SerializeField] [Range(0, 10000)] private float aggroRange = 10;

    [Range(0, 100)] private int enemyTicksBtwnJumps = 4;

    private Blob blob;


    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //ajoute la fonction onTick au delegate pour qu'elle s'effectue à chaque tick
        TickManager.doTick += onTick;
    }
    private void OnDestroy()
    {
        //retire la fonction onTick du delegate (pour éviter qu'elle ne soit appelées alors que le script n'existe plus)
        TickManager.doTick -= onTick;

    }

    #region BLOB_BEHAVIOUR
    //Comprtements qui s'effectue à chaque Tick
    public void onTick()
    {
        //Si il n'y a pas de blob dans la scène interrompt la fonction
        if (blobList.Count == 0)
            return;

        //prend un par un chaque blob de la scène
        for (int i = 0; i < blobList.Count; i++)
        {
            blob = blobList[i];

            switch (blob.GetBlobType())
            {
                #region NORMAL
                //Si le blob est normal
                case BlobType.normal:

                    //Permet au blob de savoir depuis combien de ticks il vit
                    blob.tickCount++;
                    //si il est resté trop longtemps il se change en madblob
                    if (blob.tickCount > ticksBeforeMad)
                    {
                        blob.tickCount = 0;
                        blob.ChangeType(BlobType.mad);
                    }


                    break;
                #endregion

                #region EXPLO
                case BlobType.explorateur:

                    blob.tickCount++;
                    blob.lifeTime++;
                    if (blob.lifeTime > blob.LifeSpan)
                    {
                        blob.Destruct();
                    }

                    if (blob.tickCount > exploTicksBtwnJumps)
                    {
                        if (blob.carriedObject != null)
                        {
                            blob.JumpBackToCell();

                        }
                        else if (TryAttack(blob, 1))
                        {

                            
                        }
                        else if (blob.resourceTransform != null)
                        {

                            blob.JumpTowards(blob.resourceTransform);

                        }
                        else if (!blob.CheckIfInFlagRadius())
                        {

                            blob.JumpTowardFlag();

                        }
                        else if (CheckNearbyRessources(blob))
                        {

                            blob.JumpTowards(blob.resourceTransform);
                        }
                        else
                        {
                            blob.RandomJump();
                        }
                    blob.tickCount = 0;
                    }
                    break;
                #endregion

                #region SOLDIER
                case BlobType.soldier:

                    blob.tickCount++;
                    blob.lifeTime++;
                    if (blob.lifeTime > blob.LifeSpan)
                    {
                        blob.Destruct();
                    }

                    //Check si le blob doit agir sur ce tick
                    if (blob.tickCount > soldierTicksBtwnJumps)
                    {
                        blob.tickCount = 0;

                        //si le blob detecte un ennemi proche
                        if (CheckNearbyEnemies(blob))
                        {
                            Debug.Log("detected enemy");
                            //il calcule la bonne direction
                            Vector3 directionToTarget = targetTransform.position - blob.transform.position;

                            //si il est assez près: BOOM!
                            if (directionToTarget.magnitude < attackRange / 2)
                            {
                                TryAttack(blob, 1);
                                Debug.Log("Attack");
                            }
                            //sinon  il se rapproche
                            else
                            {
                                JumpTowards(blob, targetTransform);
                                Debug.Log("JumpTowardEnemy" + targetTransform.name);
                            }
                        }
                        //si il ne detecte rien il se dépace de manière aléatoire
                        else
                        {
                            if (!blob.CheckIfInFlagRadius())
                            {
                                JumpTowardsFlag(blob);
                                Debug.Log("JumpToFlag");
                            }
                            else
                            {
                                RandomJump(blob);
                                Debug.Log("RandomJump");
                            }
                        }
                    }


                    break;
                #endregion

                #region MAD
                case BlobType.mad:

                    blob.tickCount++;
                    if (enemiesHaveLifeTime)
                    {
                        blob.lifeTime++;
                        if (blob.lifeTime > enemyLifeSpan)
                        {
                            blob.Destruct();
                        }
                    }

                    //Si le blob est déjà accroché à une cell il ne fait rien
                    if (blob.isStuck)
                    {
                        blob.ReduceCapacity();
                    }
                    else
                    {

                        //Check si le blob doit agir sur ce tick
                        if (blob.tickCount > enemyTicksBtwnJumps)
                        {
                            blob.tickCount = 0;


                            if (TryAttack(blob, 1))
                            {

                            }
                            else if (CheckNearbyCells(blob))
                            {
                                enemyTicksBtwnJumps = AngryEnemyTicksBtwnJumps;
                                JumpTowards(blob, targetTransform);
                            }
                            else if (CheckNearbyEnemies(blob))
                            {
                                JumpTowards(blob, blob.tagetTransform);
                            }
                            else
                            {
                                enemyTicksBtwnJumps = standardEnemyTicksBtwnJumps;

                                if (blob.knowsNexus)
                                {
                                    JumpTowards(blob, LevelManager.instance.nexus.transform);
                                }
                                else if (blob.CheckIfOutOfVillage())
                                {
                                    JumpTowards(blob, blob.village.transform);
                                }
                                else
                                {
                                    RandomJump(blob);
                                }

                            }
                        }

                    }
                    break;
                    #endregion
            }
        }
    }
    #endregion

    #region CHECK_SURROUNDINGS
    private bool CheckNearbyEnemies(Blob blob)
    {
        targetTransform = blob.tagetTransform;


        //si il a pas de cible il check si il y en a une à proximioté
        if (targetTransform == null)
        {
            //stockage de la plus courte distance trouvée
            float closestDistSqr = Mathf.Infinity;
            Vector3 currentPos = blob.transform.position;

            //il récupère tout les blobs dans la sphere de detection
            Collider[] detectedColliders;
            detectedColliders = Physics.OverlapSphere(currentPos, detectionRadius, 1 << 12);
            //Debug.Log(detectedColliders.Length + " enemy detected");

            //il check lequel est le plus près
            for (int i = 0; i < detectedColliders.Length; i++)
            {
                BlobType detectedBlobType = detectedColliders[i].GetComponent<Blob>().GetBlobType();
                if (detectedBlobType == BlobType.mad && blob.GetBlobType() == BlobType.soldier
                    || detectedBlobType == BlobType.explorateur && blob.GetBlobType() == BlobType.mad
                    || detectedBlobType == BlobType.soldier && blob.GetBlobType() == BlobType.mad)
                {

                    Vector3 directionToTarget = detectedColliders[i].transform.position - currentPos;
                    float doSqrToTarget = directionToTarget.sqrMagnitude;
                    //si il trouve un blob plus près il enregistre sa distance et son transform
                    if (doSqrToTarget < closestDistSqr)
                    {
                        closestDistSqr = doSqrToTarget;
                        targetTransform = detectedColliders[i].transform;
                    }
                }
                else
                {
                    continue;
                }
            }
        }



        if (targetTransform != null)
        {
            //Debug.Log("Enemy Detected");
            return true;
        }
        else
            return false;
    }
    private bool CheckNearbyCells(Blob blob)
    {
        targetTransform = blob.tagetTransform;

        //si il a pas de cible il check si il y en a une à proximioté
        if (targetTransform == null)
        {
            //stockage de la plus courte distance trouvée
            float closestDistSqr = Mathf.Infinity;
            Vector3 currentPos = blob.transform.position;

            //il récupère tout les cells dans la sphere de detection
            Collider[] detectedColliders;
            detectedColliders = Physics.OverlapSphere(currentPos, aggroRange, 1 << 11);
            //Debug.Log(detectedColliders.Length + " cells detected");

            //il check laquelle est la plus près
            for (int i = 0; i < detectedColliders.Length; i++)
            {
                Vector3 directionToTarget = detectedColliders[i].transform.position - currentPos;
                float doSqrToTarget = directionToTarget.sqrMagnitude;
                //si il trouve une cell plus près il enregistre sa distance et son transform
                if (doSqrToTarget < closestDistSqr)
                {
                    closestDistSqr = doSqrToTarget;
                    targetTransform = detectedColliders[i].transform;
                }
            }
        }



        if (targetTransform != null)
        {
            //Debug.Log("Cell Detected");
            return true;
        }
        else
            return false;
    }
    private bool CheckNearbyRessources(Blob blob)
    {
        Collider[] touchedResources;
        touchedResources = Physics.OverlapSphere(blob.transform.position, resourcesDetectionRadius, 1 << 16);

        for (int i = 0; i < touchedResources.Length; i++)
        {
            if (touchedResources[i].TryGetComponent<Carriable>(out Carriable carriable))
            {
                blob.resourceTransform = carriable.transform;
                return true;
            }


            if (touchedResources[i].TryGetComponent<Destructible>(out Destructible destructible))
            {
                if (destructible.remainingLife <= 0)
                    continue;

                if (destructible.destructType == Destructible.DestructType.crystal
                    || destructible.destructType == Destructible.DestructType.ressources
                    || destructible.destructType == Destructible.DestructType.rock
                    || destructible.destructType == Destructible.DestructType.shroom
                    || destructible.destructType == Destructible.DestructType.tree)
                {
                    blob.resourceTransform = destructible.transform;
                    return true;
                }
            }
        }

        blob.resourceTransform = null;
        return false;


    }
    #endregion

    #region JUMPS
    public void JumpTowards(Blob blob, Transform target)
    {

        blob.JumpTowards(target);

    }
    public void JumpTowardsFlag(Blob blob)
    {
        blob.JumpTowardFlag();
    }
    public void JumpForward(Blob blob)
    {
        blob.JumpForward();
    }
    public void RandomJump(Blob blob)
    {
        blob.RandomJump();
    }
    #endregion

    #region ATTACK
    public bool TryAttack(Blob blob, int dmg)
    {
        Collider[] touchedBlobs;
        touchedBlobs = Physics.OverlapSphere(blob.transform.position, attackRange, 1 << 12);

        Collider[] touchedDestructibles;
        touchedDestructibles = Physics.OverlapSphere(blob.transform.position, attackRange, 1 << 15 | 1 << 16);

        if (blob.GetBlobType() == BlobType.soldier && touchedBlobs.Length > 0)
        {
            for (int i = 0; i < touchedBlobs.Length; i++)
            {

                if (touchedBlobs[i].GetComponent<Blob>().GetBlobType() == BlobType.mad)
                {

                    touchedBlobs[i].GetComponent<Blob>().ReceiveDamage();
                    blob.lifeTime += lifeTimeReductionOnAttack;
                    blob.anim.Play("Attack");
                    return true;
                }
            }
        }

        //Debug.Log("Soldier Explosed " + touchedBlobs.Length + " blobs");

        if (blob.GetBlobType() == BlobType.mad && touchedBlobs.Length > 0)
        {
            for (int i = 0; i < touchedBlobs.Length; i++)
            {

                if (touchedBlobs[i].GetComponent<Blob>().GetBlobType() == BlobType.explorateur)
                {
                    touchedBlobs[i].GetComponent<Blob>().ReceiveDamage();
                    blob.lifeTime += lifeTimeReductionOnAttack;
                    blob.anim.Play("Attack");
                    return true;
                }
            }

            return false;

        }

        if (touchedDestructibles.Length > 0)
        {

            for (int i = 0; i < touchedDestructibles.Length; i++)
            {
                if (blob.GetBlobType() == BlobType.explorateur && touchedDestructibles[i].TryGetComponent<Carriable>(out Carriable carriable))
                {
                    carriable.GetCarried(blob);
                    return true;
                }


                if (touchedDestructibles[i].TryGetComponent<Destructible>(out Destructible destructible))
                {
                    if (destructible.remainingLife <= 0)
                    {
                        continue;
                    }


                    if (blob.GetBlobType() == BlobType.soldier && (destructible.destructType == Destructible.DestructType.EnemyBlob
                        || destructible.destructType == Destructible.DestructType.all
                        || destructible.destructType == Destructible.DestructType.enemyCell
                        || destructible.destructType == Destructible.DestructType.enemyNexus
                        || destructible.destructType == Destructible.DestructType.barricade))
                    {
                        blob.anim.Play("Attack");
                        destructible.ReceiveDamage(dmg);
                        return true;

                    }
                    else if (blob.GetBlobType() == BlobType.explorateur && (destructible.destructType == Destructible.DestructType.ressources
                        || destructible.destructType == Destructible.DestructType.all
                        || destructible.destructType == Destructible.DestructType.shroom
                        || destructible.destructType == Destructible.DestructType.crystal))
                    {

                        blob.anim.Play("Attack");
                        destructible.ReceiveDamage(dmg);
                        if(destructible.remainingLife <= 0)
                        {
                            if (!CheckNearbyRessources(blob))
                            {
                                blob.resourceTransform = null;
                            }
                        }
                        return true;

                    }

                }
            }
        }

        return false;

    }
    #endregion



}
