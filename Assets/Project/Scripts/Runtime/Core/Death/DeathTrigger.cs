using System;
using UnityEngine;
using Winter.Assets.Project.Scripts.Runtime.Core.Player;
using Winter.Assets.Project.Scripts.Runtime.Infrastructure.Scene.Root;

namespace Winter.Assets.Project.Scripts.Runtime.Core.Death
{
    [RequireComponent(typeof(Collider))]
    public class DeathTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.GetComponent<PlayerIndicator>() != null)
            {
                if (LevelBootstrap.Singleton_Instance)
                    LevelBootstrap.Singleton_Instance._deathHandler.SetDeath();
            }
                
        }
    }
}
