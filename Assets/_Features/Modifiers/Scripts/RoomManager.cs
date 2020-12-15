using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public abstract class Modifier : MonoBehaviour
    {
        public bool active { get; set; }
        public bool check { get; protected set; }

        public virtual void Init() => active = true;

        public virtual void End() => active = false;
    }

    public enum ModifierTarget { VR, Mobile, VR_Mobile }

    public class RoomManager : MonoBehaviour
    {
        public bool useLevelGeneration;

        [ShowIf("useLevelGeneration")] public Transform entranceAnchor;
        [ShowIf("useLevelGeneration")] public Transform exitAnchor;

        public ModifierType roomModifier = ModifierType.None;

        public ModifierTarget modifierTarget;

        [HideIf("modifierTarget", ModifierTarget.Mobile)]
        public Modifier VRModifier;
        [HideIf("modifierTarget", ModifierTarget.VR)]
        public GameEvent initMobileModifier;

        void Start()
        {
            OnEnterRoom();
        }

        public void OnEnterRoom()
        {
            if (modifierTarget == ModifierTarget.VR) InitVRModifier();

            else if (modifierTarget == ModifierTarget.Mobile) InitMobileModifier();

            else
            {
                InitVRModifier();
                
                InitMobileModifier();
            }
        }

        private void InitVRModifier() => VRModifier.Init();

        private void InitMobileModifier() => initMobileModifier.Raise();
    } 
}
