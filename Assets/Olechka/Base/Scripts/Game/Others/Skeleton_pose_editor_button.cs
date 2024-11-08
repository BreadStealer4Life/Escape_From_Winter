using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Olechka
{
	[CustomEditor(typeof(Skeleton_pose))]
	public class Skeleton_pose_editor_button : Editor
	{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Skeleton_pose myScript = (Skeleton_pose)target;

        if (GUILayout.Button("��������������� ������"))
        {
            myScript.Restart_preparation();
        }

        if (GUILayout.Button("������������ ��������� ����"))
        {
            myScript.Restart_save_start_pose();
        }

        if (GUILayout.Button("��������� ��������� ����"))
        {
            myScript.Start_Load_pose();
        }

        if (GUILayout.Button("��������� ����� ����"))
        {
            myScript.New_Save_pose();
        }

        for (int i = 0; i < myScript.Pose_array.Count; ++i)
        {
            myScript.Pose_array[i].Is_Foldout = EditorGUILayout.Foldout(myScript.Pose_array[i].Is_Foldout, "���� : " + myScript.Pose_array[i].Name);

            if (myScript.Pose_array[i].Is_Foldout)
            {
                myScript.Pose_array[i].Name = EditorGUILayout.TextField(myScript.Pose_array[i].Name);

                if (GUILayout.Button("������� ����"))// + myScript.Pose_array[i].Name))
                    myScript.Pose_array[i].Delete();

                if (GUILayout.Button("������������ ����"))// + myScript.Pose_array[i].Name))
                    myScript.Pose_array[i].Save();

                if (GUILayout.Button("���������� ����"))// + myScript.Pose_array[i].Name))
                    myScript.Pose_array[i].Load();

                EditorGUILayout.Space();
            }
        }
        
    }
    
}
}
