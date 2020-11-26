using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerBehavior : MonoBehaviour, IKillable
    {
        [SerializeField] private CallableFunction _GameOver;
        private bool isDead;
        public void GE_Die()
        {
            if (!isDead)
            {
                _GameOver.Raise();
                isDead = true;
            }

        }

    }
}

