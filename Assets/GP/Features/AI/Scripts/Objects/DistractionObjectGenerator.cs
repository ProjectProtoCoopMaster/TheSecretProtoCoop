using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class DistractionObjectGenerator : MonoBehaviour
    {
        public Transform dispenserPool;
        public Transform dispenserSpawn;

        public GameObject accessTrap;
        private bool isOpen;

        private float currentTime;

        void Start()
        {
            SpawnObject();
        }

        public void PoolObject(GameObject gameObject)
        {
            gameObject.transform.parent = dispenserPool.transform;
        }

        public void SpawnObject()
        {
            accessTrap.SetActive(false);

            currentTime = 1f;
            isOpen = true;
        }

        void Update()
        {
            if (isOpen)
            {
                if (currentTime <= 0.0f)
                {
                    accessTrap.SetActive(true);

                    GameObject distractionObject = dispenserPool.GetChild(0).gameObject;

                    distractionObject.transform.parent = dispenserSpawn;
                    distractionObject.transform.localPosition = Vector3.zero;

                    isOpen = false;
                }

                currentTime -= Time.deltaTime;
            }
        }
    } 
}
