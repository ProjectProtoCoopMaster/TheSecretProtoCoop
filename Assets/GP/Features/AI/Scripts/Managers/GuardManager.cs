using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public enum GuardType { Patrol, Static }

    public class GuardManager : AgentManager
    {
        [Title("Guard")]
        [SerializeField] private GuardType guardType; public GuardType GuardType { get => guardType; }

        [ShowIf("guardType", GuardType.Patrol), SerializeField] private PatrolBehavior patrolBehavior;

        [SerializeField] private DistractionBehavior distractionBehavior;

        public AnimationManager animationManager;

        protected override void InitializeAgent()
        {
            agentBehaviors = new Dictionary<StateType, AgentBehavior>();

            agentBehaviors.Add(StateType.Distraction, distractionBehavior); distractionBehavior.agent = this;
            if (guardType == GuardType.Patrol) { agentBehaviors.Add(StateType.Patrol, patrolBehavior); patrolBehavior.agent = this; }
        }

        public override void StartAgent()
        {
            animationManager.SetIdleAnim();

            if (guardType == GuardType.Patrol)
            {
                if (Utility.SafeCheck(patrolBehavior.Path, "There is no path attached to " + this.gameObject.name + " the guard will not move !"))
                {
                    patrolBehavior.InitializePatrol();
                    SwitchAgentState(Usage.Start, StateType.Patrol);
                }
            }
        }

        public void DistractTo(Vector3 destination)
        {
            if (distractionBehavior.active == true)
            {
                if (Vector3.Distance(transform.position, destination) > Vector3.Distance(transform.position, distractionBehavior.distractionPosition))
                {
                    return;
                }
            }

            if (guardType == GuardType.Patrol && patrolBehavior.active == true && patrolBehavior.currentAction.actionType == ActionType.Move)
            {
                distractionBehavior.SetDistraction(destination);
            }
            else
            {
                distractionBehavior.SetDistractionWithReturn(destination);
            }

            SwitchAgentState(Usage.Start, StateType.Distraction, !distractionBehavior.active);
        }
    }
}
