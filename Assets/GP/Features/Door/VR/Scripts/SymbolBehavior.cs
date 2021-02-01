using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Gameplay.VR
{
    public class SymbolBehavior : MonoBehaviour, ISymbol
    {

        private SymbolManager sm;
        [SerializeField] private CallableFunction _loadSymbols;
        public CodeLine[] codeLines;
        [SerializeField] private List<string> codes;
        [SerializeField] private List<Sprite> icons;
        [SerializeField] private List<int> randoms;
        private int random;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            sm = SymbolManager.instance;
            sm.symbol = this;
            _loadSymbols.Raise();
            //Debug.Log("Load");
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
                                int newRand2 = Random.Range(0, icons.Count);
                                codeLines[random].symbols[k].overrideSprite = icons[newRand2];
                                icons.Remove(icons[newRand2]);
                            }
                            break;
                        }
                    }
                }


            }

        }


        public void ResetSymbols()
        {
            SetSymbols();

            for (int i = 0; i < codeLines.Length; i++)
            {
                if(codeLines[i].symbols[0].overrideSprite == sm.iconsSelected[0])
                {
                    if (i == 0)
                    {
                        string textReminder = codeLines[2].codeText.text;
                        codeLines[2].codeText.text = codeLines[i].codeText.text;
                        codeLines[i].codeText.text = textReminder;
                    }
                    else
                    {
                        string textReminder = codeLines[i - 1].codeText.text;
                        codeLines[i - 1].codeText.text = codeLines[i].codeText.text;
                        codeLines[i].codeText.text = textReminder;
                    }


                }
            }
        }


        [System.Serializable]
        public struct CodeLine
        {
            public Text codeText;
            public Image[] symbols;
        }
    }
}

