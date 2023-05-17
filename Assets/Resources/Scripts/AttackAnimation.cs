using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttackAnimation : StateMachineBehaviour
{
    private bool throwItem = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvatarController.isAttacking= true;
        
        switch (animator.GetInteger("attack"))
        {
            case 0:

                throwItem = true;
                break;

            case 1:

                break;

            case 2:
                break;

            
                

    }
    animator.SetInteger("attack", -1);
    }

    private GameObject Shoot(GameObject hand, GameObject prefab)
    {
        // Obtener la posición del ratón en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del ratón en la pantalla a una posición en el mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

        // Obtener la posición del objeto en el mundo
        Vector3 objectPosition = hand.transform.position;

        // Calcular la dirección hacia la posición del ratón
        Vector3 direction = worldMousePosition - objectPosition;
        var fle = Instantiate(prefab, hand.transform.position, hand.transform.rotation);
        // Calcular el ángulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;
        Vector3 newRotation = new Vector3(0, 0, angle);
        fle.transform.localEulerAngles = newRotation;
        // Instanciar el objeto con la misma posición y rotación que el GameObject existente
        return fle;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (throwItem && stateInfo.normalizedTime > 0.5f) 
        {
            throwItem = false;
            GameObject startingPoint = GameObject.Find("point");
            //GameObject prefab = Resources.Load<GameObject>("Prefabs/FireBall");
            GameObject prefab = Resources.Load<GameObject>("Prefabs/dagger_common");
            Shoot(startingPoint, prefab);
            
        }

    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvatarController.isAttacking = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

}
