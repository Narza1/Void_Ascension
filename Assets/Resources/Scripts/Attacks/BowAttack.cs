using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAttack : StateMachineBehaviour
{

    private bool shoot;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shoot = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.35f && !shoot)
        {
            //si me da por hacer que cargue manteniendo pulsado
            //animator.speed = (0);
            ShootArrow(animator.GetInteger("attack"));
            animator.SetInteger("attack", -1);
            shoot = true;

        }
        
    }
    
    private void ShootArrow(int attackType)
    {

        GameObject startingPoint = GameObject.Find("handSlotLeft");
        //GameObject prefab = Resources.Load<GameObject>("Prefabs/FireBall");
        GameObject prefab = Resources.Load<GameObject>("Prefabs/arrow_green");

        // Obtener la posición del ratón en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del ratón en la pantalla a una posición en el mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

        // Obtener la posición del objeto en el mundo
      

        // Calcular la dirección hacia la posición del ratón
        Vector3 direction = worldMousePosition - startingPoint.transform.position;
        Vector3 position = new Vector3(startingPoint.transform.position.x, startingPoint.transform.position.y,0.1f);

        var fle = Instantiate(prefab, position, startingPoint.transform.rotation);
        // Calcular el ángulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg +90;
        Vector3 newRotation = new Vector3(0, 0, angle);
        fle.transform.localEulerAngles = newRotation;

        if (attackType == 2)
        {

            //pruebas con 30
            var fle2 = Instantiate(prefab, position, startingPoint.transform.rotation);
            // Calcular el ángulo en grados
            fle2.transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 105));

            var fle3 = Instantiate(prefab, position, startingPoint.transform.rotation);
            // Calcular el ángulo en grados
            fle3.transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 75));
        }
        // Instanciar el objeto con la misma posición y rotación que el GameObject existente
        
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
