using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CanEditMultipleObjects]
[CustomEditor(typeof(CelluleTemplate))]
public class CellTemplateCustomInspector : Editor
{
    #region VARIABLES
    SerializedProperty cellsEnableToBuildProp, blopPrefabProp, buttonColorProp, cellTextureProp;
    SerializedProperty typeProp;
    SerializedProperty energyCostProp, energyCapBaseProp, rangeBaseProp, blobSpawnRatioAtDeathProp , blobSpawnAdditionnalRatioAtDeath;

    SerializedProperty prodPerTickProp, rejectPowerProp, storageCapabilityProp/*, linkCapabilityProp*/, tickForActivationBaseProp, energyPerClickProp;

    SerializedProperty proximityLevelMaxProp, positivesInteractionsProp, negativesInteractionsProp, StatsModificationProp;

    //Productrice Spé
    SerializedProperty SurprodRateProp;
    //Armory Spé
    SerializedProperty BlopPerTickProp;

    SerializedProperty speProp;

    SerializedProperty descriptionProp, descriptionProximityProp, descriptionClickProp;


    SerializedProperty stockageCapacityProp, LinkCapacityProp, RangeProp, tickForActivationProp;

    SerializedProperty InfoBoxToggleProp, refToggleProp, statToggleProp, ProductionGestionsProp, ProximityGestionProp, clickInteractionProp;

    SerializedProperty energyPerblop, energyCapProp;

    SerializedProperty overLoadTickMaxProp, overloadTreshHoldProp;

    SerializedProperty genererateProximityProp, proximityColliderNumberProp, proximityCollidersProp;

    SerializedProperty limitedInLinksProp, numberOfOuputLinksProp, numberOfInputLinksProp, slotDistanceProp, numberOfFlexLinksProp;

    SerializedProperty maxLifeProdProp;

    SerializedProperty shotPowerProp, verticalConstantPowerProp, verticalOffsetProp, magazinSlotDistanceProp;
    //ReorderableList proximityColliderList; 
    #endregion

    #region VARIABLES SPE 

    SerializedProperty MaxEnergieProp, tourelleMaxMunProp;
    SerializedProperty tourelleDamageProp, tourelleAttackRadiusProp;
    SerializedProperty maxBlobCoachProp;

    SerializedProperty magazineDragRangeProp, minDragRatioProp;

    SerializedProperty clickBeforeLaunchProp, blobLostPerTickProp;

    SerializedProperty maxBlobShreddedPerClickProp;

    SerializedProperty explosionRadiusProp;


    #endregion

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
        descriptionProp = serializedObject.FindProperty("description");
        descriptionProximityProp = serializedObject.FindProperty("descriptionProximity");
        descriptionClickProp = serializedObject.FindProperty("descriptionClick");


        #region Proximity Initialisation 
        genererateProximityProp = serializedObject.FindProperty("generateProximity");
        proximityColliderNumberProp = serializedObject.FindProperty("proximityColliderNumber");
        proximityCollidersProp = serializedObject.FindProperty("proximityColliders");

        //proximityColliderList = new ReorderableList(serializedObject, proximityCollidersProp, true, false, false, false);
        //proximityColliderList.drawElementCallback += ElementCallBack;
        #endregion

        #region STATS
        energyCostProp = serializedObject.FindProperty("energyCost");
        energyCapBaseProp = serializedObject.FindProperty("energyCapBase");

        rangeBaseProp = serializedObject.FindProperty("rangeBase");
        blobSpawnRatioAtDeathProp = serializedObject.FindProperty("blobSpawnRatioAtDeath");
        blobSpawnAdditionnalRatioAtDeath = serializedObject.FindProperty("blobSpawnAdditionnalRatioAtDeath");

        overLoadTickMaxProp = serializedObject.FindProperty("overLoadTickMax");
        overloadTreshHoldProp = serializedObject.FindProperty("overloadTreshHold");

        #endregion

        #region Production Gestion 
        prodPerTickProp = serializedObject.FindProperty("prodPerTickBase");
        rejectPowerProp = serializedObject.FindProperty("rejectPowerBase");
        storageCapabilityProp = serializedObject.FindProperty("storageCapability");
        //linkCapabilityProp = serializedObject.FindProperty("linkCapability");
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
        RangeProp = serializedObject.FindProperty("rangeLien");
        tickForActivationProp = serializedObject.FindProperty("tickForActivation");
        energyCapProp = serializedObject.FindProperty("energyCap");

