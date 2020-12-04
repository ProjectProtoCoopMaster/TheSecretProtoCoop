using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class DodgeBehavior : AgentBehavior
    {
        [SerializeField] private MoveAction moveBehavior;

        private Vector3 direction;

        protected override void InitializeBehavior()
        {
            actionBehaviors = new Dictionary<ActionType, ActionBehavior>
            {
                { ActionType.Move, moveBehavior }
            };
        }

        private void SetDodge()
        {
            direction = transform.position + Vector3.forward;

            actions = new List<_Action>
            {
                new _Action { actionType = ActionType.Move, destination = direction, area = 0.1f }
            };
        }

        public override void Update()
        {
            base.Update();
        }

        private void CheckForTargets()
        {
            // Raycast
        }
    } 
}
