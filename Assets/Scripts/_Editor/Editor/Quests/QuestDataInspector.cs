using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestData))]
public class QuestDataInspector : Editor
{
    SerializedProperty questTypeProp;
    SerializedProperty questTitleProp;
    SerializedProperty questDescriptionProp;
    SerializedProperty questEventsProp;

    SerializedProperty popObjProp;

    SerializedProperty energyProp;

    SerializedProperty cellTypeProp;
    SerializedProperty cellNbrToObtProp;

    SerializedProperty anyCellsProp;
    SerializedProperty coloCellProp;
    SerializedProperty transformToGetProp;
    SerializedProperty rangeProp;

    SerializedProperty destructTypProp;
    SerializedProperty nbrOfObjectProp;


    //bool foldout = false;
    bool eventsFoldout = false;


    void OnEnable()
    {
        questTypeProp = serializedObject.FindProperty("questType");
        questTitleProp = serializedObject.FindProperty("questTitle");
        questDescriptionProp = serializedObject.FindProperty("questDescription");
        popObjProp = serializedObject.FindProperty("populationObjective");

        energyProp = serializedObject.FindProperty("energyToObtain");

        cellTypeProp = serializedObject.FindProperty("cellType");
        cellNbrToObtProp = serializedObject.FindProperty("cellNbrToObtain");

        anyCellsProp = serializedObject.FindProperty("anyCells");
        coloCellProp = serializedObject.FindProperty("colonialCellType");
        transformToGetProp = serializedObject.FindProperty("placeToGet");
        rangeProp = serializedObject.FindProperty("range");

        destructTypProp = serializedObject.FindProperty("destructType");
        nbrOfObjectProp = serializedObject.FindProperty("nbrOfObject");


        questEventsProp = serializedObject.FindProperty("questEvents");

    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(questTypeProp);
        EditorGUILayout.PropertyField(questTitleProp);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("QuestDescription");
        questDescriptionProp.stringValue = EditorGUILayout.TextArea(questDescriptionProp.stringValue, GUILayout.MinHeight(50));
        EditorGUILayout.EndHorizontal();

        switch (questTypeProp.enumValueIndex)
        {
            case (int)QuestManager.QuestType.Batiments:
                EditorGUILayout.PropertyField(cellTypeProp);
                EditorGUILayout.PropertyField(cellNbrToObtProp);
                break;

            case (int)QuestManager.QuestType.Energy:
                EditorGUILayout.PropertyField(energyProp);
                break;





            case (int)QuestManager.QuestType.Destruction:

                EditorGUILayout.PropertyField(destructTypProp);
                EditorGUILayout.PropertyField(nbrOfObjectProp);


                break;

            case (int)QuestManager.QuestType.Population:
                EditorGUILayout.PropertyField(popObjProp);
                break;
        }


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Quest Events", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Quest Events");
        questEventsProp.arraySize = EditorGUILayout.IntField(questEventsProp.arraySize);
        EditorGUILayout.EndHorizontal();

        if (eventsFoldout)
        {
            for (int i = 0; i < questEventsProp.arraySize; i++)
            {
                EditorGUILayout.PropertyField(questEventsProp.GetArrayElementAtIndex(i));
            }
        }



        serializedObject.ApplyModifiedProperties();


    }
}
