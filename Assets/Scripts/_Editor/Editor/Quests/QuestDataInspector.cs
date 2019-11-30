﻿using System.Collections;
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

    SerializedProperty cellTypeProp;
    SerializedProperty cellNbrToObtProp;

    SerializedProperty anyCellsProp;
    SerializedProperty coloCellProp;
    SerializedProperty transformToGetProp;
    SerializedProperty rangeProp;

    SerializedProperty destroyListProp;


    bool foldout = false;
    bool eventsFoldout = false;


    void OnEnable()
    {
        questTypeProp = serializedObject.FindProperty("questType");
        questTitleProp = serializedObject.FindProperty("questTitle");
        questDescriptionProp = serializedObject.FindProperty("questDescription");
        popObjProp = serializedObject.FindProperty("populationObjective");
        cellTypeProp = serializedObject.FindProperty("cellType");
        cellNbrToObtProp = serializedObject.FindProperty("cellNbrToObtain");
        anyCellsProp = serializedObject.FindProperty("anyCells");
        coloCellProp = serializedObject.FindProperty("colonialCellType");
        transformToGetProp = serializedObject.FindProperty("placeToGet");
        rangeProp = serializedObject.FindProperty("range");
        destroyListProp = serializedObject.FindProperty("objectToDestroy");
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

            case (int)QuestManager.QuestType.Colonisation:
                EditorGUILayout.PropertyField(anyCellsProp);
                EditorGUILayout.PropertyField(coloCellProp);
                EditorGUILayout.PropertyField(transformToGetProp);
                EditorGUILayout.PropertyField(rangeProp);

                break;

            case (int)QuestManager.QuestType.Destruction:

                EditorGUILayout.LabelField("Destruction", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                foldout = EditorGUILayout.Foldout(foldout, "Objects to destroy", true);
                destroyListProp.arraySize =  EditorGUILayout.IntField( destroyListProp.arraySize);
                EditorGUILayout.EndHorizontal();

                if (foldout)
                {
                    for (int i = 0; i < destroyListProp.arraySize; i++)
                    {
                        EditorGUILayout.ObjectField(destroyListProp.GetArrayElementAtIndex(i));
                    }
                }

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
