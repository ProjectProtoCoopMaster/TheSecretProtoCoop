using UnityEngine;

namespace Gameplay.VR
{
    public class DeathSoundBehaviour : SoundMaker
    {
        new void Awake()
        {
            base.Awake();
        }

        public void GE_CryOut()
        {
            MakeNoise();
        }
    } 
}
