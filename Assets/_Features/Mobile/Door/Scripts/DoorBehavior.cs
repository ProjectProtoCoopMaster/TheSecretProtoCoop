using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
namespace Gameplay.Mobile
{
    public class DoorBehavior : MonoBehaviour, ISwitchable
    {
        public enum LockState { Locked, Unlocked }
        public LockState lockState;

        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;

        [Header("---References---")]
        [SerializeField] private Sprite door_open;
        [SerializeField] private Sprite door_close;
        [SerializeField] private Image door;
        [SerializeField] private Sprite padLock_open;
        [SerializeField] private Sprite padLock_close;
        [SerializeField] private Image padLock;
        [SerializeField] private Color color_open;
        [SerializeField] private Color color_close;
        [SerializeField] private Outline doorOutline;
        [SerializeField] private Animator anim;
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
        private void OnEnable()
        {
            CheckLockState();
        }
        private void Start() => Power = power;


        public void TurnOn()
        {
            door.overrideSprite = door_close;
            door.DOColor(Color.red, .5f);
        }

        public void TurnOff()
        {
            if (lockState == LockState.Locked)
            {
                door.overrideSprite = door_close;
                door.DOColor(color_close, .5f);
            }
            else
            {
                door.overrideSprite = door_open;
                door.DOColor(color_open, .5f);
            }
        }

        [Button("Unlock")]
        public void Unlock()
        {

            lockState = LockState.Unlocked;
            FeedbackUnlock();
            if (Power == 0)
            {
                TurnOff();
            }
        }

        private void CheckLockState()
        {
            if(lockState == LockState.Locked)
            {
                padLock.overrideSprite = padLock_close;
                padLock.DOColor(color_close, .5f);
                
                GetComponent<Button>().enabled = true;
                anim.enabled = true;
                doorOutline.enabled = true;
            }
            else
            {
                FeedbackUnlock();
            }
        }

        private void FeedbackUnlock()
        {
            padLock.overrideSprite = padLock_open;
            padLock.DOColor(color_open, .5f);
            door.DOColor(color_open, .5f);
            GetComponent<Button>().enabled = false;
            anim.enabled = false;
            doorOutline.enabled = false;
        }

        public void RaiseLoadSymbols(CallableFunction loadSymbols) { loadSymbols.Raise(); }

        // Only for Debug !!
        [Button]
        public void ChangePower()
        {
            if (Power == 1) Power = 0;
            else Power = 1;
        }


    }
}

