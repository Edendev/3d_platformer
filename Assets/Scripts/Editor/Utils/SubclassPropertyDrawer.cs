using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Game.Utils;
using Utils;

#if UNITY_EDITOR

using UnityEditor;

namespace Editor.Utils
{
    /// <summary>
    /// Allows serializing abstract classes and interfaces.
    /// </summary>
    [CustomPropertyDrawer(typeof(SubclassSelectorPropertyAttribute))]
    public class SubclassPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            System.Type type = fieldInfo.FieldType;
            if (type.IsGenericList()) type = type.GetGenericArguments()[0];   
            if (type.IsArray) type = type.GetElementType();
            string typeName = property.managedReferenceValue?.GetType().Name ?? "Not set";

            Rect dropdownRect = position;
            dropdownRect.x += EditorGUIUtility.labelWidth + 2;
            dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;
            if (EditorGUI.DropdownButton(dropdownRect, new(typeName), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                
                menu.AddItem(new GUIContent("None"), property.managedReferenceValue == null, () =>
                {
                    property.managedReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                });
                
                foreach (System.Type assemblyType in Assembly.GetAssembly(type).GetTypes().Where(t => (t.IsSubclassOf(type) || type.IsAssignableFrom(t)) && !t.IsAbstract))
                {
                    menu.AddItem(new GUIContent(assemblyType.Name), typeName == assemblyType.Name, () =>
                    {
                        property.managedReferenceValue = assemblyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            }

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}

#endif
