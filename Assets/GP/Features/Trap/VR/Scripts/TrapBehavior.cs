using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Gameplay.VR
{
    [RequireComponent(typeof(BoxCollider))]
    public class TrapBehavior : MonoBehaviour, ISwitchable
    {
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private ParticleSystem ps;
        [SerializeField] private AudioSource audioSource;

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
            Debug.Log("I collided with " + other.gameObject.name);

            if (other.GetComponentInParent<IKillable>() != null && !other.GetComponentInParent<IKillable>().isDead)
            {
                ps.Play();
                audioSource.Play();
                Debug.Log(other.gameObject.name);
                other.GetComponentInParent<IKillable>().Die(Vector3.zero);
            }
        }

        [Button]
        void SetColliderPosition()
        {
            GetComponent<BoxCollider>().size = mesh.transform.localScale;
        }
    }
}
