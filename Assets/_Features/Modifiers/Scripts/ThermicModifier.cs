using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ThermicModifier : Modifier
    {
        public bool showPlayer { get; set; }
        public BoolVariable ShowPlayer;

        public override void Init()
        {
            active = true;
            showPlayer = false;
        }

        public override void End()
        {
            active = true;
            showPlayer = false;
        }
    } 
}
