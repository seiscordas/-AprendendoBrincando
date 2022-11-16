using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ListToPopupAttribute : PropertyAttribute
{
    public Type Type;
    public string PropertyName;

    public ListToPopupAttribute(Type type, string propertyName)
    {
        Type = type;
        PropertyName = propertyName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ListToPopupAttribute listToPopup = attribute as ListToPopupAttribute;
        List<string> stringList = null;
        if (listToPopup.Type.GetField(listToPopup.PropertyName) != null)
        {
            stringList = listToPopup.Type.GetField(listToPopup.PropertyName).GetValue(listToPopup.Type) as List<string>;
        }
        if (stringList != null && stringList.Count != 0)
        {
            int selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue), 0);
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
