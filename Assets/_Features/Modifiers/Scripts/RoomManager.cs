using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public abstract class Modifier : MonoBehaviour
    {
        public bool active { get; set; }
        public bool check { get; protected set; }

        public abstract void Init();

        public abstract void End();
    }

    public class RoomManager : MonoBehaviour
    {
        public Transform entranceAnchor;
        public Transform exitAnchor;

        public ModifierType roomModifier = ModifierType.None;

        public Modifier modifier;

        void Start()
        {
            OnEnterRoom();
        }

        public void OnEnterRoom()
        {
            modifier.Init();
        }

        public void OnExitRoom()
        {
            modifier.End();
        }
    } 
}
