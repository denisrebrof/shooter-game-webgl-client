using System;
using UnityEditor;
using UnityEngine;

namespace Utils.AutoId.Editor
{
    [CustomPropertyDrawer(typeof(AutoIdAttribute))]
    public class AutoIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.stringValue.Length == 0)
            {
                property.stringValue = Guid.NewGuid().ToString();
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}