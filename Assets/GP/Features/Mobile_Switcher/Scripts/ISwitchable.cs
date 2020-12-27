using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface ISwitchable
    {
        GameObject MyGameObject { get; set; }
        int State { get; set; }

        int Power { get; set; }

        void TurnOn();

        void TurnOff();
    }
}

