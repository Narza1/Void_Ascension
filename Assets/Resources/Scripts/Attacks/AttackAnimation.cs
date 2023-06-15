using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class AttackAnimation : StateMachineBehaviour
{

    private GameObject caster;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        caster = animator.transform.parent.gameObject;
        
        if (caster.name.Equals("Player"))
        {
           

            var player = caster.GetComponent<AvatarController>();
            player.isAttacking = true;
            switch (animator.GetInteger("attack"))
            {

                case 1:
                    player.currentStamina -= 10;
                    break;

                case 2:
                    player.currentStamina -= 15;
                    break;
            }
        }
        else
        {
            var revenant = caster.GetComponent<RevenantController>();
            revenant.isAttacking = true;
            switch (animator.GetInteger("attack"))
            {

                case 1:
                    //revenant.currentStamina -= 10;
                    break;

                case 2:
                    //revenant.currentStamina -= 15;
                    break;
            }
        }

        
        animator.SetInteger("attack", -1);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (caster.name.Equals("Player"))
            caster.GetComponent<AvatarController>().isAttacking = false;
        else
            caster.GetComponent<RevenantController>().isAttacking = false;

    }

}
