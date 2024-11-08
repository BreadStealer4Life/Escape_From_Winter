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

        if (GUILayout.Button("Переподготовить скрипт"))
        {
            myScript.Restart_preparation();
        }

        if (GUILayout.Button("Перезаписать стартовую позу"))
        {
            myScript.Restart_save_start_pose();
        }

        if (GUILayout.Button("Загрузить стартовую позу"))
        {
            myScript.Start_Load_pose();
        }

        if (GUILayout.Button("Сохранить новую позу"))
        {
            myScript.New_Save_pose();
        }

        for (int i = 0; i < myScript.Pose_array.Count; ++i)
        {
            myScript.Pose_array[i].Is_Foldout = EditorGUILayout.Foldout(myScript.Pose_array[i].Is_Foldout, "Поза : " + myScript.Pose_array[i].Name);

            if (myScript.Pose_array[i].Is_Foldout)
            {
                myScript.Pose_array[i].Name = EditorGUILayout.TextField(myScript.Pose_array[i].Name);

                if (GUILayout.Button("Удалить позу"))// + myScript.Pose_array[i].Name))
                    myScript.Pose_array[i].Delete();

                if (GUILayout.Button("Перезаписать позу"))// + myScript.Pose_array[i].Name))
                    myScript.Pose_array[i].Save();

                if (GUILayout.Button("Установить позу"))// + myScript.Pose_array[i].Name))
                    myScript.Pose_array[i].Load();

                EditorGUILayout.Space();
            }
        }
        
    }
    
}
}
