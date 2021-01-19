
using UnityEngine;

namespace Gameplay
{
    public interface IKillable
    {
        void Die(Vector3 force = default);

        void Die();
    }
}