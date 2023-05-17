using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    AvatarController player;
    private bool isTakingDamage = false;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<AvatarController>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if(collider.gameObject.CompareTag("Monster") && AvatarController.isAttacking) {
            collider.gameObject.GetComponent<EnemyStats>().TookDamae(10,false);
        }
    }
}
