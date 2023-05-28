using System;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class AttackAnimation : StateMachineBehaviour
{



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvatarController.isAttacking = true;

        switch (animator.GetInteger("attack"))
        {

            case 1:

                break;

            case 2:
                break;




        }
        animator.SetInteger("attack", -1);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvatarController.isAttacking = false;
    }

}
