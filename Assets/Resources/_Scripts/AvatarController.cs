using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AvatarController : MonoBehaviour
{
    Animator animator;
    private float horizontal, vertical, startTime;
    private Rigidbody2D rb;
    public float speed = 3;
    public static bool set1 = true, isAttacking;
    public InventoryUIController inventory;
    private VisualElement m_Root;
    private GameManager gameManager;
    public List<Character> characters = new List<Character>();
    public int currentCharacter = -1;
    private readonly string[] names = { "Warrior", "Mage", "Minion", "Archer" };
    private float maxHP, currentHP;

    private bool damaged = false, isAlive = true;

    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

       
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

        //characters = playerData.characters;

        //LoadCharacters();
        if (!GameManager.SaveFileExists())
        {
            Debug.Log("ssssss");
            characters = gameManager.playerData.characters;
            foreach (var item in characters)
            {
                Debug.Log("yaaaaaaaaaaa");

            }
        }
        }


    public void LoadCharacters()
    {
        if (GameManager.SaveFileExists())
        {
            var playerData = gameManager.playerData;
            characters = playerData.characters;
            currentCharacter = playerData.currentCharacter;//aqui en lugar de cero sera el valor que leamos del archvo
            Set1Ready();
        }

        if (currentCharacter != -1)
        {
            ChangeCharacter(currentCharacter);
            ChangeStats(currentCharacter);
        }
    }

    private void ChangeStats(int currentCharacter)
    {
        Debug.Log(currentCharacter);
        currentHP = maxHP = characters[currentCharacter].Hp * characters[currentCharacter].Level;
        speed = characters[currentCharacter].Speed;
    }

    private void ChangeCharacter(int index)
    {
        List<GameObject> equipmentList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();


        // Iterar sobre los objetos secundarios
        foreach (GameObject childObject in equipmentList)
        {
            if (childObject.transform.IsChildOf(gameObject.transform))
            {
                // Hacer algo con cada objeto secundario
                if (names[index].Equals(childObject.tag))
                {
                    childObject.SetActive(true);
                }
                else if (names.Contains(childObject.tag))
                {
                    childObject.SetActive(false);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isAlive)
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

    private void Set1Ready  () {
        animator.SetInteger("ammo", 0);
        animator.SetInteger("weaponType", 0);
        for (int i = 0; i < 3; i++)
        {
            SetEquip(inventory.SetSlots[i]);
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

                        if (item.name == slotItem.Icon.name && item.transform.IsChildOf(gameObject.transform))
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

                            if (item.name == slotItem.Icon.name && item.transform.IsChildOf(gameObject.transform))
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
                        animator.SetBool("consumable", true);
                    }
                    else
                    {
                        animator.SetBool("consumable", false);

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
        if (!isDashing && isAlive)
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

    public void TookDamae(float damage, bool isMagic)
    {
        if (!damaged && isAlive)
        {
            Debug.Log("fff");
            StartCoroutine(TookDamage(damage, isMagic));
        }
    }
    public IEnumerator TookDamage(float damage, bool isMagic)
    {
        damaged = true;
        if (isMagic)
        {
            currentHP -= Math.Max(damage - characters[currentCharacter].MagDef, 1);

        }
        else
        {
            currentHP -= Math.Max(damage - characters[currentCharacter].Def, 1);

        }

        if (currentHP <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {

            yield return new WaitForSeconds(0.5f);



        }
        damaged = false;
        yield return 0;

    }

    private IEnumerator Death()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        var gameManagerScript = gameManager.GetComponent<GameManager>();

        characters[currentCharacter].isDead = true;
        gameManagerScript.Death();

        isAlive = false;
        transform.Find("KayKit Animated Character").GetComponent<Rotation>().enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("NewGameScene");

    }
}