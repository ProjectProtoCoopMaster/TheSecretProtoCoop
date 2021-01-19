using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Mobile
{
    public class OxygenMobileModifier : Modifier
    {
        [SerializeField] IntVariable _oxygenTimerVR;
        [SerializeField] IntVariable _oxygenTimerMobile;
        [SerializeField] Text oxygenText;

        private void Update()
        {
            if(_oxygenTimerVR.Value <= 100 && _oxygenTimerMobile.Value <=100)
            {
                oxygenText.text = _oxygenTimerVR.Value.ToString();
            }
        }

        private void OnDisable()
        {
            _oxygenTimerVR.Value = 0;
            _oxygenTimerMobile.Value = 0;
        }
    } 
}
