using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestData))]
public class QuestDataInspector : Editor
{
    SerializedProperty questTypeProp;
    SerializedProperty questTitleProp;
    SerializedProperty popObjProp;
    SerializedProperty cellTypeProp;
    SerializedProperty cellNbrToObtProp;
    SerializedProperty anyCellsProp;
    SerializedProperty coloCellProp;
    SerializedProperty transformToGetProp;
    SerializedProperty rangeProp;
    SerializedProperty destroyListProp;


    void OnEnable()
    {
        questTypeProp = serializedObject.FindProperty("questType");
        questTitleProp = serializedObject.FindProperty("QuestTitle");
        popObjProp = serializedObject.FindProperty("populationObjective");
        cellTypeProp = serializedObject.FindProperty("cellType");
        cellNbrToObtProp = serializedObject.FindProperty("cellNbrToObtain");
        anyCellsProp = serializedObject.FindProperty("anyCells");
        coloCellProp = serializedObject.FindProperty("colonialCellType");
        transformToGetProp = serializedObject.FindProperty("placeToGet");
        rangeProp = serializedObject.FindProperty("range");
        destroyListProp = serializedObject.FindProperty("objectToDestroy");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(questTypeProp);

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
                EditorGUILayout.PropertyField(destroyListProp);
                break;

            case (int)QuestManager.QuestType.Population:
                EditorGUILayout.PropertyField(popObjProp);
                break;
        }


        serializedObject.ApplyModifiedProperties();


    }
}
