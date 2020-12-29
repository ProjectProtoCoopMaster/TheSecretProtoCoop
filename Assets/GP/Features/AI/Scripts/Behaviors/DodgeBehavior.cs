using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class DodgeBehavior : AgentBehavior
    {
        [SerializeField] private MoveAction moveBehavior;

        private Vector3 direction;

        public float distance;
        public float lateralRange;

        public float reactionTime;
        private float currentTime;

        private bool hitAlready;

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
            RaycastHit hit;

            Vector3 leftDir = transform.TransformDirection(new Vector3(-lateralRange, 0, 1));
            Vector3 rightDir = transform.TransformDirection(new Vector3(lateralRange, 0, 1));

            Debug.DrawRay(transform.position, leftDir * distance, Color.yellow);
            Debug.DrawRay(transform.position, rightDir * distance, Color.yellow);

            int hits = 0;
            hits = Physics.Raycast(transform.position, leftDir, out hit, distance) ? hits++ : Physics.Raycast(transform.position, rightDir, out hit, distance) ? hits++ : hits ;

            bool singleHit = (Physics.Raycast(transform.position, leftDir, out hit, distance) || Physics.Raycast(transform.position, rightDir, out hit, distance)) && !hitAlready;
            bool twoHits = Physics.Raycast(transform.position, leftDir, out hit, distance) && Physics.Raycast(transform.position, rightDir, out hit, distance);

            if (singleHit && !hitAlready)
            {
                currentTime = reactionTime;
                hitAlready = true;
            }
            else if (hitAlready)
            {
                if (currentTime <= 0.0f)
                {
                    hitAlready = false;
                    SetDodge();
                }
            }
            if (twoHits)
            {
                hitAlready = false;
                return;
            }

            currentTime -= Time.deltaTime;
        }
    } 
}
