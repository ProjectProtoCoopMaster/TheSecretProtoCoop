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

        private Renderer _renderer;
        private MaterialPropertyBlock _propBlock;

        void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
        }

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
                _renderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat("_GlowState", valueToLerp);
                _renderer.SetPropertyBlock(_propBlock);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            valueToLerp = endValue;
            _propBlock.SetFloat("_GlowState", valueToLerp);
            _renderer.SetPropertyBlock(_propBlock);
        }
    } 
}
#endif
