#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine;

namespace Gameplay.VR
{
    public class CameraBehavior : MonoBehaviour, ISwitchable
    {
        [SerializeField] CameraPowerBehaviour cameraPowerBehaviour;
        public UnityEvent camerasOff, camerasOn;

        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;

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
                if (power == 1) if (state == 1) TurnOn(); else TurnOff();
                else TurnOff();
            }
        }

        public GameObject MyGameObject { get { return this.gameObject; } set { MyGameObject = value; } }

        private void Awake()
        {
            Power = power;
        }

        [Button] public void TurnOff() => cameraPowerBehaviour.PowerOff();

        [Button] public void TurnOn() => cameraPowerBehaviour.PowerOn();
    }
}
#endif