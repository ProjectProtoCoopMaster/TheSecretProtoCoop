using Gameplay.VR.Player;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Feedbacks
{
    public class PlayerAudioManager : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("Player")] AudioSource playerAudioSource;
        [SerializeField] [FoldoutGroup("Player")] AudioClip gunshotClip;
        public float volume = 0.5f;

        private void Awake()
        {
            playerAudioSource = FindObjectOfType<VR_TeleportBehaviour>().gameObject.GetComponent<AudioSource>();
        }

        public void GE_GunShotSFX()
        {
            playerAudioSource.PlayOneShot(gunshotClip, volume);
        }
    }
}