using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PopUp))]
public class QuestPopUpPropertyDrawer : PropertyDrawer
{
    float line = EditorGUIUtility.singleLineHeight + 2;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect box = new Rect(position.x +5 , position.y+5, position.width, position.height);
        EditorGUI.DrawRect(box, new Color(.2f,.2f,.2f,1));

        float line = EditorGUIUtility.singleLineHeight + 2;
        SerializedProperty rpgStyleProp = property.FindPropertyRelative("rpgStyle");
        SerializedProperty anchor = property.FindPropertyRelative("anchor");
        SerializedProperty offset = property.FindPropertyRelative("offset");
        SerializedProperty usingSprite = property.FindPropertyRelative("usingSprite");
        SerializedProperty sprite = property.FindPropertyRelative("sprite");
        SerializedProperty Title = property.FindPropertyRelative("Title");
        SerializedProperty Text = property.FindPropertyRelative("Text");

        if (sprite.objectReferenceValue == null)
        {
            GUI.Button(new Rect(position.x + 10, position.y + 10, 64, 64), "none");     

        }
        else
        {
            GUI.Button(
                new Rect(position.x + 10, position.y + 10, 64, 64),
                sprite.objectReferenceValue as Texture
                );
        }

        EditorGUI.LabelField(new Rect(position.x + 15 + 65, position.y + 10, 50, line),
           "Title");

        Title.stringValue =  EditorGUI.TextField(new Rect(position.x + 15 + 65 + 35, position.y + 10, position.width - 15 - 65 - 25 -10, line),
            Title.stringValue);

        EditorGUI.LabelField(new Rect(position.x + 15 + 65, position.y + 10 + line, 50, line),
            "Text");

        Text.stringValue =  EditorGUI.TextArea(new Rect(position.x + 15 + 65 + 35, position.y + 10 + line, position.width - 15 - 65 - 25 - 10, line * 2),
            Text.stringValue);

        EditorGUI.PropertyField(
            new Rect(position.x + 10, position.y + 10 + 64 , position.width -20, line)
            , sprite);

        EditorGUI.PropertyField(
           new Rect(position.x + 10, position.y + 10 + 64 + line, position.width - 20, line)
           , anchor);


        EditorGUI.PropertyField(
               new Rect(position.x + 10, position.y + 10 + 64 +2*line, position.width - 20, line)
               , offset);

        EditorGUI.PropertyField(
       new Rect(position.x + 10, position.y + 10 + 64 + 4 * line, (position.width - 20)/2, line)
       , rpgStyleProp);

        EditorGUI.PropertyField(
       new Rect((position.width - 20) / 2 + position.x + 10, position.y + 10 + 64 + 4 * line, (position.width - 20) / 2, line)
       , usingSprite);

        //EditorGUI.PropertyField(
        //    new Rect(position.x + 10, position.y + 10, 64, 64)
        //    , sprite);


    }

}
