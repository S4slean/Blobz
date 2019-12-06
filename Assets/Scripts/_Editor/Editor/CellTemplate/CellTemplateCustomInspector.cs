﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CanEditMultipleObjects]
[CustomEditor(typeof(CelluleTemplate))]
public class CellTemplateCustomInspector : Editor
{
    SerializedProperty cellsEnableToBuildProp, blopPrefabProp, buttonColorProp, cellTextureProp;
    SerializedProperty typeProp;
    SerializedProperty energyCostProp, energyCapBaseProp, rangeBaseProp, blobRatioAtDeathProp, impulseForce_DeathProp;

    SerializedProperty prodPerTickProp, rejectPowerProp, storageCapabilityProp, linkCapabilityProp, tickForActivationBaseProp, energyPerClickProp;

    SerializedProperty proximityLevelMaxProp, positivesInteractionsProp, negativesInteractionsProp, StatsModificationProp;

    //Productrice Spé
    SerializedProperty SurprodRateProp;
    //Armory Spé
    SerializedProperty BlopPerTickProp;
    //Broyeuse Spé

    //Stockage Spé
    SerializedProperty stockageCapacityProp, LinkCapacityProp, RangeProp, tickForActivationProp;

    SerializedProperty InfoBoxToggleProp, refToggleProp, statToggleProp, ProductionGestionsProp, ProximityGestionProp;

    SerializedProperty energyPerblop, energyCapProp;

    SerializedProperty genererateProximityProp, proximityColliderNumberProp, proximityCollidersProp;

    ReorderableList proximityColliderList; 

    private float fieldWidthBase, labelWidthBase;


