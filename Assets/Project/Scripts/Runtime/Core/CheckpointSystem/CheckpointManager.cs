using System.IO;
using UnityEngine;
using Winter.Assets.Project.Scripts.Runtime.Core.FreezeSystem;
using Winter.Assets.Project.Scripts.Runtime.Core.Player;
using Winter.Assets.Project.Scripts.Runtime.Infrastructure.Scene.Root;

namespace Shark.Systems.Checkpoints
{
    public struct CheckpointData
    {
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;

        public float freezeValue;

        public bool isActivated;
    }

    public static class CheckpointManager
    {
        public static bool HasSave => File.Exists(_path);
        public static readonly string _path = Path.Combine(Application.persistentDataPath, "checkpoint.json");

        public static FreezeController _freeze = GameObject.FindFirstObjectByType<FreezeController>();

        public static void Save(this Checkpoint checkpoint, CheckpointData data)
        {
            RefreshFreezeController();
            data.freezeValue = _freeze.timer;

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_path, json);
        }

        public static void TryLoadCheckpoint()
        {
            if (HasSave)
            {
                string json = File.ReadAllText(_path);
                CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);

                GameObject.FindFirstObjectByType<LevelBootstrap>().ReloadLevelFromCheckpoint(data);
            }
        }

        public static void Clear()
        {
            if (HasSave)
            {
                File.Delete(_path);
            }
        }

        private static void RefreshFreezeController()
        {
            if (_freeze == null)
            {
                _freeze = GameObject.FindFirstObjectByType<FreezeController>();
            }
        }
    }
}