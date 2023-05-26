using System;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class AttackAnimation : StateMachineBehaviour
{
    private bool throwItem = false;
    private GameObject revenant,startingPoint;
    private AvatarController avatar;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public static Transform FindDeepChild(Transform parent, string name)
    {
        Transform result = parent.Find(name);
        if (result != null)
            return result;

        foreach (Transform child in parent)
        {
            result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }

        return null;
    }


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvatarController.isAttacking= true;


       // startingPoint = revenant.transform.Find("point").gameObject;
        Debug.Log("startingPoint");
        switch (animator.GetInteger("attack"))
        {
            case 0:
                var aux = animator.gameObject.transform.parent.gameObject;


                if (aux.name == "Player")
                {
                    avatar = aux.GetComponent<AvatarController>();
                    Debug.Log("111111111111111111");
                    AvatarController.isAttacking = true;

                }

                revenant = aux;
                startingPoint = FindDeepChild(revenant.transform, "point")?.gameObject;
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
        // Obtener la posici�n del rat�n en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posici�n del rat�n en la pantalla a una posici�n en el mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

        // Obtener la posici�n del objeto en el mundo
        Vector3 objectPosition = hand.transform.position;

        // Calcular la direcci�n hacia la posici�n del rat�n
        Vector3 direction = worldMousePosition - objectPosition;
        var fle = Instantiate(prefab, hand.transform.position, hand.transform.rotation);
        // Calcular el �ngulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;
        Vector3 newRotation = new Vector3(0, 0, angle);
        fle.transform.localEulerAngles = newRotation;
        // Instanciar el objeto con la misma posici�n y rotaci�n que el GameObject existente
        return fle;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject prefab;
        if (throwItem && stateInfo.normalizedTime > 0.5f) 
        {


            
            throwItem = false;
            if (avatar != null){
                var aux = AvatarController.set1 ? 2 : 5;

                
                
                //Debug.Log(GameController.GetItemByGuid(GameObject.Find("UserInterface").GetComponent<InventoryUIController>().SetSlots[2].ItemGuid).Icon.name);
                
                //prefab = Resources.Load<GameObject>($"Prefabs/{avatar.inventory.SetSlots[aux].ItemGuid}");
                prefab = Resources.Load<GameObject>($"Prefabs/{GameController.GetItemByGuid(GameObject.Find("UserInterface").GetComponent<InventoryUIController>().SetSlots[2].ItemGuid).Icon.name}");
            }
            else
            {
                prefab = Resources.Load<GameObject>($"Prefabs/{GameController.GetItemByGuid(revenant.GetComponent<RevenantController>().revenantData.accessoryGUID).Icon.name}");

            }
            if(prefab!= null) { 
            Shoot(startingPoint, prefab);
            }
            //GameObject prefab = Resources.Load<GameObject>("Prefabs/FireBall");

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
