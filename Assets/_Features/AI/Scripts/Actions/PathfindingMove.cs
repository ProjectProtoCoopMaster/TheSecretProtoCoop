using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gameplay;

namespace Gameplay.AI
{
    public class PathfindingMove : MoveAction
    {
        public NavMeshAgent navMeshAgent;

        public bool move { get; set; }

        public float angular;

        #region Set
        void Start()
        {
            destination = target.position;
        }

        public override void SetMove(Vector3 direction, bool _move)
        {
            base.SetMove(direction, _move);

            SetNavAgent(!_move);
            move = _move;

            SetNavDestination(destination);
        }

        public void SetNavAgent(bool locked)
        {
            if (locked) navMeshAgent.angularSpeed = 0.0f;

            else navMeshAgent.angularSpeed = angular;
        }

        private void SetNavDestination(Vector3 dest)
        {
            navMeshAgent.SetDestination(dest);
        }
        #endregion
    } 
}
