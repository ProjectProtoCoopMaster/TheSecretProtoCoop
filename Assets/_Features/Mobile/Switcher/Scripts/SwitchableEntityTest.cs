using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class SwitchableEntityTest : MonoBehaviour, ISwitchable
    {

        [Range(0,1),SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;
        [SerializeField] private Color offColor;
        [SerializeField] private Color onColor;

        private void OnEnable()
        {
            Power = power;
        }

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

        public void TurnOff() { GetComponent<Image>().color = offColor; }
        public void TurnOn() { GetComponent<Image>().color = onColor; }

        public void SwitchNode(int changeNodes)
        {
            throw new System.NotImplementedException();
        }
    }
}

