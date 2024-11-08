using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Game / Other / Skeleton pose")]
    [DisallowMultipleComponent]
	public class Skeleton_pose : MonoBehaviour
	{

    [Tooltip("Главная кость")]
    [SerializeField]
    Transform Main_bone = null;

    [HideInInspector]
    [SerializeField]
    public Transform[] Bones_array = new Transform[0];

    
    [Tooltip("Лист поз")]
    [HideInInspector]
    [SerializeField]
    public List<Bone_save_parameter> Pose_array = new List<Bone_save_parameter>();

    [HideInInspector]
    [SerializeField]
    public Bone_save_parameter Start_Pose = null;

    [HideInInspector]
    [SerializeField]
    bool Preparation_bool = false;

    void OnValidate()
    {
        if (!Preparation_bool)
            Preparation();
    }
    
    void Preparation()
    {
        Restart_preparation();
        Restart_save_start_pose();

        Preparation_bool = true;
    }

   [ContextMenu("Переподготовить скрипт")]
    public void Restart_preparation()
    {
        if (Main_bone)
        {
            Bones_array = Main_bone.GetComponentsInChildren<Transform>();

            if(Start_Pose == null)
                Start_Pose = new Bone_save_parameter(this, 0, Bones_array);
            else
                Start_Pose.Save_base_bones(Bones_array);


            for (int x = 0; x < Pose_array.Count; x++)
            {
                Pose_array[x].Save_base_bones(Bones_array);
            }
        }
    }

    [ContextMenu("Перезаписать стартовую позу")]
    public void Restart_save_start_pose()
    {
        if (Bones_array.Length > 0)
        {
            Start_Pose.Save();
        }
    }

    [ContextMenu("Сохранить новую позу")]
    public void New_Save_pose()
    {
        Pose_array.Add(new Bone_save_parameter(this, Pose_array.Count, Bones_array));
        Pose_array[Pose_array.Count - 1].Save();
    }

    [ContextMenu("Загрузить стартовую позу")]
    public void Start_Load_pose()
    {
        if (Preparation_bool)
        {
            Start_Pose.Load();
        }
    }


    [ContextMenu("Удалить позу")]
    void Delete_pose(int _id)
    {
        if (Pose_array.Count > _id)
            Pose_array.RemoveAt(_id);
        else
            Debug.LogError("Такой позы по Id " + _id + " не существует!");
    }

    [System.Serializable]
    public class Bone_save_parameter
    {
        [Tooltip("Имя позы")]
        [SerializeField]
        public string Name;

        [HideInInspector]
        [SerializeField]
        Skeleton_pose Main_script = null;

        int Id = 0;

        [HideInInspector]
        [SerializeField]
        public Transform[] Bones_array = new Transform[0];

        [HideInInspector]
        [SerializeField]
        public Vector3[] Position = new Vector3[0];

        [HideInInspector]
        [SerializeField]
        public Quaternion[] Rotation = new Quaternion[0];

        [HideInInspector]
        [SerializeField]
        public Vector3[] Scale = new Vector3[0];

        [HideInInspector]
        [SerializeField]
        public bool Is_Foldout = false;

        public Bone_save_parameter(Skeleton_pose _main_script, int _id, Transform[] _bones_array)
        {
            Main_script = _main_script;
            Bones_array = _bones_array;
            Id = _id;
        }

        public void Save_base_bones(Transform[] _bones_array)
        {
            Bones_array = _bones_array;
        }

        public void Delete()
        {
            Main_script.Delete_pose(Id);
        }

        public void Save()
        {
            if (Position.Length != Bones_array.Length)
            {
                Position = new Vector3[Bones_array.Length];
                Rotation = new Quaternion[Bones_array.Length];
                Scale = new Vector3[Bones_array.Length];
            }


            for (int x = 0; x < Bones_array.Length; x++)
            {
                Position[x] = Bones_array[x].localPosition;
                Rotation[x] = Bones_array[x].localRotation;
                Scale[x] = Bones_array[x].localScale;
            }

        }

        public void Load()
        {
            for (int x = 0; x < Bones_array.Length; x++)
            {
                Bones_array[x].localPosition = Position[x];
                Bones_array[x].localRotation = Rotation[x];
                Bones_array[x].localScale = Scale[x];
            }

        }

    }
}
}