        speProp = serializedObject.FindProperty("specifique");

        #endregion

        #region Boolean Inspector
        InfoBoxToggleProp = serializedObject.FindProperty("ToggleInfoBox");
        refToggleProp = serializedObject.FindProperty("REFS");
        statToggleProp = serializedObject.FindProperty("STATS");
        ProductionGestionsProp = serializedObject.FindProperty("ProductionGestion");
        ProximityGestionProp = serializedObject.FindProperty("ProximityGestion");

        clickInteractionProp = serializedObject.FindProperty("clickInteraction");
        #endregion

        #region Specificité
        //Broyeur
        energyPerblop = serializedObject.FindProperty("energyPerblop");
        MaxEnergieProp = serializedObject.FindProperty("maxEnergie");
        tourelleMaxMunProp = serializedObject.FindProperty("tourelleMaxMun");
        tourelleAttackRadiusProp = serializedObject.FindProperty("tourelleAttackRadius");
        tourelleDamageProp = serializedObject.FindProperty("tourelleDamage");

        maxBlobCoachProp = serializedObject.FindProperty("maxBlobCoach");

        magazineDragRangeProp = serializedObject.FindProperty("magazineDragRange");
        minDragRatioProp = serializedObject.FindProperty("minDragRatio");

        clickBeforeLaunchProp = serializedObject.FindProperty("clickBeforeLaunch");
        blobLostPerTickProp = serializedObject.FindProperty("blobLostPerTick");

        maxLifeProdProp = serializedObject.FindProperty("maxLifeProd");


        shotPowerProp = serializedObject.FindProperty("shotPower");
        verticalConstantPowerProp = serializedObject.FindProperty("verticalConstantPower");
        verticalOffsetProp = serializedObject.FindProperty("verticalOffset");
        magazinSlotDistanceProp = serializedObject.FindProperty("magazinSlotDistance");

        explosionRadiusProp = serializedObject.FindProperty("explosionRadius");

        maxBlobShreddedPerClickProp = serializedObject.FindProperty("maxBlobShreddedPerClick");



        #endregion

        #region LINK
        limitedInLinksProp = serializedObject.FindProperty("limitedInLinks");
        numberOfOuputLinksProp = serializedObject.FindProperty("numberOfOuputLinks");
        numberOfInputLinksProp = serializedObject.FindProperty("numberOfInputLinks");
        slotDistanceProp = serializedObject.FindProperty("slotDistance");
        numberOfFlexLinksProp = serializedObject.FindProperty("numberOfFlexLinks");
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


        #region TYPE

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.HelpBox("DESCRIPTION", MessageType.None);
        descriptionProp.stringValue = EditorGUILayout.TextArea(descriptionProp.stringValue, GUILayout.MaxHeight(80));
        EditorGUILayout.PropertyField(clickInteractionProp);
        if (clickInteractionProp.boolValue)
        {
            EditorGUILayout.HelpBox("DESCRIPTION INTERACTION", MessageType.None);
            descriptionClickProp.stringValue = EditorGUILayout.TextArea(descriptionClickProp.stringValue, GUILayout.MaxHeight(80));
        }


