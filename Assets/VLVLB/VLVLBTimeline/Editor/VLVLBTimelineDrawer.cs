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
        int fieldCount = 58;
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

        var buttonWidth = 100;
        var buttonMargine = 4;
        var objectReferenceValue    = property.serializedObject.targetObject as VLVLBTimelineClip;
     
        var type                    = objectReferenceValue.GetType();
        var bindingAttr             = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        

        // Debug.Log(ptlPropsObject.IsTargetAlive());
        // Debug.Log(ptlPropsObject.objectReferenceValue);
        var isSave = objectReferenceValue.resolvedVlvlbClipProfile != null;
        string saveButtonText = isSave  ? "Save" : "Export";
        if (GUI.Button( new Rect(position.x+position.width-buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight), saveButtonText))
        {
            MethodInfo method = isSave 
                ? type.GetMethod("SaveProps", bindingAttr)
                : type.GetMethod("ExportProfile", bindingAttr);
           
            method.Invoke(objectReferenceValue,null);

        }



        var isLoadButton = objectReferenceValue.resolvedVlvlbClipProfile != null;

        // if (objectReferenceValue.ptlPropObject.defaultValue == null && objectReferenceValue.useProfile) isLoadButton = false;
        
        
        
        EditorGUI.BeginDisabledGroup(!isLoadButton);
        if (GUI.Button(new Rect(position.x+position.width-buttonWidth*2-buttonMargine, position.y, buttonWidth, EditorGUIUtility.singleLineHeight), "Load"))
        {
            
            var method                  = type.GetMethod( "LoadProps", bindingAttr );
            method.Invoke(objectReferenceValue,null);
            
            Debug.Log($"Load profile {objectReferenceValue.template.vlvlbClipProfile.name}");

        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        
        
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.BeginDisabledGroup(objectReferenceValue.useProfile && objectReferenceValue.ptlPropObject.defaultValue != null);
        defaultProps = PropertyDrawerUtility.DrawDefaultGUI(position,property,new GUIContent("props"));
        EditorGUI.EndDisabledGroup();
        position.y += PropertyDrawerUtility.GetDefaultPropertyHeight(property,new GUIContent("props"));

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
