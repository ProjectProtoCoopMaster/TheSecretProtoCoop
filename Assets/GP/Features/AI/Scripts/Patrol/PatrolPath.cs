using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay.AI
{
    // Compilation order is significant when a script references a class compiled in a different phase (and therefore located in a different assembly).
    // The basic rule is that anything that is compiled in a phase after the current one cannot be referenced.
    // Anything that is compiled in the current phase or an earlier phase is fully available.

    // Assembly C# Editor is compiled after Assembly C#.
    // Therefore I can't use a class from Editor into my Runtime scripts.
    // Fuck.

    //#if UNITY_EDITOR
    //public class PatrolPathContextMenu
    //{
    //    [ContextMenu("Open Patrol Path Editor")]
    //    public void OpenEditorWindow()
    //    {
    //        PatrolPathEditorWindow.Open((GameDataObject)target);
    //    }
    //}
    //#endif

    public class PatrolPath : MonoBehaviour
    {
        public Color pathColor;

        public List<Waypoint> waypoints = new List<Waypoint>();

        public float pointArea { get; set; } = 0.5f;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (Waypoint point in waypoints)
            {
                if (point != null)
                {
                    Vector3 center = new Vector3(point.position.x, 0.0f, point.position.z);
                    Vector3 size = new Vector3(pointArea, 0.0f, pointArea);
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }
    } 
}
