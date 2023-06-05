using System;
using System.Collections;
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
    private bool isBoss;
    [SerializeField]
    private int coins, experience;

    private EnemyAI enemyAI;
    CircleCollider2D collide;
    Animator animator;
    GameManager gameManager;

    private GameObject[] stairs;

    public float Atk { get => atk; set => atk = value; }
    public float MagAtk { get => magAtk; set => magAtk = value; }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //level *= GameController.GetFloor();
        SetStats();
        animator = gameObject.transform.Find("MushroomMonsterRot/Monster").gameObject.GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        collide = GetComponent<CircleCollider2D>();
        if (isBoss)
        {
            stairs = GameObject.FindGameObjectsWithTag("Stairs");
            foreach (var item in stairs)
            {
                item.SetActive(false);
            }
        }


    }

    private void SetStats()
    {
        var level = 1;//gameManager.playerData.currentFloor;
        hp *= level;
        atk *= level;
        magAtk *= level;
        magDef *= level;
        def *= level;

    }
    public void TookDamae(float damage, bool isMagic)
    {
        Debug.Log("monster hit");
        if (collide.enabled)
        {
            StartCoroutine(TookDamage(damage, isMagic));
        }
    }
    public IEnumerator TookDamage(float damage, bool isMagic)
    {
        Debug.Log("Got HIT" + hp);
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
            
            collide.enabled = false;
            yield return new WaitForSeconds(0.5f);
            collide.enabled = true;
           


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

        GameController.Drop(dropGUID, dropQuantity);
        gameManager.DropCoinsExp(coins, experience);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        if (isBoss)
        {
            foreach (var item in stairs)
            {
                item.SetActive(true);
            }
        }
    }

    private int GenerateDrop()
    {
        return Random.Range(0, itemGuid.Length);
    }
}
