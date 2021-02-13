using Gameplay.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class AnimationManager : MonoBehaviour
    {
        public AgentManager agent;

        public Animator animator;

        public void SetIdleAnim()
        {
            animator.SetBool("isMoving", false);
        }

        public void SetMoveAnim()
        {
            animator.SetBool("isMoving", true);
        }

        public void SetAlertAnim()
        {
            animator.SetBool("isAlert", true);
        }
    }
}
