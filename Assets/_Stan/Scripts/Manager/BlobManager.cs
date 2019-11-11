﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobManager : MonoBehaviour
{
    public enum BlobType { normal, charged, soldier, mad };


    public static List<Blob> blobList = new List<Blob>();


    [Header("Normal variables")]
    public Material normalMat;
    [SerializeField][Range(0, 128)] private int ticksBeforeMad = 4;

    [Header("Charged variables")]
    public Material chargedMat;
    //[SerializeField][Range(0, 10000)] private int energyGain = 10;


    [Header("Soldier variables")]
    public Material soldierMat;
    [SerializeField] [Range(0,1000)] private float explosionTriggerRadius = 10;
    [SerializeField] [Range(0, 1000)] private float explosionRadius = 20;
    [SerializeField] [Range(0,1000)] private float detectionRadius = 10;
    private Transform targetTransform;


    [Header("Enemy variables")]
    public Material angryMat;
    [SerializeField] [Range(0, 10000)] private float jumpForce = 10;
    [SerializeField] [Range(0, 1000)] private float jumpHeight = 5;
    [SerializeField] [Range(0, 10000)] private float aggroRange = 10;
    [SerializeField] [Range(0, 100)] private int ticksBtwnJumps = 4;



    private void Start()
    {
        //ajoute la fonction onTick au delegate pour qu'elle s'effectue à chaque tick
        TickManager.doTick += onTick;
    }

    private void OnDestroy()
    {
        //retire la fonction onTick du delegate (pour éviter qu'elle ne soit appelées alors que le script n'existe plus)
        TickManager.doTick -= onTick;

    }


    //Comprtements qui s'effectue à chaque Tick
    public void onTick()
    {
        //Si il n'y a pas de blob dans la scène interrompt la fonction
        if (blobList.Count == 0)
            return;

        //prend un par un chaque blob de la scène
        foreach (Blob blob in blobList)
        {
            switch (blob.blobType)
            {
                //Si le blob est normal
                case BlobType.normal:

                    //Permet au blob de savoir depuis combien de ticks il vit
                    blob.tickCount++;
                    //si il est resté trop longtemps il se change en madblob
                    if (blob.tickCount > ticksBeforeMad)
                    {
                        blob.tickCount = 0;
                        ChangeBlobTypeTo(blob, BlobType.mad);
                    }


                    break;

                case BlobType.charged:

                    break;

                case BlobType.soldier:

                    blob.tickCount++;
                    //Check si le blob doit agir sur ce tick
                    if (blob.tickCount > ticksBtwnJumps)
                    {
                        blob.tickCount = 0;

                        //si le blob detecte un ennemi proche
                        if (CheckNearbyEnemies(blob))
                        {

                            //il calcule la bonne direction
                            Vector3 directionToTarget = targetTransform.position - blob.transform.position;

                            //si il est assez près: BOOM!
                            if (directionToTarget.magnitude < explosionTriggerRadius)
                            {
                                Explode(blob);
                            }
                            //sinon  il se rapproche
                            else
                            {
                                Jump(blob, targetTransform);
                            }
                        }
                        //si il ne detecte rien il se dépace de manière aléatoire
                        else
                        {
                            Jump(blob);
                        }
                    }
                   

                    break;

                case BlobType.mad:

                    //Si le blob est déjà accroché à une cell il ne fait rien
                    if (blob.isStuck)
                        return;

                    blob.tickCount++;
                    //Check si le blob doit agir sur ce tick
                    if (blob.tickCount > ticksBtwnJumps)
                    {
                        blob.tickCount = 0;

                        if (CheckNearbyCells(blob))
                        {
                            Jump(blob, targetTransform);
                        }
                        else
                        {
                            Jump(blob);
                        }
                    }

                    break;
            }
        }
    }

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
            detectedColliders = Physics.OverlapSphere(currentPos, detectionRadius, 1<<12);
            //Debug.Log(detectedColliders.Length + " enemy detected");

            //il check lequel est le plus près
            for (int i = 0; i < detectedColliders.Length; i++)
            {
                if (detectedColliders[i].GetComponent<Blob>().blobType != BlobType.mad)
                    continue;

                Vector3 directionToTarget = detectedColliders[i].transform.position - currentPos;
                float doSqrToTarget = directionToTarget.sqrMagnitude;
                //si il trouve un blob plus près il enregistre sa distance et son transform
                if (doSqrToTarget < closestDistSqr)
                {
                    closestDistSqr = doSqrToTarget;
                    targetTransform = detectedColliders[i].transform;
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
            detectedColliders = Physics.OverlapSphere(currentPos, aggroRange, 1<<11);
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


    //si le joueur click sur le blob on fait ça
   

    //permet de changer le type de Blob
    public void ChangeBlobTypeTo(Blob blob , BlobType newBlobType)
    {

        switch (newBlobType)
        {
            case BlobType.normal:
                blob.rd.material = normalMat;
                break;

            case BlobType.charged:
                blob.rd.material = chargedMat;
                break;

            case BlobType.soldier:
                blob.rd.material = soldierMat;
                break;

            case BlobType.mad:
                blob.rd.material = angryMat;
                break;
        }


        blob.blobType = newBlobType;
        //Debug.Log("Blob changed into " + blob.blobType);
    }

    public void Jump(Blob blob)
    {
        Vector2 jumpDir2D = Random.insideUnitCircle.normalized;
        Vector3 jumpDir = new Vector3(jumpDir2D.x, jumpHeight, jumpDir2D.y);
        blob.Jump(jumpDir * jumpForce);
        //Debug.Log("Jump");
    }

    public void Jump(Blob blob, Transform target)
    {
        Vector3 jumpDir = target.position - blob.transform.position;
        jumpDir = jumpDir.normalized;
        jumpDir = new Vector3(jumpDir.x, jumpHeight, jumpDir.z);
        blob.Jump(jumpDir * jumpForce);
        Debug.Log("Jump towards target "+ targetTransform.name);
    }

    public void Explode(Blob blob)
    {
        Collider[] touchedBlobs;
        touchedBlobs = Physics.OverlapSphere(blob.transform.position, explosionRadius, 1 << 12);
        foreach(Collider blobCol in touchedBlobs)
        {
            if(blobCol.GetComponent<Blob>().blobType == BlobType.mad)
            Destroy(blobCol.gameObject);
        }
        //Debug.Log("Soldier Explosed " + touchedBlobs.Length + " blobs");
        Destroy(blob.gameObject);
    }



}
