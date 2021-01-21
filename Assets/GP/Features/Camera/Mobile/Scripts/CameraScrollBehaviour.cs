using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Gameplay.Mobile
{
    public class CameraScrollBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        public Vector2 basePosition { get; set; }
        [SerializeField] private float speed;
        public Vector2 clampValue { get; set; }
        [SerializeField] private float clampHeightValue;
        private Touch touch;
        private float magnitudeReminder;
        private float magnitude;

        private void Update()
        {
            Move();
        }
        private void Move()
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    //transform.DOMove(transform.position + (new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y) * speed), .2f);
                    _transform.Translate(-touch.deltaPosition * speed);

                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (_transform.position.x > 30 + basePosition.x)
                    {
                        _transform.DOMoveX(clampValue.x, .5f);
                    }
                    else if (_transform.position.x < -30 + basePosition.x)
                    {
                        _transform.DOMoveX(-clampValue.x, .5f);
                    }

                    if (_transform.position.z > 30 + basePosition.y)
                    {
                        _transform.DOMoveZ(clampValue.y, .5f);
                    }
                    else if (_transform.position.z < -30 + basePosition.y)
                    {
                        _transform.DOMoveZ(-clampValue.y, .5f);

                    }
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                if (_transform.position.y > clampHeightValue)
                {
                    _transform.DOMoveY(clampHeightValue, .2f);
                }
                else if (_transform.position.y < 14)
                {
                    _transform.DOMoveY(14, .2f);
                }
                else
                {
                    //transform.Translate(transform.position.y + (difference * .07f));
                    _transform.DOMoveY(_transform.position.y + (-difference * .01f), 0);

                }


            }


        }
    }

}
