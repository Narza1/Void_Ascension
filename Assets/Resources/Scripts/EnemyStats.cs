using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    private float hp = 10, atk = 10, magAtk = 10, magDef = 10, def = 10;

    [SerializeField]
    private string[] itemGuid;

    [SerializeField]
    private int[] quantities;

    [SerializeField]
    private int coins, experience;

    private EnemyAI enemyAI;
    Collider2D collide;
    Animator animator;
    GameManager gameManager;

    public float Atk { get => atk; set => atk = value; }
    public float MagAtk { get => magAtk; set => magAtk = value; }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //level *= GameController.GetFloor();
        SetStats();
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        collide = GetComponent<Collider2D>();

    }

    private void SetStats()
    {
        var level = gameManager.playerData.currentFloor;
        hp *= level;
        atk *= level;
        magAtk *= level;
        magDef *= level;
        def *= level;

    }
    public void TookDamae(float damage, bool isMagic)
    {
        if (collide.enabled)
        {
            StartCoroutine(TookDamage(damage, isMagic));
        }
    }
    public IEnumerator TookDamage(float damage, bool isMagic)
    {
        if (isMagic)
        {
            hp -= Math.Max(damage - magDef, 1);

        }
        else
        {
            hp -= Math.Max(damage - def, 1);

        }
        animator.SetTrigger("damage");
        if (hp <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            Debug.Log("hitINV");
            collide.enabled = false;
            yield return new WaitForSeconds(0.5f);
            collide.enabled = true;
            Debug.Log("CANhit");


        }
        Debug.Log(hp);
        yield return 0;

    }

    private IEnumerator Death()
    {
        collide.enabled = false;
        enemyAI.enabled = false;
        animator.SetTrigger("death");
        string[] dropGUID = new string[3];
        int[] dropQuantity = new int[3];
        for (int i = 0; i < 3; i++)
        {
            var index = GenerateDrop();
            dropGUID[i] = itemGuid[index];
            dropQuantity[i] = quantities[index];
        }

        gameManager.ManageDrops(dropGUID, dropQuantity);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private int GenerateDrop()
    {
        return Random.Range(0, itemGuid.Length);
    }
}
