using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public class SymbolManager : MonoBehaviour
    {
        [SerializeField] private List<Sprite> iconsAsset;
        [SerializeField] private List<Sprite> iconsAssetReminder;
        [SerializeField] private Image[] iconsGame;
        [SerializeField] private List<Sprite> iconsSelected;
        [SerializeField] private Networking.TransmitterManager transmitterManager;
        public List<int> indexes;
        
        [Button]
        public void ChargeSymbols()
        {
            iconsSelected.Clear();
            iconsAssetReminder.Clear();
            indexes.Clear();
            for (int i = 0; i < iconsGame.Length; i++)
            {
                int random = Random.Range(0, iconsAsset.Count);
                indexes.Add(random);
                iconsGame[i].overrideSprite = iconsAsset[random];
                iconsAssetReminder.Add(iconsAsset[random]);
                iconsAsset.Remove(iconsAsset[random]);

            }



            for (int i = 0; i < 3; i++)
            {
                int randomSelected = Random.Range(0, iconsAssetReminder.Count);
                if (iconsSelected.Count != 0)
                {
                    for (int j = 0; j < iconsSelected.Count; j++)
                    {
                        if (iconsAssetReminder[randomSelected] == iconsSelected[j])
                        {
                            i--;
                            break;
                        }
                        else
                        {
                            if (j == iconsSelected.Count - 1)
                            {
                                iconsSelected.Add(iconsAssetReminder[randomSelected]);
                                indexes.Add(randomSelected);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    iconsSelected.Add(iconsAssetReminder[randomSelected]);
                    indexes.Add(randomSelected);
                }


            }
            for (int i = 0; i < iconsAssetReminder.Count; i++)
            {
                iconsAsset.Add(iconsAssetReminder[i]);
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                transmitterManager.SendIntToSymbolManagerToOther(indexes[i]);
            }

        }

        public void ChargeSymbols(List<int> IDs)
        {
            iconsSelected.Clear();
            for (int i = 0; i < iconsGame.Length; i++)
            {
                iconsGame[i].overrideSprite = iconsAsset[IDs[i]];
            }

            for (int i = 0; i < 3; i++)
            {
                iconsSelected.Add(iconsAssetReminder[IDs[i] + iconsGame.Length - 1]);
            }

            indexes.Clear();
        }

    }
}

