#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.VR.Feedbacks
{
    public class PlayerAudioManager : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("General")] AudioSource playerAudioSource;
        AudioSource _playerAudioSource
        {
            get
            {
                if (playerAudioSource == null) 
                    playerAudioSource = GameObject.Find(playerHead.Value).GetComponent<AudioSource>();
                return playerAudioSource;
            }
        }

        [SerializeField] [FoldoutGroup("General")] float volume = 0.5f;
        [SerializeField] [FoldoutGroup("General")] StringVariable playerHead;

        [SerializeField] [FoldoutGroup("Shooting")] AudioClip[] gunshotsMain;
        [SerializeField] [FoldoutGroup("Shooting")] AudioClip[] gunshotBacking;
        AudioClip lastGunshotClip; // to avoid repeating the sound
        [SerializeField] [FoldoutGroup("Shooting")] AudioClip[] casingClips;
        AudioClip lastCasingClip; // to avoid repeating the sound

        [SerializeField] [FoldoutGroup("Shooting")] AudioClip[] ricochetClips;
        AudioClip lastRicochetClip; // to avoid repeating the sound

        [SerializeField] [FoldoutGroup("Shooting")] AudioClip[] impactEnemyClips;
        AudioClip lastImpactClip; // to avoid repeating the sound

        [SerializeField] [FoldoutGroup("Teleportation")] AudioClip[] teleportationMain;
        [SerializeField] [FoldoutGroup("Teleportation")] AudioClip[] teleportationBacking;
        AudioClip lastTeleportationClip; // to avoid repeating the sound

        [SerializeField] [FoldoutGroup("GameOver")] AudioClip metalGearSolidDetection;
        [SerializeField] [FoldoutGroup("GameOver")] AudioClip gameOverAlarm;

        [SerializeField] [FoldoutGroup("User Interface")] AudioClip uIHoverClip;
        [SerializeField] [FoldoutGroup("User Interface")] AudioClip uIClickClip;

        [SerializeField] [FoldoutGroup("Elements")] AudioClip electrictyClip;

        int attempts = 3; // 1-out-of-3 chance to replay the same sound 

        public void GE_GunshotSFX()
        {
            // play two "layers" of SFX for the silenced gun
            _playerAudioSource.PlayOneShot(RandomClip(gunshotsMain, null), volume);
            _playerAudioSource.PlayOneShot(lastGunshotClip = RandomClip(gunshotBacking, lastGunshotClip), volume);
            _playerAudioSource.PlayOneShot(lastCasingClip = RandomClip(casingClips, lastCasingClip), volume);
        }

        public void GE_HitRicochetSFX()
        {
            _playerAudioSource.PlayOneShot(lastRicochetClip = RandomClip(ricochetClips, lastRicochetClip), volume);
        }

        public void GE_HitEnemySFX()
        {
            _playerAudioSource.PlayOneShot(lastImpactClip = RandomClip(impactEnemyClips, lastImpactClip), volume);
        }

        public void GE_TeleportationSFX()
        {
            _playerAudioSource.PlayOneShot(RandomClip(teleportationMain, null), volume);
            _playerAudioSource.PlayOneShot(lastTeleportationClip = RandomClip(teleportationBacking, lastTeleportationClip), volume);
        }

        public void GE_DetectionSFX()
        {
            _playerAudioSource.PlayOneShot(metalGearSolidDetection, volume);
        }

        public void GE_GameOverAlarmSFX()
        {
            _playerAudioSource.PlayOneShot(gameOverAlarm, volume);
        }

        public void GE_UIHoverSFX()
        {
            _playerAudioSource.PlayOneShot(uIHoverClip, volume);
        }

        public void GE_UIClickSFX()
        {
            _playerAudioSource.PlayOneShot(uIClickClip, volume);
        }

        AudioClip RandomClip(AudioClip[] clipArray, AudioClip previousClip)
        {
            AudioClip ricochetClip = clipArray[Random.Range(0, clipArray.Length)];

            while (ricochetClip == previousClip && attempts > 0)
            {
                ricochetClip = clipArray[Random.Range(0, clipArray.Length)];
                attempts--;
            }
            return ricochetClip;
        }

        void LayeredSounds()
        {

        }
    }
}
#endif