using UnityEngine;

namespace Shark.Systems.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        private CheckpointData _data;

        private void Start() => Refresh();
        private void OnValidate() => Refresh();

        private void Refresh()
        {
            _data.spawnPosition = transform.position;

            gameObject.SetActive(!_data.isActivated);
            enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.SetActive(_data.isActivated = false);
                this.Save(_data);
            }
        }
    }
}
