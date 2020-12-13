using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="NewEntityVisionData", menuName = "EntityData/EntityVisionData", order = 1)]
public class EntityVisionScriptable : ScriptableObject
{
    public float rangeOfVision;
    public float coneOfVision;
    //public Transform playerHead;
}
