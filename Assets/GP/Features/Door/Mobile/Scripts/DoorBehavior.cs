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

        private Material mat;
        private bool selectable;
        [SerializeField] private Transform mesh;

        public enum LockState { Locked, Unlocked }
        public LockState lockState;

        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;

        [Header("---References---")]
        [SerializeField] private Color color_Open_Unlocked;
        [SerializeField] private Color color_Open_Locked;
        [SerializeField] private Color color_Close;
        [SerializeField] private GameObject padlock_Open;
        [SerializeField] private GameObject hints;
        [SerializeField] private GameObject padlock_Close;
        [SerializeField] private GameObject door;
        [SerializeField] private Animator anim;
        [SerializeField] private Canvas symbolCanvas;
        public Text code;
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

            mat = new Material(door.GetComponent<MeshRenderer>().material);
            door.GetComponent<MeshRenderer>().material = mat;
            CheckLockState();

        }
        private void Start() => Power = power;

        private void Update()
        {
            if (selectable)
            {
                if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position)))
                    {
                        hints.SetActive(true);
                        
                    }
                }

            }
        }

        public void TurnOn()
        {
            mat.DOColor(color_Close * 2, "_EmissionColor", .5f);
        }

        public void TurnOff()
        {
            if (lockState == LockState.Locked)
            {
                mat.DOColor(color_Open_Locked * 2, "_EmissionColor", .5f);
            }
            else
            {
                mat.DOColor(color_Open_Unlocked * 2, "_EmissionColor", .5f);
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

                selectable =true;
                padlock_Close.SetActive(true);
                padlock_Open.SetActive(false);
            }
            else
            {
                FeedbackUnlock();
            }
        }

        private void FeedbackUnlock()
        {

            selectable = false;

            padlock_Close.SetActive(false);
            padlock_Open.SetActive(true);

            if (power == 1) TurnOn();
            else TurnOff();
        }

        public void RaiseLoadSymbols(CallableFunction loadSymbols) { loadSymbols.Raise(); }

        // Only for Debug !!
        [Button]
        public void ChangePower()
        {
            if (Power == 1) Power = 0;
            else Power = 1;
        }

        [Button]
        public void SetColliderOnMesh()
        {
            GetComponent<BoxCollider>().size = mesh.localScale;
            GetComponent<BoxCollider>().center = mesh.localPosition;
        }
        public void ShowCanvas()
        {
            symbolCanvas.enabled = true;
        }
    }
}