    private void OnEnable()
    {
        #region REFS
        cellsEnableToBuildProp = serializedObject.FindProperty("cellsEnableToBuild");
        blopPrefabProp = serializedObject.FindProperty("blopPrefab");
        buttonColorProp = serializedObject.FindProperty("buttonColor");
        cellTextureProp = serializedObject.FindProperty("cellTexture");
        #endregion
        typeProp = serializedObject.FindProperty("type");

        #region Proximity Initialisation 
        genererateProximityProp = serializedObject.FindProperty("generateProximity");
        proximityColliderNumberProp = serializedObject.FindProperty("proximityColliderNumber");
        proximityCollidersProp = serializedObject.FindProperty("proximityColliders");

        proximityColliderList = new ReorderableList(serializedObject, proximityCollidersProp, true, false, false, false);
        proximityColliderList.drawElementCallback += ElementCallBack;
        #endregion

        #region STATS
        energyCostProp = serializedObject.FindProperty("energyCost");
        energyCapBaseProp = serializedObject.FindProperty("energyCapBase");

        rangeBaseProp = serializedObject.FindProperty("rangeBase");
        blobRatioAtDeathProp = serializedObject.FindProperty("blobRatioAtDeath");
        impulseForce_DeathProp = serializedObject.FindProperty("impulseForce_Death");
        #endregion

        #region Production Gestion 
        prodPerTickProp = serializedObject.FindProperty("prodPerTickBase");
        rejectPowerProp = serializedObject.FindProperty("rejectPowerBase");
        storageCapabilityProp = serializedObject.FindProperty("storageCapability");
        linkCapabilityProp = serializedObject.FindProperty("linkCapability");
        tickForActivationBaseProp = serializedObject.FindProperty("tickForActivationBase");
        energyPerClickProp = serializedObject.FindProperty("energyPerClick");
        #endregion

        #region Proximity Gestion

        proximityLevelMaxProp = serializedObject.FindProperty("proximityLevelMax");
        positivesInteractionsProp = serializedObject.FindProperty("positivesInteractions");
        negativesInteractionsProp = serializedObject.FindProperty("negativesInteractions");
        StatsModificationProp = serializedObject.FindProperty("StatsModification");
        #endregion

        #region Variable affectées par la proximité

        SurprodRateProp = serializedObject.FindProperty("SurproductionRate");
        BlopPerTickProp = serializedObject.FindProperty("BlopPerTick");
        stockageCapacityProp = serializedObject.FindProperty("stockageCapacity");
        LinkCapacityProp = serializedObject.FindProperty("LinkCapacity");
        RangeProp = serializedObject.FindProperty("Range");
        tickForActivationProp = serializedObject.FindProperty("tickForActivation");
        energyCapProp = serializedObject.FindProperty("energyCap");

        #endregion

        #region Boolean Inspector
        InfoBoxToggleProp = serializedObject.FindProperty("ToggleInfoBox");
        refToggleProp = serializedObject.FindProperty("REFS");
        statToggleProp = serializedObject.FindProperty("STATS");
        ProductionGestionsProp = serializedObject.FindProperty("ProductionGestion");
        ProximityGestionProp = serializedObject.FindProperty("ProximityGestion");
        #endregion

        #region Specificité
        //Broyeur
        energyPerblop = serializedObject.FindProperty("energyPerblop");
        #endregion

        fieldWidthBase = EditorGUIUtility.fieldWidth;
        labelWidthBase = EditorGUIUtility.labelWidth;

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //REIMPORTANTS
        EditorGUI.indentLevel += 2;
        EditorGUILayout.PropertyField(InfoBoxToggleProp);
        EditorGUI.indentLevel -= 2;



        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField(typeProp);
        switch ((CellType)typeProp.enumValueIndex)
        {
            case CellType.Productrice:
                break;
            case CellType.Armory:
                break;
            case CellType.Stockage:
                break;
            case CellType.Broyeur:
                EditorGUILayout.PropertyField(energyPerblop);

                break;
            case CellType.Passage:
                break;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField(genererateProximityProp);
        if (genererateProximityProp.boolValue)
        {
            EditorGUILayout.HelpBox("Il s'agit du pourcentage de chance de surproduction", MessageType.Info);
            EditorGUILayout.PropertyField(proximityColliderNumberProp);

            proximityCollidersProp.arraySize = proximityColliderNumberProp.intValue;
            //DisplayArray(proximityCollidersProp, "Proximity Collider n°", true);
            DisplayProximityCollider(proximityCollidersProp, "Proximity Collider");
        }

        EditorGUILayout.EndVertical();



        EditorGUILayout.PropertyField(refToggleProp);
        if (refToggleProp.boolValue)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel += 1;

            DisplayArray(cellsEnableToBuildProp, "Cell To build");

            EditorGUILayout.PropertyField(blopPrefabProp);

            EditorGUILayout.PropertyField(buttonColorProp);
            EditorGUILayout.PropertyField(cellTextureProp);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(statToggleProp);
        if (statToggleProp.boolValue)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(energyCostProp);
            EditorGUILayout.PropertyField(rangeBaseProp);

            if (EditorGUIUtility.currentViewWidth < 485)
            {
                EditorGUILayout.PropertyField(blobRatioAtDeathProp);
                EditorGUILayout.PropertyField(impulseForce_DeathProp);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(energyPerClickProp);
                EditorGUILayout.PropertyField(energyCapBaseProp);

            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.fieldWidth = 30;
                EditorGUIUtility.labelWidth = 140;

                EditorGUILayout.PropertyField(blobRatioAtDeathProp);
                EditorGUILayout.PropertyField(impulseForce_DeathProp);

                EditorGUIUtility.labelWidth = labelWidthBase;
                EditorGUIUtility.fieldWidth = fieldWidthBase;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.fieldWidth = 30;
                EditorGUIUtility.labelWidth = 140;
                EditorGUILayout.PropertyField(energyPerClickProp);
                EditorGUILayout.PropertyField(energyCapBaseProp);
                EditorGUIUtility.labelWidth = labelWidthBase;
                EditorGUIUtility.fieldWidth = fieldWidthBase;
                EditorGUILayout.EndHorizontal();


            }

            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(ProductionGestionsProp);
        // EditorGUILayout.LabelField(EditorGUIUtility.currentViewWidth.ToString());
        if (ProductionGestionsProp.boolValue)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel += 1;


            if (EditorGUIUtility.currentViewWidth < 430)
            {
                EditorGUILayout.PropertyField(prodPerTickProp);
                EditorGUILayout.PropertyField(rejectPowerProp);
                EditorGUILayout.PropertyField(storageCapabilityProp);
                EditorGUILayout.PropertyField(linkCapabilityProp);
                EditorGUILayout.PropertyField(tickForActivationBaseProp);

            }
            else
            {
                EditorGUIUtility.fieldWidth = 30;
                EditorGUIUtility.labelWidth = 110;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(prodPerTickProp);
                EditorGUILayout.PropertyField(rejectPowerProp);

                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(storageCapabilityProp);
                EditorGUILayout.PropertyField(linkCapabilityProp);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(tickForActivationBaseProp);

                EditorGUILayout.EndHorizontal();



                EditorGUIUtility.labelWidth = labelWidthBase;
                EditorGUIUtility.fieldWidth = fieldWidthBase;
            }
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.PropertyField(ProximityGestionProp);
        if (ProximityGestionProp.boolValue)
        {
            EditorGUILayout.BeginVertical("Box");//1
            EditorGUI.indentLevel += 1;

            EditorGUILayout.PropertyField(proximityLevelMaxProp);
            InteractionArrayDisplay(positivesInteractionsProp, "Positve");
            InteractionArrayDisplay(negativesInteractionsProp, "Negative");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(StatsModificationProp);
            if (EditorGUI.EndChangeCheck()) ResetStats();
            StatsModifDisplay(StatsModificationProp);


            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();//1
        }


        serializedObject.ApplyModifiedProperties();
        // base.OnInspectorGUI();
    }

    private void DisplayArray(SerializedProperty array, string Label)
    {
        EditorGUILayout.BeginHorizontal();
        array.arraySize = EditorGUILayout.IntField(array.name, array.arraySize);
        if (array.arraySize <= 0 && InfoBoxToggleProp.boolValue)
        {
            EditorGUILayout.HelpBox("Aucun(e)" + Label, MessageType.Info);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < array.arraySize; i++)
            {

                EditorGUIUtility.labelWidth = 0.01f;
                SerializedProperty currentElement = array.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(Label + " " + (i).ToString());

                EditorGUILayout.PropertyField(currentElement);

                if (currentElement == null && InfoBoxToggleProp.boolValue)
                {
                    EditorGUILayout.HelpBox("Le Label est vide", MessageType.Warning);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = labelWidthBase;
            }

            EditorGUI.indentLevel -= 1;
        }
        serializedObject.ApplyModifiedProperties();
    }
    private void InteractionArrayDisplay(SerializedProperty array, string interactionType)
    {
        EditorGUILayout.BeginVertical("Box");//3
        EditorGUILayout.BeginHorizontal("Box");//4
        array.arraySize = EditorGUILayout.IntField(interactionType + "s" + " Interactions", array.arraySize);
        if (array.arraySize <= 0 && InfoBoxToggleProp.boolValue)
        {
            EditorGUILayout.HelpBox("La cellule n'a pas d'interactions" + interactionType + "s", MessageType.Info);
            EditorGUILayout.EndVertical();//3
            EditorGUILayout.EndHorizontal();//4
        }
        else
        {
            EditorGUILayout.EndHorizontal();//4
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < array.arraySize; i++)
            {
                EditorGUIUtility.labelWidth = 0.01f;
                SerializedProperty currentElement = array.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal("Box");//5

                EditorGUILayout.LabelField(interactionType + " interaction with : ");
                EditorGUILayout.PropertyField(currentElement);

                EditorGUIUtility.labelWidth = labelWidthBase;
                EditorGUILayout.EndHorizontal();//5
            }

            EditorGUILayout.EndVertical();//3
            EditorGUI.indentLevel -= 1;
        }
    }
    private void StatsModifDisplay(SerializedProperty statsModif)
    {
        switch ((StatsModificationType)statsModif.enumValueIndex)
        {
            case StatsModificationType.Surproduction:

                EditorGUILayout.HelpBox("Il s'agit du pourcentage de chance de surproduction", MessageType.Info);
                SurprodRateProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(SurprodRateProp, "Level");
                break;

            case StatsModificationType.RejectForce:

                EditorGUILayout.HelpBox("Nombre de blob éjecter par tick , attention les blob sont des entiers", MessageType.Info);
                BlopPerTickProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(BlopPerTickProp, "Level");
                break;

            case StatsModificationType.StockageCapacity:
                EditorGUILayout.HelpBox("Capacité total du bâtiment en fonction du level de proximité", MessageType.Info);
                stockageCapacityProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(stockageCapacityProp, "Level");
                break;

            case StatsModificationType.LinkCapacity:
                EditorGUILayout.HelpBox("Nombre maximum de lien du bâtiment en fonction du level de proximité", MessageType.Info);
                LinkCapacityProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(LinkCapacityProp, "Level");
                // DisplayArray(SurprodRateProp, "Level");
                break;

            case StatsModificationType.Range:
                EditorGUILayout.HelpBox("Longueur maximum des liens venant de ce bâtiment ( aussi de la proximité pour l'instant ) ", MessageType.Info);
                rangeBaseProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(rangeBaseProp, "Level");
                break;

            case StatsModificationType.TickForActivation:
                EditorGUILayout.HelpBox("Nombre de tick nécessaire avant l'action de la cellule ", MessageType.Info);
                tickForActivationProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(tickForActivationProp, "Level");
                break;

            case StatsModificationType.EnergyCap:
                EditorGUILayout.HelpBox("Modifie l'energie Cap en fonction de la proximité", MessageType.Info);
                energyCapProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(energyCapProp, "Level");
                break;

            case StatsModificationType.Aucune:
                EditorGUILayout.HelpBox("Le batiment n'est pas effecter par la proximité ", MessageType.Info);
                break;

            default:
                break;

        }

    }

