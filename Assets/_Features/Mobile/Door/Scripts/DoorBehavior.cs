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
        private Color color;
        public enum LockState { Locked, Unlocked }
        public LockState lockState;
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
        private void OnEnable()
        {
            color = GetComponent<Image>().color;
            
        }
        private void Start() => Power = power;


        public void TurnOn()
        {
            if (lockState == LockState.Locked) GetComponent<Image>().DOColor(Color.red, .5f);
            else GetComponent<Image>().DOColor(Color.green, .5f);

        }

        public void TurnOff()
        {
            if (lockState == LockState.Unlocked) GetComponent<Image>().DOColor(Color.blue, .5f);
            else GetComponent<Image>().DOColor(Color.red, .5f);
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
                 GetComponent<Image>().DOColor(Color.green, .5f);
            }
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

