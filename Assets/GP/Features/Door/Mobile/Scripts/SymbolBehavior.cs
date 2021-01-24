using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Gameplay.Mobile
{
    public class SymbolBehavior : MonoBehaviour, ISymbol
    {
        [SerializeField] private CallableFunction _sendOnOpenDoor;
        [SerializeField] private CallableFunction _sendGameOver;
        private SymbolManager sm;
        private Networking.TransmitterManager transmitterManager;
        [SerializeField] public Image[] iconsAnswers;
        [SerializeField] private Image[] results;
        [SerializeField] private Image[] iconsGame;
        [SerializeField] Text codeNameText;
        [SerializeField] Text timerText;
        [SerializeField] Image timerImage;
        [SerializeField] Sprite cross;
        [SerializeField] Sprite winImage;

        [SerializeField] private UnityEngine.Events.UnityEvent _OnMiss;
        [SerializeField] private UnityEngine.Events.UnityEvent _OnSucceed;

        private int missNumber;
        [Header("---IMPORTANT---")]
        [SerializeField] private DoorBehavior door;
        [SerializeField] private Text symbolAreaCodeName;

        private float timer;
        private bool startTimer;
        private bool hasWin;
        private bool isOpenOnce;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float timerStartValue;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            sm = SymbolManager.instance;
            sm.symbol = this;
            transmitterManager = Networking.TransmitterManager.instance;
        }

        private void Update()
        {
            if (canvas.enabled && !isOpenOnce) isOpenOnce = true;
            if (isOpenOnce)
            {
                if (!startTimer)
                {
                    startTimer = true;
                    timer = timerStartValue;
                }
 
            }

            if (startTimer && !hasWin)
            {
                timer -= Time.deltaTime;
                timerText.text = ((int)timer).ToString();
                timerImage.fillAmount = (timer / timerStartValue);
            }
            else
            {
                timerText.text = 0.ToString();
                timerImage.fillAmount = 0;
            }

            if (timer < 0 && startTimer) 
            { 
                ResetCodes(); startTimer = false; 
            }
        }

        private void OnDisable()
        {
            sm.indexes.Clear();
        }
        public void ResetIcon(Image image)
        {
            image.overrideSprite = null;
            image.color = new Color(0, 0, 0, 0);
        }

        public void SelectIcon(Image image)
        {
            for (int i = 0; i < iconsAnswers.Length; i++)
            {
                if (iconsAnswers[i].overrideSprite == null)
                {
                    iconsAnswers[i].color = Color.white;
                    iconsAnswers[i].overrideSprite = image.overrideSprite;
                    i = iconsAnswers.Length;
                    break;
                }
            }

            if (iconsAnswers[2].overrideSprite != null) Unlock();
        }

        public void Unlock()
        {
            for (int i = 0; i < sm.iconsSelected.Count; i++)
            {
                if (sm.iconsSelected[i] != iconsAnswers[i].overrideSprite)
                {
                    Miss();
                    i = sm.iconsSelected.Count;
                    break;
                }
                else
                {
                    if (i == sm.iconsSelected.Count - 1)
                    {
                        Succeed();

                        break;
                    }
                }
            }
        }

        [Button]
        private void Succeed()
        {
            _OnSucceed.Invoke();
            hasWin = true;
            door.Unlock();
            results[missNumber].gameObject.SetActive(true);
            results[missNumber].overrideSprite = winImage;
            results[missNumber].color = Color.green;
            StartCoroutine(WaitCloseSymbolCanvas());
            _sendOnOpenDoor.Raise();
        }

        IEnumerator WaitCloseSymbolCanvas()
        {
            yield return new WaitForSeconds(.5f);
            door.GetComponent<DoorBehavior>().hints.SetActive(false);
            canvas.enabled = false;
            gameObject.SetActive(false);
            yield break;
        }

        [Button]
        private void Miss()
        {
            _OnMiss.Invoke();
            missNumber++;
            results[missNumber - 1].gameObject.SetActive(true);
            results[missNumber - 1].overrideSprite = cross;

            foreach (Image answers in iconsAnswers)
            {
                answers.overrideSprite = null;
                answers.color = new Color(0, 0, 0, 0);
            }
            if (missNumber == 2)
            {
                Networking.TransmitterManager.instance.SendLoseToAll(5);
                door.GetComponent<DoorBehavior>().hints.SetActive(false);
                canvas.enabled = false;
                gameObject.SetActive(false);

            }
        }

        public void SetSymbols()
        {
            sm.iconsAssetReminder.Clear();
            sm.iconsStashed.Clear();
            
            for (int i = 0; i < sm.iconsAsset.Count; i++)
            {
                sm.iconsAssetReminder.Add(sm.iconsAsset[i]);
            }
            for (int i = 0; i < iconsGame.Length; i++)
            {
                iconsGame[i].overrideSprite = sm.iconsAsset[sm.indexes[i]];
                sm.iconsStashed.Add(sm.iconsAsset[sm.indexes[i]]);
                sm.iconsAsset.Remove(sm.iconsAsset[sm.indexes[i]]);
            }

            sm.iconsSelected.Clear();

            for (int i = 0; i < 3; i++)
            {
                sm.iconsSelected.Add(sm.iconsStashed[sm.indexes[i + iconsGame.Length]]);
            }

            sm.iconsAsset.Clear();

            for (int i = 0; i < sm.iconsAssetReminder.Count; i++)
            {
                sm.iconsAsset.Add(sm.iconsAssetReminder[i]);
            }

            codeNameText.text = sm.pickedNames[0];
            symbolAreaCodeName.text = sm.pickedNames[0];
        }

        private void ResetCodes()
        {
            sm.PickCodeName();

            codeNameText.text = sm.pickedNames[0];
            symbolAreaCodeName.text = sm.pickedNames[0];

            transmitterManager.SendCodeNameToOthers(sm.pickedNames);

            door.GetComponent<DoorBehavior>().code.text = sm.pickedNames[0];
        }
    }
}
