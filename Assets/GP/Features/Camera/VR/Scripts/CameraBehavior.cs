using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine;

namespace Gameplay.VR
{
    public class CameraBehavior : MonoBehaviour, ISwitchable
    {
        public UnityEvent camerasOff, camerasOn;

        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;

        private int baseState, basePower;

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
            baseState = state;
            basePower = power;
            Power = power;
        }

        [Button]
        public void TurnOff()
        {
            camerasOff.Invoke();
        }

        [Button]
        public void TurnOn()
        {
            camerasOn.Invoke();
        }

        public void GE_RefreshScene()
        {
            State = baseState;
            Power = basePower;
        }
    }
}