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
    public bool isAttacking, isDead;
    public static bool set1 = true, selectCharacter;
    public InventoryUIController inventory;
    private VisualElement m_Root, staminaBar, staminaBarScreen, hpBar, hpBarScreen;
    private GameManager gameManager;
    public List<Character> characters;
    public int currentCharacter = 0;
    private readonly string[] names = { "Minion", "Mage", "Warrior", "Archer" };
    public float currentHP, currentStamina = 1, maxStamina = 1;


    private bool damaged, isRunning = false;
    private void Awake()
    {
        selectCharacter = false;
    
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject ui = GameObject.Find("UserInterface");
        var doc = ui.GetComponent<UIDocument>().rootVisualElement;
        m_Root = doc.Q("Container");

        m_Root.style.display = DisplayStyle.None;
        inventory = ui.GetComponent<InventoryUIController>();
        staminaBar = doc.Q("Stamina");
        hpBar = doc.Q("HP");


        ui = GameObject.Find("StatusBars");
        doc = ui.GetComponent<UIDocument>().rootVisualElement;
        staminaBarScreen = doc.Q("Stamina");
        hpBarScreen = doc.Q("HP");


        // Obtiene una referencia al objeto secundario
        GameObject childObject = transform.Find("KayKit Animated Character").gameObject;

        // Obtiene una referencia al componente Animator del objeto secundario
        animator = childObject.GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
        isAttacking = false;

    }

    private IEnumerator RecoverStamina()
    {
        while (true)
        {
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            if (isRunning) { currentStamina -= 0.5f; }
            if (currentStamina < maxStamina && !isAttacking && !isRunning && !isDashing)
            {
                currentStamina += 0.5f;

            }

            staminaBar.style.width = staminaBarScreen.style.width = Length.Percent(currentStamina / maxStamina * 100);
            yield return new WaitForSeconds(0.05f);
        }

    }

    public void LoadCharacters()
    {
        if (GameManager.SaveFileExists() && !selectCharacter)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            var playerData = gameManager.playerData;
            characters = playerData.characters;
            currentCharacter = playerData.currentCharacter;//aqui en lugar de cero sera el valor que leamos del archvo
            Set1Ready();
            ChangeCharacter(currentCharacter);
            ChangeStats(currentCharacter);
            isDead = characters[currentCharacter].isDead;

        }
    }

    public void ChangeStats(int currentCharacter)
    {
        var currentCharacter1 = characters[currentCharacter];


        maxStamina = currentCharacter1.Stamina * currentCharacter1.Level;
        StartCoroutine(RecoverStamina());
        currentHP = currentCharacter1.Hp * currentCharacter1.Level;
        speed = currentCharacter1.Speed;
        GameObject ui = GameObject.Find("UserInterface");
        if (gameManager.playerData.currentFloor == 0)
        {
            currentHP = currentCharacter1.Hp * currentCharacter1.Level;
        }
        hpBar.style.width = hpBarScreen.style.width = Length.Percent(currentHP / (currentCharacter1.Hp * currentCharacter1.Level) * 100);
    }

    public void ChangeCharacter(int index)
    {
        List<GameObject> equipmentList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();


        // Iterar sobre los objetos secundarios
        foreach (GameObject childObject in equipmentList)
        {
            if (childObject.transform.IsChildOf(gameObject.transform))
            {
                // Hacer algo con cada objeto secundario
                if (childObject.CompareTag(names[index]))
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
        if (!isDead && !selectCharacter)
        {

            if (m_Root.style.display == DisplayStyle.None)
            {
                if (currentStamina != 0)
                {
                    Attack();
                    DashPattern();
                    Run();

                }
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

    private void Set1Ready()
    {
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
            ////////////////////////TODO
        }

    }

    public void SetConsumbleState(bool consumible)
    {
        var index = set1 ? 2 : 5;
        animator.SetBool("consumible", GameController.GetItemByGuid(inventory.SetSlots[index].ItemGuid).GetType().Equals(typeof(Consumable)));



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
                        animator.SetInteger("ammo", ammo);
                    }
                    break;

                case ObjectType.Consumable:

                    var index = set1 ? 2 : 5;
                        Debug.Log(index);

                    if (inventory.SetSlots[index].Equals(inventorySlot))
                    {
                        Debug.Log("yupi");
                        animator.SetBool("consumable", GameController.GetItemByGuid(inventory.SetSlots[index].ItemGuid).GetType().Equals(typeof(Consumable)));
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
    private bool isDashing = false;

    private IEnumerator Dash()
    {
        isDashing = true;
        currentStamina -= 5;
        Vector2 dashDirection = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        isDashing = false;

    }


    private void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            speed = characters[currentCharacter].Speed * 2;
            animator.SetBool("running", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || currentStamina <= 0)
        {
            isRunning = false;
            speed = characters[currentCharacter].Speed;
            animator.SetBool("running", false);

        }
    }

    private void FixedUpdate()
    {
        
        if (!isDashing && !isDead && !selectCharacter)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = (new Vector2(horizontal, vertical)) * speed;
            animator.SetBool("walking", rb.velocity.magnitude > Mathf.Epsilon);

        }

        
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
    public void RecoverHealth()

    {
        Debug.Log("start");
        var currentCharacter1 = characters[currentCharacter];
        var index = set1 ? 2 : 5;
        var recoveryItem = inventory.SetSlots[index];

        currentHP = Math.Min(currentHP + ((Consumable)GameController.GetItemByGuid(recoveryItem.ItemGuid)).recoveryValue, currentCharacter1.Hp * currentCharacter1.Level);
        hpBar.style.width = hpBarScreen.style.width = Length.Percent(currentHP / (currentCharacter1.Hp * currentCharacter1.Level) * 100);
        recoveryItem.UseItem();
        if (recoveryItem.ItemGuid == "")
            animator.SetBool("consumable", false);
    }
    public void TookDamae(float damage, bool isMagic)
    {
        if (!damaged && !isDead)
        {
            StartCoroutine(TookDamage(damage, isMagic));
        }
    }
    public IEnumerator TookDamage(float damage, bool isMagic)
    {
        var character = characters[currentCharacter];
        damaged = true;
        if (isMagic)
        {
            currentHP -= Math.Max(damage - character.MagDef, 1);

        }
        else
        {
            currentHP -= Math.Max(damage - character.Def, 1);

        }
        hpBar.style.width = hpBarScreen.style.width = Length.Percent((currentHP / (character.Hp * character.Level)) * 100);

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
        gameManagerScript.playerData.characters = characters;

        gameManagerScript.Death();

        isDead = true;
        transform.Find("KayKit Animated Character").GetComponent<Rotation>().enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("NewGameScene");

    }
}