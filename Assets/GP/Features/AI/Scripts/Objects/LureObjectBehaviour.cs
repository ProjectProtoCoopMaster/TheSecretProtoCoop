using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(Rigidbody))]   
    public class LureObjectBehaviour : SoundMaker
    {
        public DistractionObjectGenerator generator;
        private bool isOnGround;
        public float timeBeforeRespawn;
        private float currentTime;

        // accessed by the Grab Behaviour
        internal Rigidbody rigidBody;

        new void Awake()
        {
            base.Awake();
            rigidBody = GetComponent<Rigidbody>();
        }

        new void Update()
        {
            base.Update();

            if (generator != null)
            {
                if (isOnGround)
                {
                    if (currentTime <= 0.0f)
                    {
                        isOnGround = false;

                        generator.PoolObject(gameObject);
                        generator.SpawnObject();
                    }
                    currentTime -= Time.deltaTime;
                } 
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > minimumVelocity)
            {
                MakeNoise();

                isOnGround = true;
                currentTime = timeBeforeRespawn;
            }
        }
    }
}