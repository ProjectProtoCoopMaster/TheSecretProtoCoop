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

        private int missNumber;
        [Header("---IMPORTANT---")]
        [SerializeField] private DoorBehavior door;
        [SerializeField] private Text symbolAreaCodeName;

        private float timer;
        private bool startTimer;
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
            if (canvas.enabled && !startTimer) { startTimer = true; timer = timerStartValue; }

            if (startTimer) timer -= Time.deltaTime;

            if (timer < 0) { ResetCodes(); startTimer = false; }
        }

        private void OnDisable()
        {
            sm.indexes.Clear();
        }
        public void ResetIcon(Image image)
        {
            image.overrideSprite = null;
        }

        public void SelectIcon(Image image)
        {
            for (int i = 0; i < iconsAnswers.Length; i++)
            {
                if (iconsAnswers[i].overrideSprite == null)
                {
                    
                    iconsAnswers[i].overrideSprite = image.overrideSprite;
                    i = iconsAnswers.Length;
                    break;
                }
            }
        }

        public void Unlock()
        {
            for (int i = 0; i < sm.iconsSelected.Count; i++)
            {
                if(sm.iconsSelected[i] != iconsAnswers[i].overrideSprite)
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

            door.Unlock();
            results[missNumber].color = Color.green;
            _sendOnOpenDoor.Raise();
            StartCoroutine(WaitCloseSymbolCanvas());
        }

        IEnumerator WaitCloseSymbolCanvas()
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
            yield break;
        }
        [Button]
        private void Miss()
        {
            missNumber++;
            results[missNumber - 1].color = Color.red;
            if (missNumber == 2)
            {
                _sendGameOver.Raise();
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
