using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;


[CustomPropertyDrawer(typeof(QuestEvent))]
public class QuestEventPropertyDrawer : PropertyDrawer
{

    SerializedProperty eventTypeProp;
    SerializedProperty ObjectToWatchProp;
    SerializedProperty popUpsMsgProp;
    SerializedProperty UEventProp;
    SerializedProperty foldoutProp;

    bool foldout;
    int type = 0;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        foldoutProp = property.FindPropertyRelative("foldout");
        eventTypeProp = property.FindPropertyRelative("eventType");

        switch (eventTypeProp.intValue)
        {
            case 0:
                ObjectToWatchProp = property.FindPropertyRelative("ObjectToWatch");
                if (!foldoutProp.boolValue)
                    return (EditorGUIUtility.singleLineHeight + 2) * 3;
                else
                    return (EditorGUIUtility.singleLineHeight + 2) * 3 + (EditorGUIUtility.singleLineHeight + 2) * ObjectToWatchProp.arraySize;


            case 1:
                popUpsMsgProp = property.FindPropertyRelative("popUpsMsg");
                if (!foldoutProp.boolValue)
                    return (EditorGUIUtility.singleLineHeight + 2) * 3;
                else
                    return (EditorGUIUtility.singleLineHeight + 2) * 3 + (EditorGUIUtility.singleLineHeight + 2) * popUpsMsgProp.arraySize * 2;

            case 3:
                UEventProp = property.FindPropertyRelative("UEvent");
                SerializedProperty calls = UEventProp.FindPropertyRelative("m_PersistentCalls.m_Calls");
                if (calls.arraySize < 2)
                {
                    return (EditorGUIUtility.singleLineHeight + 2) * 2 + (EditorGUIUtility.singleLineHeight + 2) * 4.5f;
                }
                else
                    return (EditorGUIUtility.singleLineHeight + 2 )*3.4f + (EditorGUIUtility.singleLineHeight + 2) * 2.5f * calls.arraySize ;

                

                
        }

        

        return base.GetPropertyHeight(property, label) * 10;
    }

    public int GetLength(QuestEvent qEvent)
    {
        
        return qEvent.UEvent.GetPersistentEventCount();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight + 2;

        eventTypeProp = property.FindPropertyRelative("eventType");
        ObjectToWatchProp = property.FindPropertyRelative("ObjectToWatch");
        popUpsMsgProp = property.FindPropertyRelative("popUpsMsg");
        UEventProp = property.FindPropertyRelative("UEvent");
        type = eventTypeProp.intValue;


        Rect box = new Rect(position.x, position.y + 5, position.width, position.height);
        EditorGUI.DrawRect(box, new Color(.15f, .15f, .15f, 1));
        //GUI.Button(box," ");

        EditorGUI.PropertyField(
            new Rect(position.x + 5, position.y + 10, position.width - 10, line),
            eventTypeProp);


        switch (eventTypeProp.intValue)
        {
            case 0:


                ObjectToWatchProp.arraySize = EditorGUI.IntField(
                     new Rect(position.x + 15, position.y + 10 + line, position.width - 25, line - 2),
                     "Nbr of Object to go through",
                     ObjectToWatchProp.arraySize);

                foldoutProp.boolValue = EditorGUI.Foldout(
                    new Rect(position.x + 15, position.y + 10 + line, position.width - 30, line - 2),
                    foldoutProp.boolValue, "");

                if (foldoutProp.boolValue)
                {
                    for (int i = 0; i < ObjectToWatchProp.arraySize; i++)
                    {
                        EditorGUI.PropertyField(
                            new Rect(position.x + 5, position.y + 10 + 2 * line + i * line, position.width - 10, line),
                            ObjectToWatchProp.GetArrayElementAtIndex(i));

                    }
                }


                break;



            case 1:

                popUpsMsgProp.arraySize = EditorGUI.IntField(
                     new Rect(position.x + 15, position.y + 10 + line, position.width - 25, line - 2),
                     "Nbr of Message",
                     popUpsMsgProp.arraySize);

                foldoutProp.boolValue = EditorGUI.Foldout(
                    new Rect(position.x + 15, position.y + 10 + line, position.width - 30, line - 2),
                    foldoutProp.boolValue, "");

                if (foldoutProp.boolValue)
                {
                    for (int i = 0; i < popUpsMsgProp.arraySize; i++)
                    {
                        EditorGUI.TextArea(new Rect(position.x + 5, position.y + 10 + 2 * line + i * line * 2, position.width - 10, line * 2),
                            popUpsMsgProp.GetArrayElementAtIndex(i).stringValue);

                    }
                }

                break;

            case 2:
                break;

            case 3:

                EditorGUI.PropertyField(
                    new Rect(position.x + 15, position.y + 10 + line, position.width - 25, line - 2),
                    UEventProp);

                break;
        }


    }
}
