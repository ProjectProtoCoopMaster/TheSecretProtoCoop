using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class CameraFeedbackBehaviour : MonoBehaviour
    {
        [SerializeField] MeshRenderer lightMesh;
        [SerializeField] Material activeMat, disabledMat;

        public void GE_ActiveLightFeedback()
        {
            lightMesh.material = activeMat;
        }

        public void GE_DisabledLightFeedback()
        {
            lightMesh.material = disabledMat;
        }
    }
}
