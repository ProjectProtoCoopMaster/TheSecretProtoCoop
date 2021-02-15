using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class BonePositionTracker : MonoBehaviour
    {
        public Vector3Variable boneToTrack;
        public BoolVariable _debuggingGame;

        int framesPassed;
        int frequency = 4;

        private void Awake()
        {
            if (_debuggingGame) GetComponentInChildren<Collider>().enabled = false;
        }

        private void Update()
        {
            framesPassed++;
            if (framesPassed % frequency == 0)
            {
                if (_debuggingGame.Value != false) boneToTrack.Value = transform.position;
                framesPassed = 0;
            }
        }
    }
}
