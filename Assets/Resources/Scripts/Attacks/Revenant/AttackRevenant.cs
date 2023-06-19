using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRevenant : MonoBehaviour
{
    AvatarController player;
    RevenantController revenant;

    private void OnEnable()
    {
        if (player == null)        
            player = GameObject.Find("Player").GetComponent<AvatarController>();       
        
        if (revenant == null)       
            revenant = GameObject.Find("Revenant").GetComponent<RevenantController>();
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {   
        if (collider.gameObject.CompareTag("Player") && revenant.isAttacking)
        {
            player.TookDamae(revenant.character.Atk, false);
        }
    }
}
