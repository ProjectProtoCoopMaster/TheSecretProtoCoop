using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
namespace Gameplay.Mobile
{
    public class SymbolBehavior : MonoBehaviour
    {
        [SerializeField] private List<Sprite> iconsSelected;
        [SerializeField] private Image[] iconsAnswers;
        [SerializeField] private Image[] results;
        private int missNumber;
        [Header("---IMPORTANT---")]
        [SerializeField] private DoorBehavior door;

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
            for (int i = 0; i < iconsSelected.Count; i++)
            {
                if(iconsSelected[i] != iconsAnswers[i].overrideSprite)
                {
                    Miss();
                    i = iconsSelected.Count;
                    break;
                }
                else
                {
                    if (i == iconsSelected.Count - 1)
                    {
                        Succeed();
                        break;
                    }
                }
            }
        }

        private void Succeed()
        {

            door.Unlock();
            results[missNumber].color = Color.green;
            Destroy(gameObject);
        }

        private void Miss()
        {
            missNumber++;
            results[missNumber - 1].color = Color.red;
        }


    }

}
