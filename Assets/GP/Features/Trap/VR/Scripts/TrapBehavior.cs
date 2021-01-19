using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Gameplay.VR
{
    [RequireComponent(typeof(BoxCollider))]
    public class TrapBehavior : MonoBehaviour, ISwitchable
    {
        [SerializeField] private MeshRenderer mesh;

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

        private void Start()
        {
            Power = power;
        }

        public void TurnOff() { GetComponent<BoxCollider>().enabled = false; }
        public void TurnOn() { GetComponent<BoxCollider>().enabled = true; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.GetComponentInParent<IKillable>() != null) 
                other.transform.parent.GetComponentInParent<IKillable>().Die(Vector3.zero);
        }

        [Button]
        void SetColliderPosition()
        {
            GetComponent<BoxCollider>().size = mesh.transform.localScale;
        }
    }
}

