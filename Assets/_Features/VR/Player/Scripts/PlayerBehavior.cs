using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerBehavior : MonoBehaviour, IKillable
    {
        [SerializeField] private CallableFunction _GameOver;
        [SerializeField] private CallableFunction _SendPlayerPosition;
        [SerializeField] private Vector3Variable _PlayerPosition;
        [SerializeField] private Camera pictureCamera;
        private bool isDead;
        public void Die()
        {
            if (!isDead)
            {
                _GameOver.Raise();
                isDead = true;
            }
        }

        private void Update()
        {
            _PlayerPosition.Value = pictureCamera.WorldToScreenPoint(new Vector3(transform.position.x, 0, transform.position.z));
            _SendPlayerPosition.Raise();
        }
    }
}

