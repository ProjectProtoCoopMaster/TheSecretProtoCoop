#if UNITY_STANDALONE
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class TeleportationArea : MonoBehaviour
    {

    } 
}
#endif
