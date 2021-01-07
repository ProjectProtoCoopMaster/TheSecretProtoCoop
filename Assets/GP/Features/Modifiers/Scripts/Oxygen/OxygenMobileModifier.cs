using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Mobile
{
    public class OxygenMobileModifier : Modifier
    {

        [SerializeField] FloatVariable _oxygenTimer;
        [SerializeField] Text oxygenText;

        private void Update()
        {
            oxygenText.text = ((int)_oxygenTimer.Value).ToString();
        }

    } 
}
