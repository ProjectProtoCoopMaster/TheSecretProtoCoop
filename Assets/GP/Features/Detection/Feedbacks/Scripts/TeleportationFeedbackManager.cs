using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Gameplay.VR.Feedbacks
{
    public class TeleportationFeedbackManager : MonoBehaviour
    {
        public VisualEffectAsset teleportationAreaAsset, teleportationEffectAsset;
        public VisualEffect teleportationEffect;
        public List<VisualEffect> particles = new List<VisualEffect>();

        private void Awake()
        {
            GE_RefreshScene();
        }

        public void GE_TeleportationAreaVisualsOn()
        {
            for (int i = 0; i < particles.Count; i++)
                particles[i].Play();
        }

        public void GE_TeleportationAreaVisualsOff()
        {
            for (int i = 0; i < particles.Count; i++)
                particles[i].Stop();
        }

        public void GE_RefreshScene()
        {
            VisualEffect[] iteratorList = FindObjectsOfType<VisualEffect>();

            // sort through all VFX graphs in the scene
            for (int i = 0; i < iteratorList.Length; i++)
            {
                if (iteratorList[i].visualEffectAsset == teleportationAreaAsset)
                    particles.Add(iteratorList[i]);
                if (iteratorList[i].visualEffectAsset == teleportationEffectAsset) 
                    teleportationEffect = iteratorList[i];
            }

            for (int i = 0; i < particles.Count; i++)
                particles[i].Stop();

            teleportationEffect.Stop();
        }
    }
}
