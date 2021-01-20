using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Feedbacks
{
    public class ActivateGlow : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] meshRenderers;
        [SerializeField] private SkinnedMeshRenderer skinnedMesh;
        [SerializeField] private Material glowEffect;

        public void UE_ActivateGow()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].materials[1] = glowEffect;
            skinnedMesh.material = glowEffect;
        }
    } 
}
