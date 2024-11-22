using UnityEngine;
using Winter.Assets.Project.Scripts.Runtime.Core.Player;

namespace Shark.Systems.Checkpoints
{
    public struct CheckpointData
    {
        public Vector3 spawnPosition;
        public bool isActivated;
    }

    public static class CheckpointManager
    {
        private static readonly string _checkpointKey = "LastCheckpoint";
        public static bool HasSave => PlayerPrefs.HasKey(_checkpointKey);

        public static void Save(this Checkpoint checkpoint, CheckpointData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(_checkpointKey, json);
            PlayerPrefs.Save();
        }

        public static void TryLoadCheckpoint(this PlayerController player)
        {
            if (HasSave)
            {
                string json = PlayerPrefs.GetString(_checkpointKey);
                CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);
                player.Spawn(data);
            }
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteKey(_checkpointKey);
        }
    }
}