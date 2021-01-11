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
        private SymbolManager symbolManager;
        [SerializeField] public Image[] iconsAnswers;
        [SerializeField] private Image[] results;
        [SerializeField] private Image[] iconsGame;
        [SerializeField] Text codeNameText;

        private int missNumber;
        [Header("---IMPORTANT---")]
        [SerializeField] private DoorBehavior door;
        [SerializeField] private Text symbolAreaCodeName;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            symbolManager = SymbolManager.instance;
            symbolManager.symbol = this;
        }

        private void OnDisable()
        {
            symbolManager.indexes.Clear();
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
            for (int i = 0; i < symbolManager.iconsSelected.Count; i++)
            {
                if(symbolManager.iconsSelected[i] != iconsAnswers[i].overrideSprite)
                {
                    Miss();
                    i = symbolManager.iconsSelected.Count;
                    break;
                }
                else
                {
                    if (i == symbolManager.iconsSelected.Count - 1)
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
            symbolManager.iconsAssetReminder.Clear();
            symbolManager.iconsStashed.Clear();
            
            for (int i = 0; i < symbolManager.iconsAsset.Count; i++)
            {
                symbolManager.iconsAssetReminder.Add(symbolManager.iconsAsset[i]);
            }
            for (int i = 0; i < iconsGame.Length; i++)
            {

                iconsGame[i].overrideSprite = symbolManager.iconsAsset[symbolManager.indexes[i]];
                symbolManager.iconsStashed.Add(symbolManager.iconsAsset[symbolManager.indexes[i]]);
                symbolManager.iconsAsset.Remove(symbolManager.iconsAsset[symbolManager.indexes[i]]);
            }
            symbolManager.iconsSelected.Clear();

            for (int i = 0; i < 3; i++)
            {
                symbolManager.iconsSelected.Add(symbolManager.iconsStashed[symbolManager.indexes[i + iconsGame.Length]]);

            }

            symbolManager.iconsAsset.Clear();
            for (int i = 0; i < symbolManager.iconsAssetReminder.Count; i++)
            {
                symbolManager.iconsAsset.Add(symbolManager.iconsAssetReminder[i]);
            }

            codeNameText.text = symbolManager.pickedNames[0];
            symbolAreaCodeName.text = symbolManager.pickedNames[0];

        }
    }

}
