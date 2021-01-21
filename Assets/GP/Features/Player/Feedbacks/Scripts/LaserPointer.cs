using UnityEngine;

namespace Gameplay.VR.Player
{
    public class LaserPointer : MonoBehaviour
    {
        [SerializeField] protected float laserWidth;
        [SerializeField] protected bool showLaser = true;
        protected LineRenderer laserPointer;

        [SerializeField] protected Color laserColor;
        protected MaterialPropertyBlock baseColor;

        [SerializeField] protected LayerMask collisionMask;
        [SerializeField] public Transform laserStart, laserEnd;
        protected Transform laserHitPoint;
        protected RaycastHit hitInfo;

        [SerializeField] protected int updateFrequency;
        protected int framesPassed;

        protected void Awake()
        {
            laserPointer = GetComponent<LineRenderer>();

            baseColor = new MaterialPropertyBlock();
            baseColor.SetColor("_EmissionColor", laserColor);
        }

        private void OnEnable()
        {
            laserPointer.startWidth = laserPointer.endWidth = laserWidth;
            laserPointer.SetPropertyBlock(baseColor);
        }

        protected void FixedUpdate()
        {
            if (framesPassed % updateFrequency == 0)
            {
                if (Physics.Raycast(transform.position, laserEnd.position - transform.position, out hitInfo, collisionMask))
                {
                    if (laserHitPoint != null)
                        laserHitPoint.position = hitInfo.point;
                    else laserHitPoint = laserEnd;
                }
            }
        }

        private void LateUpdate()
        {
            if (laserHitPoint != null)
            {
                laserPointer.SetPosition(0, laserStart.position);
                laserPointer.SetPosition(1, laserHitPoint.position);
            }
            laserPointer.enabled = showLaser;
        }
    }
}
