using UnityEngine;

namespace Shark.Systems.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        private CheckpointData _data;

        private void RefreshData()
        {
            enabled = false;

            _data.spawnPosition = transform.position;
            _data.spawnRotation = transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                RefreshData();

                this.Save(_data);

                gameObject.SetActive(_data.isActivated = false);
            }
        }
    }
}
