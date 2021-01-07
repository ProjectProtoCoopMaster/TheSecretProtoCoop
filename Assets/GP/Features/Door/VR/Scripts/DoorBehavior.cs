using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay.VR
{
    public class DoorBehavior : MonoBehaviour, ISwitchable
    {
        public enum LockState { Locked, Unlocked }
        public LockState lockState;

        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;

        [SerializeField] private Collider collider;
        [SerializeField] private Material red, green, blue;
        [SerializeField] private Renderer keyPassRenderer;
        [SerializeField] private Animator anim;

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
                    if (state == 1) 
                        TurnOn(); 
                    else TurnOff();
                else TurnOff();
            }
        }

        public GameObject MyGameObject { get { return this.gameObject; } set { MyGameObject = value; } }

        private void Start() => Power = power;

        public void TurnOn()
        {
            if (lockState == LockState.Locked) keyPassRenderer.material = red;
            else keyPassRenderer.material = green;

            anim.ResetTrigger("Open");
            anim.SetTrigger("Close");
        }

        public void TurnOff()
        {
            if (lockState == LockState.Unlocked)
            {
                anim.ResetTrigger("Close");
                anim.SetTrigger("Open");

                keyPassRenderer.material = blue;
            }
            else keyPassRenderer.material = red;
        }

        [Button("Unlock")]
        public void Unlock()
        {
            lockState = LockState.Unlocked;

            if (Power == 0)
            {
                TurnOff(); 
            }
            else
            {
                keyPassRenderer.material = green;
            }
        }

        [InfoBox("Only for Debug")]
        [Button("Change Power")]
        public void ChangePower()
        {
            if (Power == 1) Power = 0;
            else Power = 1;
        }
    }
}

