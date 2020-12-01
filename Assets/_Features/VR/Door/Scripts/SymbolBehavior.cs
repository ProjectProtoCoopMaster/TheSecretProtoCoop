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

        private void OnEnable()
        {
            sm = SymbolManager.instance;
            sm.symbol = this;
            _loadSymbols.Raise();
        }
        public void SetSymbols()
        {
            int random = Random.Range(0, 3);
            codeLines[random].codeText.text = sm.pickedNames[0];
            for (int i = 0; i < codeLines[random].symbols.Length; i++)
            {
                codeLines[random].symbols[i].overrideSprite = sm.iconsSelected[i];
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

