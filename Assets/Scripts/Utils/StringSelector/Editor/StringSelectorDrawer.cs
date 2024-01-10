using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.StringSelector.Editor
{
    [CustomPropertyDrawer(typeof(StringSelectorAttribute))]
    public class StringSelectorDrawer: PropertyDrawer
    {
        private int choiceIndex;

        private StringSelectorAttribute SelectorAttribute => (StringSelectorAttribute)attribute;
        private Dictionary<string, string> Choices => SelectorAttribute.GetEntries();
        private SourceState State => SelectorAttribute.GetSourceState();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            if (State == SourceState.Invalid)
            {
                property.stringValue = string.Empty;
                EditorGUI.LabelField(position, new GUIContent(property.displayName + ": Invalid source stats :("));
                return;
            }
            
            var currentIndex = FindEntryIndex(property);
            var options = Choices.Values.ToArray();
            var propertyName = property.displayName;
            
            choiceIndex = EditorGUI.Popup(position, propertyName, currentIndex, options);
            
            if (!EditorGUI.EndChangeCheck()) 
                return;
            
            property.stringValue = Choices.Keys.ToList()[choiceIndex];
        }
        
        private int FindEntryIndex(SerializedProperty property)
        {
            var currentId = property.stringValue;
            return Choices.Keys.ToList().FindIndex(entry => entry == currentId);
        }
    }
}