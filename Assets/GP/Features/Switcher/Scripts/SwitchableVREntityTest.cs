using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class SwitchableVREntityTest : MonoBehaviour, ISwitchable
    {
        [SerializeField] private Material offMat;
        [SerializeField] private Material onMat;

        [Header("---IMPORTANT---")]
        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;
        public GameObject MyGameObject { get { return this.gameObject; } set { MyGameObject = value; } }
        public int State
        {
            get { return state; }
            set { state = value; }

        }
        public int Power
        {
            get { return power; }
            set
            {
                power = value;
                if (power == 1)
                {
                    if (state == 1)
                    {
                        TurnOn();

                    }
                    else
                    {
                        TurnOff();
                    }
                }
                else
                {
                    TurnOff();
                }
            }
        }

        private void Start() => Power = power;

        public void TurnOff()
        {
            GetComponent<MeshRenderer>().material = offMat;
        }

        public void TurnOn()
        {
            GetComponent<MeshRenderer>().material = onMat;
        }

    }
}

