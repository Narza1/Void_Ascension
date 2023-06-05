using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : StateMachineBehaviour
{

    private bool throwItem = false;
    private GameObject revenant, startingPoint;
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
        AvatarController.isAttacking = true;
        // Start is called before the first frame update
        // startingPoint = revenant.transform.Find("point").gameObject;

        var aux = animator.gameObject.transform.parent.gameObject;


        if (aux.name == "Player")
        {
            avatar = aux.GetComponent<AvatarController>();
            AvatarController.isAttacking = true;

        }

        revenant = aux;
        startingPoint = FindDeepChild(revenant.transform, "point")?.gameObject;
        throwItem = true;
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

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (throwItem && stateInfo.normalizedTime > 0.5f)
        {



            throwItem = false;
            if (avatar != null)
            {
                var aux = AvatarController.set1 ? 2 : 5;
                //Debug.Log(GameController.GetItemByGuid(GameObject.Find("UserInterface").GetComponent<InventoryUIController>().SetSlots[2].ItemGuid).Icon.name);

                //prefab = Resources.Load<GameObject>($"Prefabs/{avatar.inventory.SetSlots[aux].ItemGuid}");
                var a = GameObject.Find("UserInterface").GetComponent<InventoryUIController>().SetSlots[2];
                if (a.ItemGuid != "")
                {
                    Shoot(startingPoint, Resources.Load<GameObject>($"Prefabs/{GameController.GetItemByGuid(a.ItemGuid).Icon.name}"));
                    a.UseItem();


                }
            }
            else
            {
                var b = revenant.GetComponent<RevenantController>().revenantData.consumableGUID;
                if (b != "")
                {
                    Shoot(startingPoint, Resources.Load<GameObject>($"Prefabs/{GameController.GetItemByGuid(b).Icon.name}"));


                }

            }


            //GameObject prefab = Resources.Load<GameObject>("Prefabs/FireBall");

        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvatarController.isAttacking = false;
    }
}
