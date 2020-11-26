using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class PatrolBehavior : AgentBehavior
    {
        public PatrolPath path;

        public MoveAction moveBehavior;
        public WaitAction waitBehavior;
        public WatchAction watchBehavior;

        protected override void InitializeBehavior()
        {
            actionBehaviors = new Dictionary<ActionType, ActionBehavior>
            {
                { ActionType.Move, moveBehavior },
                { ActionType.Wait, waitBehavior },
                { ActionType.Watch, watchBehavior }
            };

            string msg = "There is no path attached to " + gameObject.name + " patrol behavior, the behavior did not initalize";
            if (Utility.SafeCheck(path, msg))
            {
                actions = ConvertPath(path);
            }
        }

        public List<_Action> ConvertPath(PatrolPath _path)
        {
            List<_Action> list = new List<_Action>();

            List<Waypoint> _waypoints = _path.waypoints;

            foreach (Waypoint _waypoint in _waypoints)
            {
                list.Add(new _Action { actionType = ActionType.Move, destination = _waypoint.position, area = 0.1f });

                _waypoint.InitializeWatch();

                List<Action> _actions = _waypoint.actions;

                foreach (Action _action in _actions)
                {
                    if (_action.type == _AType.Wait)
                    {
                        list.Add(new _Action { actionType = ActionType.Wait, timeToWait = _action.timeToWait });
                    }
                    else if (_action.type == _AType.Watching)
                    {
                        foreach (Vector3 actionDirection in _action.watchDirections)
                        {
                            list.Add(new _Action { actionType = ActionType.Watch, watchDirection = actionDirection });
                        }
                    }
                }
            }
            return list;
        }
    } 
}
