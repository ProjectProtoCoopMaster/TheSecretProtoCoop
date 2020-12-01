using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Gameplay.AI;

public class TransformDebug : MonoBehaviour
{
    [SerializeField] private bool usePathColor;
    [ShowIf("usePathColor"), SerializeField] private PatrolPath path;

    [HideIf("usePathColor"), SerializeField] private Color color;
    public Color32 Color { get { if (usePathColor) { return path.pathColor; } else { return color; } } }

    [SerializeField] private bool otherRadius;
    [ShowIf("otherRadius"), SerializeField] private SoundObject script;

    [HideIf("otherRadius"), SerializeField] private float radius;
    public float Radius { get { if (otherRadius) { return script.Radius; } else { return radius; } } }

    void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = new Color32(Color.r, Color.g, Color.b, 25);
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
