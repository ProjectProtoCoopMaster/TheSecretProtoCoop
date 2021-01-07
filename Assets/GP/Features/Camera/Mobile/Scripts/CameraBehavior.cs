using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Gameplay.Mobile
{
    public class CameraBehavior : MonoBehaviour, ISwitchable
    {
        //[SerializeField] private LineRenderer visionLine;
        private Color color;

        [Range(0, 1), SerializeField] private int state = 1;
        [Range(0, 1), SerializeField] private int power = 1;
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
            //color = GetComponent<Image>().color;
        }
        private void Start() => Power = power;

        public void TurnOff() {  }
        public void TurnOn() { }
        //public void TurnOff() { GetComponent<Image>().DOColor(Color.black, .5f); visionLine.SetColors(Color.black, Color.black); }
        //public void TurnOn() { GetComponent<Image>().DOColor(color, .5f); visionLine.SetColors(Color.red, Color.red); }

    }
}

