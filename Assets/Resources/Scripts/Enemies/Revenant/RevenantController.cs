using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RevenantController : MonoBehaviour
{

    Animator animator;
    private float startTime;
    private Rigidbody2D rb;
    public bool isAttacking;
    private Character character;
    public CharacterData revenantData;
    private readonly string[] names = { "Warrior", "Mage", "Minion", "Archer" };
    private float maxHP, currentHP, speed;
    private bool damaged, isAlive = true, ready, inRange;
    private GameObject player;


    public void StartChar(CharacterData revenantData)
    {
        this.revenantData = revenantData;
        DatabaseTest databaseTest = GameObject.Find("GameManager").GetComponent<DatabaseTest>();
        GameObject childObject = transform.Find("KayKit Animated Character").gameObject;
        animator = childObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
        LoadCharacter();
        player = GameObject.Find("Player");
        SetEquip(revenantData.weaponGUID);
        SetEquip(revenantData.accessoryGUID);
        ready = true;
    }


    private void LoadCharacter()
    {
        //esto deberia ir dentro del gamemanager supongo
        //aqui iria si existe un fichero que lo cargue y si no crearlo de zero
        switch (revenantData.characterType)
        {
            case 0:
                character = new MinionCharacter();
                break;
            case 1:
                character = new MageCharacter();

                break;
            case 2:
                character = new WarriorCharacter();

                break;
            case 3:
                character = new ArcherCharacter();

                break;

        }
        ChangeCharacter(revenantData.characterType);
        currentHP = maxHP = character.Hp * revenantData.lv;
        speed = character.Speed;
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
                if (childObject.CompareTag(names[index]))
                    childObject.SetActive(true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (!isAttacking)
            {
                if (distanceToPlayer < 20)
                {
                    Attack();
                    if (distanceToPlayer > 2)
                        MoveToPlayer();
                }
                //Run();
                //DashPattern();
            }
        }

    }

    private void MoveToPlayer()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection.z = 0f;
        playerDirection.Normalize();

        // Calcular la velocidad de movimiento
        Vector3 desiredVelocity = playerDirection * speed;

        // Aplicar la velocidad usando un Rigidbody
        rb.velocity = desiredVelocity;
    }

    public void SetEquip(string guid)
    {
        if (!guid.Equals(""))
        {
            ItemDetails slotItem = GameController.GetItemByGuid(guid);
            if (slotItem == null) { return; }
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
                        animator.SetInteger("ammo", 99999999);
                    break;

                case ObjectType.Consumable:

                    if (slotItem.GetType() == typeof(Consumable))
                        animator.SetBool("consumable", true);
                    else
                        animator.SetBool("consumable", false);
                    break;

            }
        }
    }
    private void DashPattern()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
            StartCoroutine(Dash());
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

    private void Attack()
    {
        int value;
        if (inRange)
            value = Random.Range(0, 100);
        else
            value = Random.Range(70, 100);

        if (value < 50)
            animator.SetInteger("attack", 1);

        if (value >= 50 && value <= 90)
            animator.SetInteger("attack", 2);

        if (value > 90)
            animator.SetInteger("attack", 0);
    }

    public void TookDamae(float damage, bool isMagic)
    {
        if (!damaged && isAlive)
            StartCoroutine(TookDamage(damage, isMagic));
    }
    public IEnumerator TookDamage(float damage, bool isMagic)
    {
        damaged = true;
        if (isMagic)
            currentHP -= Math.Max(damage - character.MagDef, 1);
        else
            currentHP -= Math.Max(damage - character.Def, 1);

        if (currentHP <= 0)
            StartCoroutine(Death());
        else
            yield return new WaitForSeconds(0.5f);

        damaged = false;
        yield return 0;

    }

    private IEnumerator Death()
    {
        isAlive = false;
        transform.Find("KayKit Animated Character").GetComponent<Rotation>().enabled = false;
        animator.SetTrigger("Death");
        yield return 1;
    }
}
