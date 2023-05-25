using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    AvatarController player;

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<AvatarController>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (AvatarController.isAttacking)
        {
            if (collider.gameObject.CompareTag("Monster"))
            {
                collider.gameObject.GetComponent<EnemyStats>().TookDamae(10, false);
            }
            else if (collider.gameObject.CompareTag("Revenant"))
            {
                collider.gameObject.GetComponent<RevenantController>().TookDamae(10, false);
            }

        }
    }
}