        switch ((CellType)typeProp.enumValueIndex)
        {
            case CellType.Nexus:
                EditorGUILayout.PropertyField(maxLifeProdProp);
                break;
            case CellType.Armory:
                break;
            case CellType.Stock:
                break;
            case CellType.Crusher:
                EditorGUILayout.PropertyField(energyPerblop);
                break;
            case CellType.Accelerator:
                break;

            case CellType.BlipBlop:
                break;
            case CellType.AerialStrike:
                EditorGUILayout.PropertyField(MaxEnergieProp);
                EditorGUILayout.PropertyField(explosionRadiusProp);
                break;
            case CellType.Turret:
                EditorGUILayout.PropertyField(tourelleMaxMunProp);
                EditorGUILayout.PropertyField(tourelleDamageProp);
                EditorGUILayout.PropertyField(tourelleAttackRadiusProp);
                break;
            case CellType.Gym:
                EditorGUILayout.PropertyField(maxBlobCoachProp);
                break;
            case CellType.Battery:
                EditorGUILayout.PropertyField(MaxEnergieProp);
                break;
            case CellType.Dump:
                EditorGUILayout.PropertyField(maxBlobShreddedPerClickProp);
                break;
            case CellType.Academy:
                break;
            case CellType.Treblobchet:
                EditorGUILayout.PropertyField(magazineDragRangeProp);
                EditorGUILayout.PropertyField(minDragRatioProp);
                EditorGUILayout.HelpBox("VARIABLE POUR LE LANCER", MessageType.None);
                EditorGUILayout.PropertyField(shotPowerProp);
                EditorGUILayout.PropertyField(verticalConstantPowerProp);
                EditorGUILayout.PropertyField(verticalOffsetProp);
                EditorGUILayout.PropertyField(magazinSlotDistanceProp);

                break;
            case CellType.Rocket:
                EditorGUILayout.PropertyField(maxLifeProdProp);
                EditorGUILayout.PropertyField(clickBeforeLaunchProp);
                EditorGUILayout.PropertyField(blobLostPerTickProp);
                break;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        #region REFS

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
        #endregion

        #region PROXIMITY

        EditorGUILayout.PropertyField(genererateProximityProp);
        if (genererateProximityProp.boolValue)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.HelpBox("Chaque Collider à :" +
                "" + "\n" + "- Un Proximity Level  correspondant au level de proximité donné à la cell dedans"
                + "\n" + "- Une Range au rayon du collider"
                , MessageType.Info);

            if (((CellType)typeProp.enumValueIndex) == CellType.Nexus)
            {
                EditorGUILayout.HelpBox("- Le Production Ratio Bonus est l'augmentation de la production en pourcentage " +
                    "de la productrice " +
                    ", les Bonus de chaque collider se cumule pour la meme Productrice", MessageType.Info);
            }


            EditorGUILayout.PropertyField(proximityColliderNumberProp);

            proximityCollidersProp.arraySize = proximityColliderNumberProp.intValue;
            //DisplayArray(proximityCollidersProp, "Proximity Collider n°", true);
            DisplayProximityCollider(proximityCollidersProp, "Proximity Collider");
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (((CellType)typeProp.enumValueIndex) != CellType.Nexus)
        {
            EditorGUILayout.PropertyField(ProximityGestionProp);
            if (ProximityGestionProp.boolValue)
            {
                EditorGUILayout.BeginVertical("Box");//1
                EditorGUI.indentLevel += 1;

                EditorGUILayout.PropertyField(proximityLevelMaxProp);
                //InteractionArrayDisplay(positivesInteractionsProp, "Positve");
                //InteractionArrayDisplay(negativesInteractionsProp, "Negative");
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(StatsModificationProp);
                if (EditorGUI.EndChangeCheck()) ResetStats();
                StatsModifDisplay(StatsModificationProp);


                EditorGUI.indentLevel -= 1;
                EditorGUILayout.EndVertical();//1
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        #endregion

        #region LINKS   

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(slotDistanceProp);
        EditorGUILayout.PropertyField(numberOfFlexLinksProp);
        EditorGUILayout.PropertyField(limitedInLinksProp);
        if (limitedInLinksProp.boolValue)
        {
            EditorGUIUtility.fieldWidth = 30;
            EditorGUIUtility.labelWidth = 200;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(numberOfOuputLinksProp);
            EditorGUILayout.PropertyField(numberOfInputLinksProp);
            EditorGUILayout.EndVertical();
            EditorGUIUtility.labelWidth = labelWidthBase;
            EditorGUIUtility.fieldWidth = fieldWidthBase;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        #region STATS

        EditorGUILayout.PropertyField(statToggleProp);
        if (statToggleProp.boolValue)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(energyCostProp);
            EditorGUILayout.PropertyField(rangeBaseProp);

            if (EditorGUIUtility.currentViewWidth < 485)
            {
                EditorGUILayout.PropertyField(blobSpawnRatioAtDeathProp);
                EditorGUILayout.PropertyField(blobSpawnAdditionnalRatioAtDeath);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(energyPerClickProp);
                EditorGUILayout.PropertyField(energyCapBaseProp);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(overLoadTickMaxProp);
                EditorGUILayout.PropertyField(overloadTreshHoldProp);

            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.fieldWidth = 30;
                EditorGUIUtility.labelWidth = 140;

                EditorGUILayout.PropertyField(blobSpawnRatioAtDeathProp);
                EditorGUILayout.PropertyField(blobSpawnAdditionnalRatioAtDeath);

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


                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.fieldWidth = 30;
                EditorGUIUtility.labelWidth = 140;
                EditorGUILayout.PropertyField(overLoadTickMaxProp);
                EditorGUILayout.PropertyField(overloadTreshHoldProp);
                EditorGUIUtility.labelWidth = labelWidthBase;
                EditorGUIUtility.fieldWidth = fieldWidthBase;
                EditorGUILayout.EndHorizontal();

            }

            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        #region PRODUCTIONS GESTION

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
                //EditorGUILayout.PropertyField(linkCapabilityProp);
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
                //EditorGUILayout.PropertyField(linkCapabilityProp);

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
        #endregion


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
        if ((StatsModificationType)statsModif.enumValueIndex != StatsModificationType.Aucune)
        {
            EditorGUILayout.HelpBox("DESCRIPTION MODIFICATION DE PROXIMITE", MessageType.None);
            descriptionProximityProp.stringValue = EditorGUILayout.TextArea(descriptionProximityProp.stringValue, GUILayout.MaxHeight(80));
        }


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

            case StatsModificationType.rangeLien:
                EditorGUILayout.HelpBox("Longueur maximum des liens venant de ce bâtiment ( aussi de la proximité pour l'instant ) ", MessageType.Info);
                RangeProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(RangeProp, "Level");
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
            case StatsModificationType.Spécifique:
                EditorGUILayout.HelpBox(
                    "Gere à la fois : " + "\n" +
                    "-La quantité de sploush obtenue (broyeur , décharge )  " + "\n" +
                    "-La range de tir (cell divine)  " + "\n" +
                    "-capacité dans lafile (Treblochet)  " + "\n" +
                    "-Durée de vie des coachs ou des explorateurs (Coach / Explo)"
                    , MessageType.Info);
                speProp.arraySize = proximityLevelMaxProp.intValue;
                DisplayArray(speProp, "Level");
                break;
            case StatsModificationType.Aucune:
                EditorGUILayout.HelpBox("Le batiment n'est pas effecter par la proximité ", MessageType.Info);
                break;
            default:
                break;

        }

    }
    //private void ElementCallBack(Rect rect , int index  , bool isActive , bool isFocused)
    //{
    //    rect.yMin += 2;
    //    rect.xMax -= 4;
    //    EditorGUI.PropertyField(rect, proximityCollidersProp.GetArrayElementAtIndex(index), new GUIContent("ProximityCollider n°" + index.ToString()) );
    //}
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
                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.PropertyField(currentElement);


                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField("Collider Proximity Level");
                SerializedProperty currentElementProximityLevel = currentElement.FindPropertyRelative("proximityLevel");
                currentElementProximityLevel.intValue = EditorGUILayout.IntField(currentElementProximityLevel.intValue);
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField("Collider Range");
                SerializedProperty currentElementRange = currentElement.FindPropertyRelative("range");
                currentElementRange.intValue = EditorGUILayout.IntField(currentElementRange.intValue);
                EditorGUILayout.EndHorizontal();

                if (((CellType)typeProp.enumValueIndex) == CellType.Nexus)
                {
                    EditorGUILayout.BeginHorizontal("Box");
                    EditorGUILayout.LabelField("Collider Production Ratio Bonus");
                    SerializedProperty currentElementProductionBonus = currentElement.FindPropertyRelative("productionBonusRatio");
                    currentElementProductionBonus.intValue = EditorGUILayout.IntField(currentElementProductionBonus.intValue);
                    EditorGUILayout.EndHorizontal();

                }



                if (currentElement == null && InfoBoxToggleProp.boolValue)
                {
                    EditorGUILayout.HelpBox("Le Label est vide", MessageType.Warning);
                }

                EditorGUILayout.EndVertical();
                EditorGUIUtility.labelWidth = labelWidthBase;
            }

            EditorGUI.indentLevel -= 1;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
