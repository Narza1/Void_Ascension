using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AvatarController : MonoBehaviour
{
    Animator animator;
    private float horizontal, vertical, startTime;
    private Rigidbody2D rb;
    public float speed = 3;
    public static bool set1 = true, isAttacking;
    private InventoryUIController inventory;
    private VisualElement m_Root;
    private List<Character> characters= new List<Character>();
    private int currentCharacter;
   
    void Start()
    {
        GameObject ui = GameObject.Find("UserInterface");
        m_Root = ui.GetComponent<UIDocument>().rootVisualElement.Q("Container");
        m_Root.style.display = DisplayStyle.None;
        inventory = ui.GetComponent<InventoryUIController>();

        // Obtiene una referencia al objeto secundario
        GameObject childObject = transform.Find("KayKit Animated Character").gameObject;

        // Obtiene una referencia al componente Animator del objeto secundario
        animator = childObject.GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
        LoadCharacters();
    }

    private void LoadCharacters()
    {
        //esto deberia ir dentro del gamemanager supongo
        //aqui iria si existe un fichero que lo cargue y si no crearlo de zero
        characters.Clear();
        characters.Add(new MinionCharacter());
        characters.Add(new MageCharacter());
        characters.Add(new WarriorCharacter());
        characters.Add(new ArcherCharacter());
        newCurrentCharacter = 0;//aqui en lugar de cero sera el valor que leamos del archvo
        ChangeCharacter(newCurrentCharacter);
    }
    private void ChangeCharacter(int newCurrentCharacter)
    {

    }
        // Update is called once per frame
        void Update()
    {

        if (m_Root.style.display == DisplayStyle.None || !isAttacking)
        {
            Attack();
            Run();
            DashPattern();
            ChangeSetPattern();
        }
        Inventory();

    }

    

    private void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (m_Root.style.display == DisplayStyle.None)
            {
                m_Root.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_Root.style.display = DisplayStyle.None;
            }

        }
    }
    private void ChangeSetPattern()
    {
        
        if ((Time.time - startTime >= 0.5f) && Input.mouseScrollDelta.y != 0)
        {

            animator.SetInteger("ammo", 0);
            animator.SetInteger("weaponType", 0);
            for (int i = 0; i < 6; i++)
            {
                SetEquip(inventory.SetSlots[i]);
                
                
            }
            startTime = Time.time;
            set1 = !set1;
        }
        
    }
    public void SetEquip(InventorySlot inventorySlot)
    {
        string guid = inventorySlot.ItemGuid;
        if (!guid.Equals(""))
        {




            ItemDetails slotItem = GameController.GetItemByGuid(guid);

            switch (slotItem.objectType)
            {
                case ObjectType.Equipment:
                    List<GameObject> equipmentList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();

                    foreach (var item in equipmentList)
                    {
                       
                        if (item.name == slotItem.Icon.name)
                        {
                           
                            item.SetActive(!item.activeSelf);
                            if (item.activeSelf)
                            {
                                switch (((EquipmentDetails)slotItem).equipmentType)
                                {
                                    case EquipmentType.Sword:
                                        animator.SetInteger("weaponType", 1);


                                        break;

                                    case EquipmentType.Staff:
                                        animator.SetInteger("weaponType", 2);
                                        break;

                                    case EquipmentType.Bow:
                                        animator.SetInteger("weaponType", 3);
                                        break;
                                }
                            }
                        }
                    }





                    break;

                case ObjectType.Accesory:
                    animator.SetInteger("ammo", slotItem.quantity);
                    List<GameObject> accesoryList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();
                    if (slotItem.Name.Contains("Shield"))
                    {

                        foreach (var item in accesoryList)
                        {
                            
                            if (item.name == slotItem.Icon.name)
                            {

                                item.SetActive(!item.activeSelf);
                                break;
                            }
                        }
                    }
                    else
                    {
                        int ammo = inventorySlot.quantity;
                        Debug.Log(ammo);
                        animator.SetInteger("ammo", ammo);
                    }
                    break;

                case ObjectType.Consumable:

                    if (slotItem.GetType() == typeof(Consumable))
                    {

                    }
                    else
                    {

                    }
                    break;

            }
        }
    }
    private void DashPattern()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {

            StartCoroutine(Dash());
        }
    }

    private readonly float dashSpeed = 8f;
    private readonly float dashDuration = 0.25f;
    private bool isDashing;

    private IEnumerator Dash()
    {
        isDashing = true;
        Vector2 dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        isDashing = false;

    }


    private void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 6;
            animator.SetBool("running", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3;
            animator.SetBool("running", false);

        }
    }

    private void FixedUpdate()
    {
        //if (m_Root.style.display == DisplayStyle.None)
        //{
        if (!isDashing)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = (new Vector2(horizontal, vertical)) * speed;
            animator.SetBool("walking", rb.velocity.magnitude > Mathf.Epsilon);

        }

        //}
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetInteger("attack", 1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetInteger("attack", 2);

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetInteger("attack", 0);
        }

        
    }
}
