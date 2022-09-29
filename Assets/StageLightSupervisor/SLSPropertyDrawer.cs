// using System.Collections;
// using System.Collections.Generic;
// using StageLightSupervisor;
// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine;
// using UnityEngine.UIElements;
//
//
//
// [CustomPropertyDrawer(typeof(StageLightProperty<>))]
// public class StageLightPropertyAttributeDrawer : PropertyDrawer
// {
//     // public override VisualElement CreatePropertyGUI(SerializedProperty property)
//     // {
//     //     // Create property container element.
//     //     var container = new VisualElement();
//     //
//     //     container.style.flexDirection = FlexDirection.Row;
//     //     var nameField = new UnityEngine.UIElements.Label();
//     //     nameField.text = property.displayName;
//     //     var amountField = new PropertyField(property.FindPropertyRelative("propertyOverride"));
//     //     var unitField = new PropertyField(property.FindPropertyRelative("value"));
//     //
//     //     // Add fields to the container.
//     //     container.Add(nameField);
//     //     container.Add(amountField);
//     //     container.Add(unitField);
//     //     
//     //
//     //     return container;
//     // }
//
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//
//         // EditorGUILayout.PropertyField(property);
//         base.OnGUI(position, property, label);
//
//         // EditorGUILayout.BeginHorizontal();
//         EditorGUI.BeginChangeCheck();
//         EditorGUILayout.PropertyField(property);
//         // EditorGUILayout.LabelField(property.displayName);
//         // EditorGUILayout.PropertyField(property.FindPropertyRelative("propertyOverride"), GUIContent.none);
//         // EditorGUILayout.PropertyField(property.FindPropertyRelative("value"), GUIContent.none);
//         if(EditorGUI.EndChangeCheck())
//         {
//             property.serializedObject.ApplyModifiedProperties();
//         }
//         // EditorGUILayout.EndHorizontal();
//     }
// }
