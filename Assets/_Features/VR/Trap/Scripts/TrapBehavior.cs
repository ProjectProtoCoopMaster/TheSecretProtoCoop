using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class TrapBehavior : MonoBehaviour, ISwitchable
    {
        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;
        public GameObject MyGameObject { get { return this.gameObject; } set { MyGameObject = value; } }
        public int State { get { return state; } set { state = value; } }
        public int Power
        {
            get { return power; }
            set
            {
                power = value;
                if (power == 1) if (state == 1) TurnOn(); else TurnOff();
                else TurnOff();
            }
        }

        private void Start() => Power = power;

        public void TurnOff() { GetComponent<BoxCollider>().enabled = false; }
        public void TurnOn() { GetComponent<BoxCollider>().enabled = true; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IKillable>() != null)
                other.GetComponent<IKillable>().GE_Die();
        }

    }
}

