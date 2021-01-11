using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class BonePositionTracker : MonoBehaviour
    {
        public Vector3Variable boneToTrack;

        int framesPassed;
        int frequency = 4;

        private void Update()
        {
            framesPassed++;
            if (framesPassed % frequency == 0)
            {
                boneToTrack.Value = transform.position;
                framesPassed = 0;
            }
        }
    }
}
