#if UNITY_STANDALONE
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR.Feedbacks
{
    public class UI_ReloadingBehaviour : MonoBehaviour
    {
        [SerializeField] Transform gunCanvasAnchor;
        [SerializeField] float incrementValue;
        [SerializeField] Canvas canvas;
        [SerializeField] FloatVariable shootingCooldown;
        [SerializeField] Slider shootingCooldownSlider;

        private void Update()
        {
           // canvas.transform.position = gunCanvasAnchor.position;

            if (shootingCooldownSlider.value <= shootingCooldownSlider.maxValue)
                shootingCooldownSlider.value += incrementValue * Time.unscaledDeltaTime;
        }

        public void GE_ReloadGun()
        {
            shootingCooldownSlider.value = 0;
        }

        public void GE_RefreshScene()
        {
            gunCanvasAnchor = GameObject.Find("[GunCanvasAnchor]").transform;
            canvas.transform.parent = gunCanvasAnchor;


            shootingCooldownSlider.maxValue = shootingCooldown.Value;
            incrementValue = shootingCooldownSlider.maxValue / shootingCooldown.Value;
        }

    }
}
#endif