using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;

namespace Gameplay.Mobile
{
    
    public class ElectricalLineBehavior : MonoBehaviour, ISwitchable
    {

        private Material mat;
        private Color color;
        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;
        public List<GameObject> cylinders;
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

            mat = new Material(cylinders[0].GetComponent<MeshRenderer>().material);
            color = mat.GetColor("_EmissionColor");
            for (int i = 0; i < cylinders.Count; i++)
            {
                cylinders[i].GetComponent<MeshRenderer>().material = mat;
            }
            
            
            Power = power;



        }

        public void TurnOff()
        {

            mat.DOColor(color * .4f, "_EmissionColor", .5f);
        }

        public void TurnOn()
        {
            mat.DOColor(color * 2, "_EmissionColor", .5f);
        }
        public void SwitchNode(int changeNodes)
        {
            throw new System.NotImplementedException();
        }
        
        public void AddCylinder(GameObject cylinder) => cylinders.Add(cylinder);

    }
}

