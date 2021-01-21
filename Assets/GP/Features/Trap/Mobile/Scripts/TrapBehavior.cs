using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Gameplay.Mobile
{
    public class TrapBehavior : MonoBehaviour, ISwitchable
    {
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private SpriteRenderer icon;
        private Material mat;
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
            mat = new Material(mesh.material);
            mesh.material = mat;
        }

        private void Start() => Power = power;


        public void TurnOff() 
        {
            icon.DOColor(Color.grey, .5f);
            mat.DOColor(Color.red*.4f, "_EmissionColor", .5f);

        }
        public void TurnOn() 
        {
            icon.DOColor(Color.white, .5f);
            mat.DOColor(Color.red * 2, "_EmissionColor", .5f); 
        }

    }

}
