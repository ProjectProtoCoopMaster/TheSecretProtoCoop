using Gameplay.VR.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR.Feedbacks
{
    public class PlayerAudioManager : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("Player")] AudioSource playerAudioSource;
        [SerializeField] [FoldoutGroup("Player")] float volume = 0.5f;
        [SerializeField] [FoldoutGroup("Player Gun")] AudioClip gunshotClip;
        [SerializeField] [FoldoutGroup("Player Gun")] AudioClip[] ricochetClips;
        AudioClip lastRicochetClip; // to avoid repeating the sound
        int attempts = 3; // 1-out-of-3 chance to replay the same sound 
        [SerializeField] [FoldoutGroup("Player Gun")] AudioClip[] hitEnvironmentClips;
        AudioClip lastEnvironmentClip;

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
            playerAudioSource.PlayOneShot(lastRicochetClip = RandomRicochet(ricochetClips, lastRicochetClip), volume);
        }

        public void GE_HitEnvironementSFX()
        {
            playerAudioSource.PlayOneShot(lastRicochetClip = RandomRicochet(ricochetClips, lastRicochetClip), volume);
            playerAudioSource.PlayOneShot(lastEnvironmentClip = RandomRicochet(hitEnvironmentClips, lastEnvironmentClip), volume);
        }

        AudioClip RandomRicochet(AudioClip[] clipArray, AudioClip previousClip)
        {
            AudioClip ricochetClip = clipArray[Random.Range(0, clipArray.Length)];

            while (ricochetClip == previousClip && attempts > 0)
            {
                ricochetClip = clipArray[Random.Range(0, clipArray.Length)];
                attempts--;
            }
            return ricochetClip;
        }
    }
}