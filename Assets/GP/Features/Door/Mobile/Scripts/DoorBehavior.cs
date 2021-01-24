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
        public GameObject hints;
        [SerializeField] private RectTransform hintsTextArea;
        private Vector3 initialHintsTextAreaScale;
        [SerializeField] private RectTransform hintsCircle;
        private Vector3 initialHintsCircleScale;
        [SerializeField] private GameObject padlock_Close;
        private Vector3 initialPadlockScale;
        [SerializeField] private GameObject door;
        [SerializeField] private Animator anim;
        [SerializeField] private UnityEngine.Events.UnityEvent _OnTouch;
        [SerializeField] private Canvas symbolCanvas;
        private Sequence s;
        private bool isSelected = false;
        public Text code;
        private RaycastHit hit;
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
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit))
                    {
                        if(hit.collider.gameObject == this.gameObject)
                        {
                            isSelected = true;
                            hints.SetActive(true);

                            StopAllCoroutines();
                            s.Kill();
                            s = DOTween.Sequence();
                            s.Append(padlock_Close.transform.DOScale(.25f, .2f).SetEase(Ease.OutBounce));
                            s.Append(padlock_Close.transform.DOScale(.2f, .2f).SetEase(Ease.Linear));
                            s.Play();
                            StartCoroutine(WaitBounceIDLE());
                            _OnTouch.Invoke();

                        }

                    }
                }

            }
        }

        IEnumerator WaitBounceIDLE()
        {
            if (!isSelected)
            {
                yield return new WaitForSeconds(3);
                s = DOTween.Sequence();
                s.Append(padlock_Close.transform.DOScale(initialPadlockScale + new Vector3(0.05f,0.05f,0.05f), .5f).SetEase(Ease.OutBounce));
                s.Append(padlock_Close.transform.DOScale(initialPadlockScale, 1f).SetEase(Ease.Linear));
                s.Play();
                
                StartCoroutine(WaitBounceIDLE());
                yield break;
            }

            else
            {
                yield return new WaitForSeconds(5);
                s = DOTween.Sequence();
                s.Append(hintsTextArea.DOScale(initialHintsTextAreaScale + new Vector3(0.25f, 0.25f, 0.25f), .5f).SetEase(Ease.OutBounce));
                s.Append(hintsTextArea.DOScale(initialHintsTextAreaScale, 1f).SetEase(Ease.Linear));
                s.Append(hintsCircle.DOScale(initialHintsCircleScale + new Vector3(0.5f, 0.5f, 0.5f), .5f).SetEase(Ease.OutBounce));
                s.Append(hintsCircle.DOScale(initialHintsCircleScale, 1f).SetEase(Ease.Linear));
                s.Play();

                StartCoroutine(WaitBounceIDLE());
                yield break;
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
                mat.DOColor(color_Close * .5f, "_EmissionColor", .5f);
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
                initialHintsTextAreaScale = hintsTextArea.localScale;
                initialHintsCircleScale = hintsCircle.localScale;
                initialPadlockScale = padlock_Close.transform.localScale;

                StartCoroutine(WaitBounceIDLE());
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
            hints.SetActive(false);
            StopAllCoroutines();
            s.Kill();

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

