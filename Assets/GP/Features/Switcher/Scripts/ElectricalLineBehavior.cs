using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;

namespace Gameplay.Mobile
{
    public class ElectricalLineBehavior : MonoBehaviour, ISwitchable
    {
        [SerializeField] private Color2 color;

        private Material mat;
        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;
        [SerializeField] private LineRenderer line;
        public int State { get { return state; } set { state = value; } }
        public GameObject MyGameObject { get { return this.gameObject; } set { MyGameObject = value; } }
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
            color = new Color2(Color.white, Color.white);
            mat = new Material(GetComponent<LineRenderer>().material);
            GetComponent<LineRenderer>().material = mat;
            Power = power;



        }

        public void TurnOff()
        {
            mat.DOColor(Color.white * 0, "_EmissionColor", .5f);
            //line.DOColor(color, new Color2( Color.black,Color.black), .5f);
        }

        public void TurnOn()
        {
            mat.DOColor(Color.white * 2, "_EmissionColor", .5f);
            //line.DOColor(new Color2(Color.black, Color.black),color, .5f);
        }
        public void SwitchNode(int changeNodes)
        {
            throw new System.NotImplementedException();
        }

    }
}

