using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace Olechka
{
    public class PS_handlers : MonoBehaviour
    {

        private void OnParticleSystemStopped()
        {
            LeanPool.Despawn(gameObject);
        }

    }
}