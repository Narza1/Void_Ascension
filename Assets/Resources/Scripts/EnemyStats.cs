using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    private float hp = 10, stamina = 10, atk = 10, magAtk = 10, magDef = 10, def = 10;

    [SerializeField]
    private string[] itemGui;

    [SerializeField]
    private float[] probability;

    [SerializeField]
    private int[] quantity;

    [SerializeField]
    private int coins;

    private EnemyAI enemyAI;
    Collider2D collide;

    private int level = 1;
    Animator animator;
    private void Start()
    {
        //level *= GameController.GetFloor();
        SetStats();
        animator = GetComponent<Animator>();
        enemyAI= GetComponent<EnemyAI>();
        collide = GetComponent<Collider2D>();
       
    }

    private void SetStats()
    {
        hp *= level;
        stamina *= level;
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
        if(isMagic)
        {
            hp -= Math.Max(damage - magDef, 1);

        }
        else
        {
            hp -= Math.Max(damage - def, 1);

        }
        animator.SetTrigger("damage");
        if(hp <= 0) 
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
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
