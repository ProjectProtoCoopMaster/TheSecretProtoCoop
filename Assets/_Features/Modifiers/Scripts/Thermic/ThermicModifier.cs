using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class ThermicModifier : Modifier
    {
        public bool showPlayer { get; set; } = true;

        public override void Init()
        {
            showPlayer = false;
            base.Init();
        }

        public override void End()
        {
            showPlayer = true;
            base.End();
        }
    } 
}
