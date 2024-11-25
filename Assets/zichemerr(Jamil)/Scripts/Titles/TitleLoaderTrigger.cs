using UnityEngine;
using UnityEngine.SceneManagement;
using Winter.Assets.Project.Scripts.Runtime.Core.Player;

public class TitleLoaderTrigger : MonoBehaviour
{
    [SerializeField] private int _indexTitleScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
            SceneManager.LoadScene(_indexTitleScene);
    }
}
