using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class AttackAnimation : StateMachineBehaviour
{

    private GameObject caster;
    private GameObject[] hands,handsR;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        caster = animator.transform.parent.gameObject;
        
        if (caster.name.Equals("Player"))
        {
           

            var player = caster.GetComponent<AvatarController>();
            player.isAttacking = true;
             hands= GameObject.FindGameObjectsWithTag("Hand");
            hands[0].GetComponent<CircleCollider2D>().enabled = true;
            hands[1].GetComponent<CircleCollider2D>().enabled = true;
            
            switch (animator.GetInteger("attack"))
            {

                case 1:
                    player.currentStamina -= 10;
                    hands[0].GetComponent<Fists>().hitValue = 1;
                    hands[1].GetComponent<Fists>().hitValue = 1;
                    break;

                case 2:
                    player.currentStamina -= 15;
                    hands[0].GetComponent<Fists>().hitValue = 1.5f;
                    hands[1].GetComponent<Fists>().hitValue = 1.5f;

                    break;
            }
        }
        else
        {
            var revenant = caster.GetComponent<RevenantController>();
            revenant.isAttacking = true;
            
        }

        
        animator.SetInteger("attack", -1);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (caster.name.Equals("Player"))
        {
            hands[0].GetComponent<CircleCollider2D>().enabled = false;
            hands[1].GetComponent<CircleCollider2D>().enabled = false;
            caster.GetComponent<AvatarController>().isAttacking = false;

        }

        else
            caster.GetComponent<RevenantController>().isAttacking = false;

    }

}
