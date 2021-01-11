using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Gameplay.Mobile
{
    public class ValveBehavior : MonoBehaviour
    {
        private bool isTouching = false;
        [SerializeField] private RectTransform rect;
        public float delta;
        private float deltaSign;
        private float timer;
        private void Update()
        {
            if (timer >= 100) timer = 101;
            if (timer <= 0) timer = 0;

            if (isTouching)
            {
                if(timer >= 0 && timer <= 100)
                {
                    delta = Input.GetTouch(0).deltaPosition.x;
                    if (delta > 0) deltaSign = 1;
                    else deltaSign = -1;

                    //if (delta == 0 && deltaSign == 1) deltaSign = 1;
                    //else deltaSign = -1;
                    if (Input.GetTouch(0).position.y > rect.anchoredPosition.y)
                    {
                        if (deltaSign < 0)
                        {
                           
                            
                        }
                        else
                        {

                            rect.Rotate(-transform.forward * 10f);
                            timer++;
                        }

                    }
                    else
                    {
                        if (deltaSign < 0)
                        {
                            rect.Rotate(-transform.forward * 10f);
                            timer++;
                        }
                        else
                        {
                            if (timer > 0)
                            {
                                rect.Rotate(transform.forward * 10f);
                                timer--;

                            }
                        }
                    }

                }
                else deltaSign = 0;
            }
                
        }

        public void SetIsTouching(bool value) => isTouching = value;
    }

}
