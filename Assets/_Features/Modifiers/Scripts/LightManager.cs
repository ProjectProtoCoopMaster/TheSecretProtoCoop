using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LightManager : MonoBehaviour
    {
        public Color baseLightColor;

        public List<Light> lights;

        public void SetLights(bool enable)
        {
            foreach (Light light in lights) light.gameObject.SetActive(enable);
        }
    } 
}
