using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace Gameplay.VR
{
    public class SymbolBehavior : MonoBehaviour, ISymbol
    {
        private int digit;
        [SerializeField] private TMP_Text digitText;
        private SymbolManager sm;
        [SerializeField] private CallableFunction _loadSymbols;
        public CodeLine[] codeLines;
        [SerializeField] private List<string> codes;
        [SerializeField] private List<Sprite> icons;
        [SerializeField] private List<int> randoms;
        [SerializeField] private CallableFunction _sendOnOpenDoor;
        [SerializeField] private GameObject[] digits;
        private int random;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            sm = SymbolManager.instance;
            sm.symbol = this;
            _loadSymbols.Raise();
        }

        public void SetSymbols()
        {
            codes.Clear();
            icons.Clear();
            randoms.Clear();

            for (int i = 0; i < sm.iconsAsset.Count; i++)
            {
                icons.Add(sm.iconsAsset[i]);
            }

            for (int i = 0; i < sm.codeNames.Length; i++)
            {
                codes.Add(sm.codeNames[i]);
            }
            randoms.Clear();
            random = Random.Range(0, 3);
            randoms.Add(random);
            codeLines[random].codeText.text = sm.pickedNames[0];
            codes.Remove(sm.pickedNames[0]);

            for (int i = 0; i < codeLines[random].symbols.Length; i++)
            {

                codeLines[random].symbols[i].overrideSprite = sm.iconsSelected[i];
                icons.Remove(sm.iconsSelected[i]);
            }

            for (int i = 0; i < 2; i++)
            {

                random = Random.Range(0, 3);

                for (int j = 0; j < randoms.Count; j++)
                {
                    if (random == randoms[j])
                    {
                        j = randoms.Count;
                        i--;
                        break;
                    }

                    else
                    {
                        if (j == randoms.Count - 1)
                        {
                            int newRand = Random.Range(0, codes.Count);
                            codeLines[random].codeText.text = codes[newRand];
                            codes.Remove(codes[newRand]);
                            randoms.Add(random);
                            for (int k = 0; k < codeLines[random].symbols.Length; k++)
                            {
                                int newRand2 = Random.Range(0, sm.iconsStashed.Count);
                                codeLines[random].symbols[k].overrideSprite = sm.iconsStashed[newRand2];
                                sm.iconsStashed.Remove(sm.iconsStashed[newRand2]);
                            }
                            break;
                        }
                    }
                }


            }

        }
        [Button]
        public void ChangeSymbols()
        {
            for (int i = 0; i < 1; i++)
            {
                int newRand = Random.Range(0, 3);
                if(sm.pickedNames[0] == codeLines[newRand].codeText.text)
                {
                    i--;
                }
                else
                {
                    sm.pickedNames[0] = codeLines[newRand].codeText.text;

                    sm.iconsSelected[0] = codeLines[newRand].symbols[0].overrideSprite;
                    sm.iconsSelected[1] = codeLines[newRand].symbols[1].overrideSprite;
                    sm.iconsSelected[2] = codeLines[newRand].symbols[2].overrideSprite;

                    int ID = 0;
                    int ID2 = 0;
                    int ID3 = 0;

                    for (int j = 0; j < sm.iconsAsset.Count; j++)
                    {
                        if (sm.iconsSelected[0] == sm.iconsAsset[j])
                        {
                            ID = j;
                        }

                        if (sm.iconsSelected[1] == sm.iconsAsset[j])
                        {
                            ID2 = j;
                        }

                        if (sm.iconsSelected[2] == sm.iconsAsset[j])
                        {
                            ID3 = j;
                        }
                    }
                    Networking.TransmitterManager.instance.SendCodeNameAndSpritesToOthers(sm.pickedNames[0], ID, ID2, ID3);
                    break;

                }
            }


            
        }

        [Button]
        public void EnterDigit(int value)
        {

            digitText.text += value.ToString();
            if(digitText.text.Length == 3)
            {

                digit = int.Parse(digitText.text);
                if(digit == sm.digit)
                {
                    OpenDoor();
                    Debug.Log("Open");
                }
                else
                {
                    ResetDigit();
                    Debug.Log("Reset");
                }
            }

        }

        private void OpenDoor()
        {
            _sendOnOpenDoor.Raise();
            for (int i = 0; i < digits.Length; i++)
            {
                digits[i].SetActive(false);
            }
        }

        private void ResetDigit()
        {
            digitText.text = "";
            digit = -1;
        }



        [System.Serializable]
        public struct CodeLine
        {
            public Text codeText;
            public Image[] symbols;
        }
    }
}

