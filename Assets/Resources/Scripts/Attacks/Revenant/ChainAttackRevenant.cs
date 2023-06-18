using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAttackRevenant : StateMachineBehaviour
{
  
    private int attack;
    [SerializeField]
    private bool chainAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {



        attack = animator.GetInteger("attack");
        animator.SetInteger("attack", -1);
        AttackSelector(attack);
    }


    private void AttackSelector(int state)
    {
        var startingPoint = GameObject.Find("pointR");
        GameObject prefab = Resources.Load<GameObject>("Prefabs/BlueFlamethrowerRevenant");
        switch (state)
        {

            case 2:
                //GameObject prefab = Resources.Load<GameObject>("Prefabs/FireBall");

                Destroy(Shoot(startingPoint, prefab), 1);

                break;
            case 1:
                //GameObject prefab = Resources.Load<GameObject>("Prefabs/FireBall");
                prefab = Resources.Load<GameObject>("Prefabs/FireBallRevenant");
                Destroy(Shoot(startingPoint, prefab), 5);
                break;

        }

    }

    private GameObject Shoot(GameObject wand, GameObject prefab)
    {

        // Obtener la posición del ratón en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del ratón en la pantalla a una posición en el mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

        // Obtener la posición del objeto en el mundo
        Vector3 objectPosition = wand.transform.position;

        // Calcular la dirección hacia la posición del ratón
        Vector3 direction = worldMousePosition - objectPosition;
        Vector3 position = new Vector3(wand.transform.position.x, wand.transform.position.y, wand.transform.position.z);
        var fle = Instantiate(prefab, position, wand.transform.rotation);
        // Calcular el ángulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;
        Vector3 newRotation = new Vector3(wand.transform.localEulerAngles.x, wand.transform.localEulerAngles.z, angle);
        fle.transform.localEulerAngles = newRotation;
        // Instanciar el objeto con la misma posición y rotación que el GameObject existente
        return fle;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    private float _elapsedTime = 0.0f;
    private readonly float _interval = 0.36f;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (chainAttack)
        {
            // Incrementar el tiempo transcurrido desde la última actualización
            _elapsedTime += Time.deltaTime;

            // Verificar si ha pasado suficiente tiempo desde la última actualización
            if (_elapsedTime >= _interval)
            {
                // Realizar la tarea aquí


                AttackSelector(attack);

                // Reiniciar el tiempo transcurrido
                _elapsedTime = 0.0f;
            }
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
