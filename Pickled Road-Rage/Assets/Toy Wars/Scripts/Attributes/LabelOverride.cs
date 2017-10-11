using UnityEngine;

#if UNITY_EDITOR
 using UnityEditor;
#endif

public class LabelOverrideAttribute : PropertyAttribute
{
    public string label;
    public LabelOverrideAttribute(string label)
    {
        this.label = label;
    }

#if UNITY_EDITOR
     [CustomPropertyDrawer( typeof(LabelOverrideAttribute) )]
     public class ThisPropertyDrawer : PropertyDrawer
     {
         public override void OnGUI ( Rect position , SerializedProperty property , GUIContent label )
         {
             var propertyAttribute = this.attribute as LabelOverrideAttribute;
             label.text = propertyAttribute.label;
             EditorGUI.PropertyField( position , property , label );
         }
     }
#endif
}
