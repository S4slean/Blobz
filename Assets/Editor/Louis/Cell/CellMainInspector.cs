using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CellMain), true)]
public class CellMainInspector : Editor
{
    SerializedProperty myCellTemplateProp;

    SerializedProperty NBlobProp, NLinkProp, NCurrentProximityProp, graphTransformProp, ProximityDectectionProp;
    //debugProperty 
    SerializedProperty linksProp, noMoreLinkProp, BlobNumberProp, hasBeenDropProp;

    SerializedProperty showRefProp, showDebugProp, showlinksProp;




    private void OnEnable()
    {
        myCellTemplateProp = serializedObject.FindProperty("myCellTemplate");

        NBlobProp = serializedObject.FindProperty("NBlob");
        NLinkProp = serializedObject.FindProperty("NLink");
        NCurrentProximityProp = serializedObject.FindProperty("NCurrentProximity");
        graphTransformProp = serializedObject.FindProperty("graphTransform");
        ProximityDectectionProp = serializedObject.FindProperty("ProximityDectection");

        linksProp = serializedObject.FindProperty("links");
        noMoreLinkProp = serializedObject.FindProperty("noMoreLink");
        BlobNumberProp = serializedObject.FindProperty("BlobNumber");
        hasBeenDropProp = serializedObject.FindProperty("hasBeenDrop");

        showRefProp = serializedObject.FindProperty("showRef");
        showDebugProp = serializedObject.FindProperty("showDebug");
        showlinksProp = serializedObject.FindProperty("showlinks");


    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();



        if (myCellTemplateProp.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Glisser l'un des scriptable object CellTemplate ici", MessageType.Warning);
        }
        EditorGUILayout.PropertyField(myCellTemplateProp);


        EditorGUILayout.PropertyField(showRefProp);
        //foldRef = EditorGUILayout.Foldout(foldRef, "Display REF VARIABLES", true);
        if (showRefProp.boolValue)
        {

            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(NBlobProp);
            EditorGUILayout.PropertyField(NLinkProp);
            EditorGUILayout.PropertyField(NCurrentProximityProp);
            EditorGUILayout.PropertyField(graphTransformProp);
            EditorGUILayout.PropertyField(ProximityDectectionProp);
            EditorGUI.indentLevel -= 1;
        }

        EditorGUILayout.PropertyField(showDebugProp);
        if (showDebugProp.boolValue)
        {
            EditorGUI.indentLevel += 1;
            EditorGUI.indentLevel += 2;

            EditorGUILayout.PropertyField(showlinksProp);
            EditorGUI.indentLevel -= 2;
            EditorGUILayout.BeginHorizontal();
            linksProp.arraySize = EditorGUILayout.IntField("Links Numbers", linksProp.arraySize);
            if (linksProp.arraySize > 0 && showlinksProp.boolValue)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel += 2;
                for (int i = 0; i < linksProp.arraySize; i++)
                {
                    SerializedProperty currentElement = linksProp.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(currentElement);

                }
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.PropertyField(noMoreLinkProp);
            }
            else
            {
                EditorGUILayout.PropertyField(noMoreLinkProp);
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.PropertyField(BlobNumberProp);
            EditorGUILayout.PropertyField(hasBeenDropProp);
            EditorGUI.indentLevel -= 1;
        }




        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }
}
