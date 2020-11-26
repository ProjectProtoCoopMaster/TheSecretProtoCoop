using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class DistractionBehavior : AgentBehavior
    {
        public MoveAction moveBehavior;
        public WaitAction waitBehavior;

        public WatchAction watchBehavior;

        private Vector3 distractionPosition;
        private Vector3 returnPosition;

        private Vector3 returnRotation;

        public float awarenessTime;
        public float searchTime;

        protected override void InitializeBehavior()
        {
            actionBehaviors = new Dictionary<ActionType, ActionBehavior>
            {
                { ActionType.Move, moveBehavior },
                { ActionType.Wait, waitBehavior },
                { ActionType.Search, watchBehavior }
            };
        }

        public void SetDistraction(Vector3 direction)
        {
            distractionPosition = direction;
            returnPosition = transform.position;

            returnRotation = transform.rotation.eulerAngles;

            actions = new List<_Action>
            {
                new _Action { actionType = ActionType.Wait, timeToWait = awarenessTime },
                new _Action { actionType = ActionType.Move, destination = distractionPosition, area = 0.8f },

                new _Action { actionType = ActionType.Wait, timeToWait = searchTime },
                new _Action { actionType = ActionType.Move, destination = returnPosition, area = 0.1f },
                new _Action { actionType = ActionType.Search, watchRotation = returnRotation }
            };
        }
    }
}