using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectPoolItem))]
public class PoolItemCustomInspector : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeigth = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float numberOfLines = 3;

        return lineHeigth * numberOfLines;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // base.OnGUI(position, property, label);  
        float lineHeigth = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //espace entre chaque rectangle 
        float spaceBetween = 10f;
        //taille d'un rectangle 


        Rect objectToPoolRect = new Rect(position.x, position.y + lineHeigth/4 , position.width, lineHeigth);

        Rect labelAmount = new Rect(position.x , position.y + objectToPoolRect.height + EditorGUIUtility.standardVerticalSpacing + lineHeigth / 4, position.width * 0.3f, lineHeigth);
        Rect AmountToPoolRect = new Rect(labelAmount.x + labelAmount.width, position.y + objectToPoolRect.height + EditorGUIUtility.standardVerticalSpacing + lineHeigth / 4, position.width * 0.12f, lineHeigth);
        Rect buttonRect1 = new Rect(AmountToPoolRect.x + AmountToPoolRect.width + 2, position.y + objectToPoolRect.height + EditorGUIUtility.standardVerticalSpacing + lineHeigth / 4, position.width * 0.1f, lineHeigth);
        Rect buttonRect2 = new Rect(buttonRect1.x + buttonRect1.width + 2, position.y + objectToPoolRect.height + EditorGUIUtility.standardVerticalSpacing + lineHeigth / 4, position.width * 0.1f, lineHeigth);

        Rect LabelExpand = new Rect(buttonRect2.x + buttonRect2.width - 15 , position.y + objectToPoolRect.height + EditorGUIUtility.standardVerticalSpacing + lineHeigth / 4, position.width * 0.25f, lineHeigth);
        Rect canExpandRect = new Rect(LabelExpand.x + LabelExpand.width -10  + spaceBetween/2, position.y + objectToPoolRect.height + EditorGUIUtility.standardVerticalSpacing + lineHeigth / 4, position.width * 0.25f, lineHeigth);

        Rect rectGlobal = new Rect(position.x, position.y, position.width, lineHeigth * 3f);

        Color baseColor = GUI.color;
        GUI.color = Color.blue;
        GUI.Box(rectGlobal, GUIContent.none);
        GUI.color = baseColor;

        SerializedProperty objectToPoolProp = property.FindPropertyRelative("objectToPool");
        EditorGUI.ObjectField(objectToPoolRect,objectToPoolProp);
    

        // EditorGUI.LabelField(labelAmount, "Amount to pool");
        SerializedProperty amouToPoolProperty = property.FindPropertyRelative("AmountToPool");
        EditorGUI.PropertyField(AmountToPoolRect, amouToPoolProperty , GUIContent.none);
        if (GUI.Button(buttonRect1 ,"+"))
        {
            amouToPoolProperty.intValue += 10;   
        }
        if (GUI.Button(buttonRect2 ,"-"))
        {
            amouToPoolProperty.intValue -= 10;
        }

        
       // EditorGUI.LabelField(LabelExpand,"Can Expand");
        SerializedProperty canExpandProperty = property.FindPropertyRelative("canExpand");
        EditorGUI.PropertyField(canExpandRect, canExpandProperty, GUIContent.none);
        if (objectToPoolProp.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Il manque un prefab dans l'une des pools", MessageType.Error);

        }
        if (position.width >=300)
        {
            EditorGUI.LabelField(labelAmount, "Amount to pool");
            EditorGUI.LabelField(LabelExpand, "Can Expand");
        }

    }
}
