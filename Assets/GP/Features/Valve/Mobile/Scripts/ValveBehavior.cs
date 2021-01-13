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

        private Vector3 mousePos;
        private float angle;
        private void Update()
        {

            if (isTouching)
            {
                if (timer >= 100) timer = 101;
                if (timer <= 0) timer = 0;

                if (timer >= 0 && timer <= 100)
                {
                    mousePos = Input.mousePosition;
                    mousePos.x = mousePos.x - rect.anchoredPosition.x;
                    mousePos.y = mousePos.y - rect.anchoredPosition.y;

                    angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                    
                    rect.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    if (angle < 0)
                    {
                        if(Input.GetTouch(0).deltaPosition.x > 0)
                        {
                            timer++;
                            
                        }
                        else
                        {
                            timer--;
                        }

                    }
                    else
                    {
                        if (Input.GetTouch(0).deltaPosition.x > 0)
                        {
                            timer--;
                        }
                        else
                        {
                            timer++;
                        }
                    }
                    

                }
                else 
                {
                
                }
            }

        }

        public void SetIsTouching(bool value) => isTouching = value;
    }

}
