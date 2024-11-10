using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var keysProperty = property.FindPropertyRelative("keys");
        var valuesProperty = property.FindPropertyRelative("values");

        if (keysProperty.arraySize != valuesProperty.arraySize)
        {
            EditorGUI.LabelField(position, "Mismatched keys and values array sizes");
            return;
        }

        EditorGUI.indentLevel++;

        for (int i = 0; i < keysProperty.arraySize; i++)
        {
            var keyProperty = keysProperty.GetArrayElementAtIndex(i);
            var valueProperty = valuesProperty.GetArrayElementAtIndex(i);

            float halfWidth = position.width / 2;
            Rect keyRect = new Rect(position.x, position.y + i * EditorGUIUtility.singleLineHeight, halfWidth, EditorGUIUtility.singleLineHeight);
            Rect valueRect = new Rect(position.x + halfWidth, position.y + i * EditorGUIUtility.singleLineHeight, halfWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
        }

        if (GUI.Button(new Rect(position.x, position.y + keysProperty.arraySize * EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), "Add Element"))
        {
            keysProperty.arraySize++;
            valuesProperty.arraySize++;
        }

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var keysProperty = property.FindPropertyRelative("keys");
        return EditorGUIUtility.singleLineHeight * (keysProperty.arraySize + 1);
    }
}
