using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#region Utility
public class Utility
{
    public static bool SafeCheck<T>(T element, string message)
    {
        if (element == null || element.Equals(null))
        {
            Debug.Log(message);
            return false;
        }
        else return true;
    }

    public static bool SafeCheck<T>(List<T> list, string message)
    {
        if (list.Count == 0)
        {
            Debug.Log(message);
            return false;
        }
        else return true;
    }
}
#endregion

namespace Gameplay.AI
{
    public enum ActionType { Move, Wait, Watch, None, Search }

    [System.Serializable]
    public class _Action
    {
        public ActionType actionType;

        [ShowIf("type", ActionType.Move)]
        public Vector3 destination;
        public float area;

        [ShowIf("type", ActionType.Wait)]
        public float timeToWait;

        [ShowIf("type", ActionType.Watch)]
        public Vector3 watchDirection;

        [ShowIf("type", ActionType.Search)]
        public Vector3 watchRotation;
    }

    public abstract class ActionBehavior : MonoBehaviour
    {
        public ActionType actionType { get; protected set; } // Not used

        public abstract void StartActionBehavior(_Action action);
        public virtual void ResumeActionBehavior(_Action action) { StartActionBehavior(action); }

        public abstract void StopActionBehavior();

        public abstract bool Check();
    }

    public abstract class AgentBehavior : MonoBehaviour
    {
        public StateType stateType { get; protected set; } // Not used

        [SerializeField] public bool loop;

        public bool active { get; private set; }

        protected Dictionary<ActionType, ActionBehavior> actionBehaviors = new Dictionary<ActionType, ActionBehavior>();

        public List<_Action> actions { get; protected set; } = new List<_Action>();

        public _Action currentAction { get; protected set; }
        public ActionType currentActionType { get; protected set; }
        public ActionType savedActionType { get; protected set; }

        protected int actionIndex;

        public AgentManager agent { get; internal set; }

        #region Get
        void Awake()
        {
            InitializeBehavior();
        }

        protected abstract void InitializeBehavior();
        #endregion

        #region Set
        public void Begin()
        {
            Debug.Log("Beginning behavior : " + GetType() + " for " + agent.name);

            active = true;
            actionIndex = 0;

            SetAction(actionIndex);
        }

        protected void SetAction(int index)
        {
            currentAction = actions[index];
            currentActionType = currentAction.actionType;

            SetActionBehavior(currentActionType);
        }

        protected void SetActionBehavior(ActionType actionType)
        {
            actionBehaviors[actionType].StartActionBehavior(currentAction);
        }
        #endregion

        #region Loop
        public virtual void Update()
        {
            if (active)
            {
                if (actionBehaviors[currentActionType].Check())
                {
                    NextAction();
                }
            }
        }
        #endregion

        #region Next
        protected void NextAction()
        {
            actionIndex++;

            Debug.Log(actionIndex + " / " + actions.Count + " " + GetType() + " " + agent.name);
            if (actions.Count == actionIndex)
            {
                if (loop) Begin();

                else End();
            }

            else SetAction(actionIndex);
        }

        protected void End()
        {
            actionIndex = 0;
            active = false;

            agent.SwitchAgentState();
        }
        #endregion

        #region Pause
        public void Stop()
        {
            active = false;
            savedActionType = currentActionType;

            foreach (ActionBehavior actionBehavior in actionBehaviors.Values)
            {
                actionBehavior.StopActionBehavior();
            }
        }

        public void Resume()
        {
            active = true;
            currentActionType = savedActionType;

            SetActionBehavior(currentActionType);
        }
        #endregion
    }
}
