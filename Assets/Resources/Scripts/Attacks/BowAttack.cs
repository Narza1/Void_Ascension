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

        var player = GameObject.Find("Player").GetComponent<AvatarController>();
        var aux = AvatarController.set1 ? 1 : 4;
        var item = player.inventory.SetSlots[aux];
        if (item.ItemGuid != "")
        {
            var arrow = GameController.GetItemByGuid(item.ItemGuid).Icon.name;
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{arrow}");
            item.UseItem();



            // Obtener la posici�n del rat�n en la pantalla
            Vector3 mousePosition = Input.mousePosition;

            // Convertir la posici�n del rat�n en la pantalla a una posici�n en el mundo
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

            // Obtener la posici�n del objeto en el mundo


            // Calcular la direcci�n hacia la posici�n del rat�n
            Vector3 direction = worldMousePosition - startingPoint.transform.position;
            Vector3 position = new Vector3(startingPoint.transform.position.x, startingPoint.transform.position.y, 0.1f);

            var fle = Instantiate(prefab, position, startingPoint.transform.rotation);
            // Calcular el �ngulo en grados
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            Vector3 newRotation = new Vector3(0, 0, angle);
            fle.transform.localEulerAngles = newRotation;

            if (attackType == 2)
            {

                //pruebas con 30
                var fle2 = Instantiate(prefab, position, startingPoint.transform.rotation);
                // Calcular el �ngulo en grados
                fle2.transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 105));

                var fle3 = Instantiate(prefab, position, startingPoint.transform.rotation);
                // Calcular el �ngulo en grados
                fle3.transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 75));
            }



        }

    }
}
