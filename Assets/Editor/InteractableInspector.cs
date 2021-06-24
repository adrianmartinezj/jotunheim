//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(Interactable))]
//public class InteractableInspector : Editor
//{
//    public SerializedProperty visualRadius;
//    private Interactable m_interactable;

//    private void OnEnable()
//    {
//        visualRadius = serializedObject.FindProperty("m_visualRadius");
//    }
//    public override void OnInspectorGUI()
//    {
//        //base.OnInspectorGUI();
//        serializedObject.Update();
//        EditorGUILayout.PropertyField(visualRadius);
//        m_interactable = (Interactable)target;
//        //if (m_interactable.visualRadius != visualRadius.floatValue)
//        //{
//        //    Debug.LogError("this is the new val: " + visualRadius.floatValue);
//        //}
//        serializedObject.ApplyModifiedProperties();
//        //EditorGUI.BeginChangeCheck();
//        //EditorGUI.PropertyField(r, sp, GUIContent.none);
//        //if (EditorGUI.EndChangeCheck())
//        //{
//        //    // Do something when the property changes 
//        //}
//    }

//}

////public override void OnInspectorGUI()
////{
////	spline = target as BezierSpline;
////	EditorGUI.BeginChangeCheck();
////	bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
////	if (EditorGUI.EndChangeCheck())
////	{
////		Undo.RecordObject(spline, "Toggle Loop");
////		EditorUtility.SetDirty(spline);
////		spline.Loop = loop;
////	}
////	if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
////	{
////		DrawSelectedPointInspector();
////	}
////	if (GUILayout.Button("Add Curve"))
////	{
////		Undo.RecordObject(spline, "Add Curve");
////		spline.AddCurve();
////		EditorUtility.SetDirty(spline);
////	}
////}
