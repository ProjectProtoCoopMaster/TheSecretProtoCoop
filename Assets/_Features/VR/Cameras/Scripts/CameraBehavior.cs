using UnityEngine;

namespace Gameplay.VR
{
    public class CameraBehavior : MonoBehaviour, ISwitchable
    {
        OverwatchBehavior overwatchBehavior;
        DetectionBehavior detectionBehavior;
        RotationBehavior rotationBehavior;

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
            overwatchBehavior = GetComponent<OverwatchBehavior>();
            detectionBehavior = GetComponent<DetectionBehavior>();
            try
            {
                rotationBehavior = GetComponent<RotationBehavior>();
            }
            finally { };
        }

        public void TurnOff()
        {
            Debug.Log("Off");
            overwatchBehavior.OverwatchOff();
            detectionBehavior.DetectionOff();
            if(rotationBehavior != null) rotationBehavior.RotationOff();
        }

        public void TurnOn()
        {
            Debug.Log("On");
            overwatchBehavior.OverwatchOn();
            detectionBehavior.DetectionOn();
            if (rotationBehavior != null) rotationBehavior.RotationOn();
        }
    }
}