    private void ElementCallBack(Rect rect , int index  , bool isActive , bool isFocused)
    {
        rect.yMin += 2;
        rect.xMax -= 4;
        EditorGUI.PropertyField(rect, proximityCollidersProp.GetArrayElementAtIndex(index), new GUIContent("ProximityCollider n°" + index.ToString()) );
    }

    private void ResetStats()
    {

    }

    private void DisplayProximityCollider(SerializedProperty array, string Label)
    {
        EditorGUILayout.BeginHorizontal();
        array.arraySize = EditorGUILayout.IntField(array.name, array.arraySize);
        if (array.arraySize <= 0 && InfoBoxToggleProp.boolValue)
        {
            EditorGUILayout.HelpBox("Aucun(e)" + Label, MessageType.Info);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < array.arraySize; i++)
            {

                EditorGUIUtility.labelWidth = 0.01f;
                SerializedProperty currentElement = array.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(Label + " " + (i).ToString());

                EditorGUILayout.PropertyField(currentElement);
                SerializedProperty currentElementProximityLevel =  currentElement.FindPropertyRelative("proximityLevel");
                SerializedProperty currentElementRange =  currentElement.FindPropertyRelative("range");
                currentElementProximityLevel.intValue = EditorGUILayout.IntField(currentElementProximityLevel.intValue);
                currentElementRange.intValue = EditorGUILayout.IntField(currentElementRange.intValue);


                if (currentElement == null && InfoBoxToggleProp.boolValue)
                {
                    EditorGUILayout.HelpBox("Le Label est vide", MessageType.Warning);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = labelWidthBase;
            }

            EditorGUI.indentLevel -= 1;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
