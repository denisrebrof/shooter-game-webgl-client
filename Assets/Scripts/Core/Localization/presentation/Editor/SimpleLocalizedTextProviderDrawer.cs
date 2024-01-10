using UnityEditor;
using UnityEngine;

namespace Core.Localization.presentation.Editor
{
    [CustomPropertyDrawer(typeof(SimpleLocalizedTextProvider))]
    public class SimpleLocalizedTextProviderDrawer : PropertyDrawer
    {
        private const float TextsMargin = 2f;
        private const float PlaceholdersMargin = 2f;

        private static readonly GUIStyle PlaceholderStyle = new(EditorStyles.textField)
        {
            normal =
            {
                textColor = Color.gray
            }
        };


        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var width = position.width / 2f;
            var ruTextRect = new Rect(position.x, position.y, width - TextsMargin, position.height);
            var enTextRect = new Rect(position.x + width + TextsMargin, position.y, width, position.height);
            DrawProperty(ruTextRect, property.FindPropertyRelative("ruText"), "<Russian>");
            DrawProperty(enTextRect, property.FindPropertyRelative("enText"), "<English>");
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private static void DrawProperty(Rect position, SerializedProperty property, string propertyName)
        {
            EditorGUI.PropertyField(position, property, GUIContent.none);
            if (property.stringValue.Length > 0) return;

            var placeholderRect = new Rect(
                x: position.x + PlaceholdersMargin,
                y: position.y,
                width: position.width,
                height: position.height
            );
            EditorGUI.LabelField(placeholderRect, propertyName, PlaceholderStyle);
        }
    }
}