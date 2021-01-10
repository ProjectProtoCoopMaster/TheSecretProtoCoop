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
        [SerializeField] private Color color;
        [ SerializeField] private GameObject visionCone;
        private Material visionConeMat;
        [SerializeField] private GameObject visualCam;
        private Material visualCamMat;

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
            visionConeMat = new Material(visionCone.GetComponent<MeshRenderer>().material);
            visionCone.GetComponent<MeshRenderer>().material = visionConeMat;

            visualCamMat = new Material(visualCam.GetComponent<MeshRenderer>().material);
            visualCam.GetComponent<MeshRenderer>().material = visualCamMat;
            //color = GetComponent<Image>().color;
        }
        private void Start() => Power = power;

        public void TurnOff() 
        {
            visionConeMat.DOColor(Color.black * 0, "ScanLineColor", .5f);
            visionConeMat.DOFloat(0, "Speed1", .5f);
            visionConeMat.DOFloat(0, "Speed2", .5f);

            visualCamMat.DOColor(color, "_EmissionColor", .5f);
        }
        public void TurnOn()
        {
            visionConeMat.DOColor(Color.red * 2, "ScanLineColor", .5f);
            visionConeMat.DOFloat(0.01f, "Speed1", .5f);
            visionConeMat.DOFloat(0.02f, "Speed2", .5f);

            visualCamMat.DOColor(color * 3, "_EmissionColor", .5f);
        }
        //public void TurnOff() { GetComponent<Image>().DOColor(Color.black, .5f); visionLine.SetColors(Color.black, Color.black); }
        //public void TurnOn() { GetComponent<Image>().DOColor(color, .5f); visionLine.SetColors(Color.red, Color.red); }

    }
}

