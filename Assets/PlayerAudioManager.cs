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
        [SerializeField] [FoldoutGroup("Player")] float volume = 0.5f;
        [SerializeField] [FoldoutGroup("Player Gun")] AudioClip gunshotClip;
        [SerializeField] [FoldoutGroup("Player Gun")] AudioClip[] ricochetClips;
        AudioClip ricochetClip, lastRicochetClip; // to avoid repeating the sound
        int attempts = 3; // 1-out-of-3 chance to replay the same sound 

        private void Awake()
        {
            playerAudioSource = FindObjectOfType<VR_TeleportBehaviour>().gameObject.GetComponent<AudioSource>();
        }

        public void GE_GunshotSFX()
        {
            playerAudioSource.PlayOneShot(gunshotClip, volume);
        }

        public void GE_GunshotRicochetSFX()
        {
            int index = Random.Range(0, ricochetClips.Length);
            lastRicochetClip = ricochetClips[index];
            playerAudioSource.PlayOneShot(RandomRicochet(), volume);
        }

        AudioClip RandomRicochet()
        {
            ricochetClip = ricochetClips[Random.Range(0, ricochetClips.Length)];

            while (ricochetClip == lastRicochetClip && attempts > 0)
            {
                ricochetClip = ricochetClips[Random.Range(0, ricochetClips.Length)];
                attempts--;
            }

            lastRicochetClip = ricochetClip;
            return ricochetClip;
        }
    }
}