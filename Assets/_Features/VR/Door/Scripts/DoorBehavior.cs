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
        [SerializeField] private Material red, orange, blue;
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
            anim.ResetTrigger("Close");
            anim.SetTrigger("Open");
            //if (lockState == LockState.Locked) { keyPassRenderer.material = orange; collider.enabled = false; }
            //else Unlock();
            Unlock();
        }

        public void TurnOff()
        {
            anim.ResetTrigger("Open");
            anim.SetTrigger("Close");
            //if (lockState == LockState.Locked) { keyPassRenderer.material = red; collider.enabled = false; }
            Lock();
        }

        [Button("Unlock")]
        public void Unlock()
        {
            anim.ResetTrigger("Close");
            anim.SetTrigger("Open");
            //collider.enabled = true;
            //keyPassRenderer.material = blue;
            lockState = LockState.Unlocked;
        }
        [Button("Lock")]
        public void Lock()
        {
            anim.ResetTrigger("Open");
            anim.SetTrigger("Close");
            //collider.enabled = true;
            //keyPassRenderer.material = blue;
            lockState = LockState.Locked;
        }

    }

}

