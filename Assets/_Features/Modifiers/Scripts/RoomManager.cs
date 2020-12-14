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

        public abstract void Activate();

        public abstract void Deactivate();
    }

    public class RoomManager : MonoBehaviour
    {
        public Transform entranceAnchor;
        public Transform exitAnchor;

        public ModifierType roomModifier = ModifierType.None;

        public Modifier modifier;

        void Start()
        {
            StartRoom();
        }

        public void StartRoom()
        {
            modifier.Init();

            modifier.Activate();
        }

        public void OnExitRoom()
        {
            modifier.Deactivate();
        }
    } 
}
