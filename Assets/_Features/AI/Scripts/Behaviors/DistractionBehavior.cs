using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class DistractionBehavior : AgentBehavior
    {
        [SerializeField] private MoveAction moveBehavior;
        [SerializeField] private WaitAction waitBehavior;
        [SerializeField] private WatchAction watchBehavior;

        public Vector3 distractionPosition { get; private set; }

        private Vector3 returnPosition;
        private Vector3 returnRotation;

        [SerializeField] private float awarenessTime;
        [SerializeField] private float searchTime;

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

            actions = new List<_Action>
            {
                new _Action { actionType = ActionType.Wait, timeToWait = awarenessTime },
                new _Action { actionType = ActionType.Move, destination = distractionPosition, area = 0.8f },

                new _Action { actionType = ActionType.Wait, timeToWait = searchTime }
            };
        }

        public void SetDistractionWithReturn(Vector3 direction)
        {
            SetDistraction(direction);

            returnPosition = transform.position;
            returnRotation = transform.rotation.eulerAngles;

            actions.Add(new _Action { actionType = ActionType.Move, destination = returnPosition, area = 0.1f });
            actions.Add(new _Action { actionType = ActionType.Search, watchRotation = returnRotation });
        }
    }
}