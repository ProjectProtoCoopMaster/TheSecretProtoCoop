#if UNITY_STANDALONE
using System.Collections;
using UnityEngine;

namespace Gameplay.VR.Player
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class TeleportationArea : MonoBehaviour
    {
        [SerializeField] Material teleportationMaterial;
        [SerializeField] float lerpDuration = .75f;
        float timeElapsed;
        float startValue = 0, endValue = 1;
        float valueToLerp;
        
        public void On()
        {
            startValue = 0;
            endValue = 1;

            timeElapsed = 0f;

            StartCoroutine(Lerp());
        }

        public void Off()
        {
            startValue = 1;
            endValue = 0;

            timeElapsed = 0f;

            StartCoroutine(Lerp());
        }

        IEnumerator Lerp()
        {
            while (timeElapsed < lerpDuration)
            {
                valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                teleportationMaterial.SetFloat("_GlowState", valueToLerp);
                yield return null;
            }

            valueToLerp = endValue;
            teleportationMaterial.SetFloat("_GlowState", valueToLerp);
        }
    } 
}
#endif
