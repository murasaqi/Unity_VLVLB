using System.Reflection;
using UnityEditor;
using UnityEngine;
using VLVLB;

[CustomPropertyDrawer(typeof(VLVLBTimelineBehaviour))]
public class VLVLBTimelineDrawer : PropertyDrawer
{

    private SerializedProperty defaultProps = null;
    SerializedObject parentSerializedObject;
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 80;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    void Repaint()
    {
        foreach (var editor in ActiveEditorTracker.sharedTracker.activeEditors)
        {
            if (editor.serializedObject == parentSerializedObject)
            {
                editor.Repaint();
                return;
            }
        }
    }
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        parentSerializedObject = property.serializedObject;

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        SerializedProperty ptlPropsObject = property.FindPropertyRelative ("ptlPropsObject");

        
        // Debug.Log(ptlPropsObject.IsTargetAlive());
        
        EditorGUILayout.BeginHorizontal();

        var buttonWidth = 80;
        var buttonMargine = 8;
        var objectReferenceValue    = property.serializedObject.targetObject as VLVLBTimelineClip;
     
        var type                    = objectReferenceValue.GetType();
        var bindingAttr             = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        

        var isSave = objectReferenceValue.resolvedVlvlbClipProfile != null && !objectReferenceValue.useProfile;
        var isLoadButton = objectReferenceValue.resolvedVlvlbClipProfile != null && !objectReferenceValue.useProfile;
     
        EditorGUI.BeginDisabledGroup(!isSave);
        if (GUI.Button( new Rect(position.x+position.width-buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight), "Save"))
        {
            var method = type.GetMethod( "SaveProps", bindingAttr );
            method.Invoke(objectReferenceValue,null);
        }
        EditorGUI.EndDisabledGroup();
        
        EditorGUI.BeginDisabledGroup(!isLoadButton);
        if (GUI.Button( new Rect(position.x+position.width-buttonWidth*2-buttonMargine, position.y, buttonWidth, EditorGUIUtility.singleLineHeight), "Load"))
        {
            MethodInfo method = type.GetMethod("LoadProps", bindingAttr);
            method.Invoke(objectReferenceValue,null);
            Debug.Log($"Load profile {objectReferenceValue.template.vlvlbClipProfile.name}");
        }
        EditorGUI.EndDisabledGroup();
        
        if (GUI.Button(new Rect(position.x+position.width-buttonWidth*3-buttonMargine*2, position.y, buttonWidth, EditorGUIUtility.singleLineHeight), "Export"))
        {
            
            MethodInfo method = type.GetMethod("ExportProfile", bindingAttr);
            method.Invoke(objectReferenceValue,null);
            
            
        }
        EditorGUILayout.EndHorizontal();
        
        
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.BeginDisabledGroup(objectReferenceValue.useProfile);
        // property.serializedObject.FindProperty(property.propertyPath);
        defaultProps = PropertyDrawerUtility.DrawDefaultGUI(position,property.FindPropertyRelative("props"),new GUIContent("props"));
        EditorGUI.EndDisabledGroup();
        // EditorGUI.PropertyField(position,property.FindPropertyRelative("props"));
        // position.y += PropertyDrawerUtility.GetDefaultPropertyHeight(property,new GUIContent("props"));

    }
    
    private bool IsRequire(SerializedProperty property)
    {
        if (property.isArray)
            return property.arraySize == 0;
 
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue == 0;
            case SerializedPropertyType.Float:
                return property.floatValue == 0f;
            case SerializedPropertyType.String:
                return property.stringValue == "";
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue == null;
        }
 
        return false;
    }
    
    
}
