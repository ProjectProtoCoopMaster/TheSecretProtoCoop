using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Networking;

namespace Gameplay
{
    public class SymbolManager : MonoBehaviour
    {
        public BoolVariable isSymbolLoaded;
        public GameEvent onOpenDoor;
        public GameEvent onResetCodes;

        public List<Sprite> iconsAsset;

        [ReadOnly] public List<Sprite> iconsAssetReminder;
        [ReadOnly] public List<Sprite> iconsSelected;
        [ReadOnly] public List<Sprite> iconsStashed;

        [SerializeField] public string[] codeNames;

        [ReadOnly] public string[] pickedNames;
        [ReadOnly] public List<int> indexes;

        public ISymbol symbol;

        public static SymbolManager instance;
        void OnEnable() { if (instance == null) instance = this; }

        void OnDisable() => isSymbolLoaded.Value = false;

        public void LoadSymbols()
        {
            if (!isSymbolLoaded.Value)
            {
                Debug.Log("Load Symbols");

                iconsSelected.Clear();
                iconsAssetReminder.Clear();
                iconsStashed.Clear();

                indexes.Clear();

                for (int i = 0; i < iconsAsset.Count; i++)
                {
                    iconsAssetReminder.Add(iconsAsset[i]);
                }

                for (int i = 0; i < 15; i++)
                {
                    int random = Random.Range(0, iconsAsset.Count);
                    indexes.Add(random);

                    iconsStashed.Add(iconsAsset[random]);
                    iconsAsset.Remove(iconsAsset[random]);
                }

                for (int i = 0; i < 3; i++)
                {
                    int randomSelected = Random.Range(0, iconsStashed.Count);
                    if (iconsSelected.Count != 0)
                    {
                        for (int j = 0; j < iconsSelected.Count; j++)
                        {
                            if (iconsStashed[randomSelected] == iconsSelected[j])
                            {
                                i--;
                                break;
                            }
                            else
                            {
                                if (j == iconsSelected.Count - 1)
                                {
                                    iconsSelected.Add(iconsStashed[randomSelected]);
                                    indexes.Add(randomSelected);

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        iconsSelected.Add(iconsStashed[randomSelected]);
                        indexes.Add(randomSelected);
                    }
                }

                iconsAsset.Clear();

                for (int i = 0; i < iconsAssetReminder.Count; i++)
                {
                    iconsAsset.Add(iconsAssetReminder[i]);
                }

                for (int i = 0; i < indexes.Count; i++)
                {
                    //Debug.Log("ID");
                    TransmitterManager.instance.SendSymbolIDToOther(indexes[i]);
                }

                PickCodeName();

                SetSymbols();

                TransmitterManager.instance.SendSetSymbolToOthers();

                for (int i = 0; i < 3; i++)
                {
                    TransmitterManager.instance.SendIconsSelectedToOthers(i,iconsStashed.IndexOf(iconsSelected[i]));
                }
                
                isSymbolLoaded.Value = true;
            }
        }

        public void SetSymbols()
        {
            if (symbol != null)
            {
                Debug.Log("Going To Set Symbol");
                symbol.SetSymbols();
            }
        }

        public void PickCodeName()
        {
            pickedNames = new string[3];
            int random = Random.Range(0, codeNames.Length);
            pickedNames[0] = codeNames[random];
            
            for (int i = 1; i < 3; i++)
            {
                int randomSelected = Random.Range(0, codeNames.Length);

                for (int j = 0; j < pickedNames.Length; j++)
                {
                    if (codeNames[randomSelected] == pickedNames[j])
                    {
                        i--;
                        break;
                    }
                    else
                    {
                        pickedNames[i] = codeNames[randomSelected];
                        break;
                    }
                }
            }

            TransmitterManager.instance.SendCodeNameToOthers(pickedNames);
        }
    }
}
