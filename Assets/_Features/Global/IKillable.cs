
using UnityEngine;

namespace Gameplay.VR
{
    public interface IKillable
    {
        void Die(Vector3 force = default);
        
    }
}