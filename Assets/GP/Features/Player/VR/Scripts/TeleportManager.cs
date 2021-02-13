#if UNITY_STANDALONE
using System.Collections;
using UnityEngine;
using Valve.VR;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Gameplay.VR.Player
{
    public class TeleportManager : MonoBehaviour
    {
        [Tooltip("The duration of Teleportation. Expressed in seconds.")]
        [SerializeField] [FoldoutGroup("Teleportation Transition")] float tweenDuration = .25f;
        [Tooltip("The Teleportation's movement curve.")]
        [SerializeField] [FoldoutGroup("Teleportation Transition")] TweenFunctions tweenFunction;
        [Tooltip("The Particle Effect to play when the player teleports.")]
        [SerializeField] [FoldoutGroup("Teleportation Transition")] ParticleSystem dashEffect;
        [Tooltip("The GameEvent that is called when the player presses down on the teleport input.")]
        [SerializeField] [FoldoutGroup("Teleportation Transition")] GameEvent teleportAiming;
        [Tooltip("The GameEvent that is called when the player releases the teleport input.")]
        [SerializeField] [FoldoutGroup("Teleportation Transition")] GameEvent teleported;
        [Tooltip("The GameEvent that is called when the player is teleporting.")]
        [SerializeField] [FoldoutGroup("Teleportation Transition")] GameEvent teleportDashing;
        TweenManagerLibrary.TweenFunction delegateTween;
        Vector3 startPos, targetPos, movingPosition, change;
        float time;

        [Tooltip("The elements to Track and Move.")] [SerializeField] Transform playerRig, playerHead;
        SteamVR_Behaviour_Pose controllerPose;
        private bool showRayPointer = false;
        [Tooltip("The Layers with which the Teleportation laser will intersect.")]
        [SerializeField] [FoldoutGroup("Teleportation Pointer")] LayerMask teleportationLayers;
        [Tooltip("The Color of the laser when the player can teleport.")]
        [SerializeField] [FoldoutGroup("Teleportation Pointer")] Gradient validTeleport;
        [Tooltip("The Color of the laser when the player cannot teleport.")]
        [SerializeField] [FoldoutGroup("Teleportation Pointer")] Gradient invalidTeleport;
        [Tooltip("The Width of the player's laser.")]
        [SerializeField] [FoldoutGroup("Teleportation Pointer")] float lineWidth;
        LineRenderer bezierVisualization;
        internal Transform pointerOrigin;
        GameObject pointer;
        Ray horizontalRay, tallRay;
        RaycastHit hitTallInfo, hitHorizontalInfo;
        Vector3 posContainer;
        Vector3 p0, p1, p2;
        float t;

        [Tooltip("The maximum teleportation distance. Expressed in standard Unity units.")]
        [SerializeField] [FoldoutGroup("Internal Values")] float maxDistance = 10f;
        [Tooltip("The Height from which the laser will be cast downwards.")]
        [SerializeField] [FoldoutGroup("Internal Values")] float castingHeight = 2f;
        [Tooltip("The minimum and maximum valid angles for aiming the player laser.")]
        [SerializeField] [FoldoutGroup("Internal Values")] float minControllerAngle = 30f, maxControllerAngle = 150f;
        [Tooltip("Define how smooth the laser is. A Higher values incurs a higher performance cost.")]
        [SerializeField] [FoldoutGroup("Internal Values")] int bezierSmoothness;

        internal bool isGameOver;
        bool canTeleport;
        bool VRPlatform;

        [SerializeField] VisualEffect targetArea, oldArea;

        VisualEffect _oldArea
        {
            get
            {
                if (oldArea == null)
                {

                    Collider[] hitColliders = Physics.OverlapBox(playerRig.position, transform.localScale, Quaternion.identity);
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        //Debug.Log(hitColliders[i].name);

                        if (hitColliders[i].GetComponentInChildren<VisualEffect>() != null)
                            oldArea = hitColliders[i].GetComponentInChildren<VisualEffect>();
                    }
                    return oldArea;
                }

                else return oldArea;
            }

            set
            {
                oldArea = value;
            }
        }

        private void Awake()
        {
            bezierVisualization = GetComponentInChildren<LineRenderer>();
        }

        private void Start()
        {
            pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointer.GetComponent<Collider>().enabled = false;
            pointer.GetComponent<Renderer>().enabled = false;

            bezierVisualization.startWidth = lineWidth;
            bezierVisualization.endWidth = lineWidth;
            bezierVisualization.useWorldSpace = true;
            bezierVisualization.positionCount = bezierSmoothness;

            delegateTween = TweenManagerLibrary.GetTweenFunction((int)tweenFunction);
        }

        private void FixedUpdate()
        {
            if (showRayPointer == true && isGameOver == false)
                ShowRayPointer();
        }

        #region Tall Ray Variables
        Vector3 controllerForward
        {
            get
            {
                if (VRPlatform) return controllerPose.transform.forward;
                else return playerHead.transform.forward;
            }
        }

        public float horizontalDistance
        {
            get
            {
                float controllerAngle = Vector3.Angle(Vector3.up * -1.0f, controllerForward);
                float clampedAngle = Mathf.Clamp(controllerAngle, minControllerAngle, maxControllerAngle);
                float angleRange = maxControllerAngle - minControllerAngle;
                float pitch = (clampedAngle - minControllerAngle) / angleRange; // Normalized pitch within range
                return maxDistance * pitch;
            }
        }

        // project a Ray on a plane, determined by the angle of the controller's forward
        Vector3 horizontalDirection
        {
            get
            {
                return Vector3.ProjectOnPlane(controllerForward, Vector3.up);
            }
        }

        // you must take into account the player's distance from the center
        Vector3 difference
        {
            get
            {
                return playerRig.position - playerHead.position;
            }
        }

        Vector3 teleportTarget
        {
            get
            {
                return pointer.transform.position + difference;
            }
        }

        // player position in world coords
        Vector3 playerPosition
        {
            get
            {
                return playerHead.position;
            }
        }

        // determine where along the ray the max distance point is
        Vector3 pointAlongRay
        {
            get
            {
                return horizontalRay.origin + horizontalDirection * horizontalDistance;
            }
        }

        // determine the tallRay's casting position
        Vector3 castingPosition
        {
            get
            {
                return playerHead.position + Vector3.up * castingHeight;
            }
        }
        #endregion

        public void GE_OnGameOver()
        {
            isGameOver = true; // go to teleportation jail
        }

        // 1. fire a ray pointing in the direction of the controller
        // 2. find a point on the horizontalRay based on the normalized pitch (defined in a range, eg. 60° to 120°) of the controller * maxDistance
        // 3. fire a raycast from above the player. Substract the point on the horiz ray from the new ray's position and normalize the result to get the direction
        // 4. if anything is hit by the second ray, it is the endpoint, otherwise, the end point will be somewhere on the horizontal axis.
        // 5. render a bézier curve between the controller and the end point

        // show the pointer, using the referenced controller's transform.forward
        internal void TallRayPointer(SteamVR_Behaviour_Pose _controllerPose)
        {
            if (_controllerPose != null && VRPlatform == false)
                VRPlatform = true;

            controllerPose = _controllerPose;
            showRayPointer = true;

            teleportAiming.Raise();
        }

        void ShowRayPointer()
        {
            // assign the Ray values
            if (VRPlatform) horizontalRay.origin = controllerPose.transform.position;
            else horizontalRay.origin = pointerOrigin.position;

            horizontalRay.direction = horizontalDirection;

            tallRay.origin = castingPosition;
            tallRay.direction = (pointAlongRay - castingPosition).normalized;

            // if you hit something with the Tall Ray, define it as the endpoint
            if (Physics.Raycast(tallRay, out hitTallInfo, 500, teleportationLayers))
            {
                if (hitTallInfo.collider.gameObject.layer == LayerMask.NameToLayer("TeleportAreas"))
                {
                    targetArea = hitTallInfo.collider.GetComponentInChildren<VisualEffect>();
                    canTeleport = true;
                }

                else canTeleport = false;

                pointer.transform.position = hitTallInfo.point;
            }

            // otherwise, the endpoint must be on the horizontal axis
            else if (Physics.Raycast(horizontalRay, out hitHorizontalInfo, 500, teleportationLayers))
            {
                pointer.transform.position = hitHorizontalInfo.point;
            }

            Debug.DrawRay(horizontalRay.origin, horizontalRay.direction * 20f, Color.blue); // draw the initial horizontal Ray
            Debug.DrawLine(horizontalRay.origin, pointAlongRay, Color.white); // draw the teleportation distance limit
            Debug.DrawRay(tallRay.origin, tallRay.direction * 50f, Color.red); // draw the Tall Ray shooting down

            #region Bézier Curve
            bezierVisualization.enabled = true;

            p1 = p2 = pointer.transform.position;

            if (VRPlatform)
            {
                p0 = controllerPose.transform.position;
                p1.y = controllerPose.transform.position.y;
            }

            else
            {
                p0 = pointerOrigin.transform.position;
                p1.y = pointerOrigin.position.y;
            }

            for (int i = 0; i < bezierSmoothness; i++)
            {
                t = i / (bezierSmoothness - 1.0f);
                posContainer = (1.0f - t) * (1.0f - t) * p0
                + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
                bezierVisualization.SetPosition(i, posContainer);
            }

            if (canTeleport == true) bezierVisualization.colorGradient = validTeleport;
            else if (canTeleport == false) bezierVisualization.colorGradient = invalidTeleport;
            #endregion                       
        }

        public void TryTeleporting()
        {
            teleported.Raise();

            if (canTeleport == true)
                StartCoroutine(TeleportThePlayer());
            bezierVisualization.enabled = showRayPointer = canTeleport = false;
        }

        IEnumerator TeleportThePlayer()
        {
            _oldArea.enabled = true;
            _oldArea = targetArea;
            targetArea.enabled = false;

            startPos = playerPosition;
            targetPos = teleportTarget;

            showRayPointer = false;

            dashEffect.Play();

            teleportDashing.Raise();

            time = 0;
            change = targetPos - startPos;

            // don't change yPos
            movingPosition.y = startPos.y;

            while (time <= tweenDuration)
            {
                time += Time.deltaTime;
                movingPosition.x = delegateTween(time, startPos.x, change.x, tweenDuration);
                movingPosition.z = delegateTween(time, startPos.z, change.z, tweenDuration);
                movingPosition.y = playerRig.position.y;
                playerRig.position = movingPosition;
                this.transform.position = movingPosition;
                yield return null;
            }

            dashEffect.Stop();
        }

        public void GE_ResetGameOver()
        {
            isGameOver = false;
        }
    }
}
#endif