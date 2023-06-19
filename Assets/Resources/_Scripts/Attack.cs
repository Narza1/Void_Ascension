using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    AvatarController player;

    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<AvatarController>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (player.isAttacking)
        {
            if (collider.gameObject.CompareTag("Monster"))
            {
                player.DurabilityHit(3);
                collider.gameObject.GetComponent<EnemyStats>().TookDamae(player.characters[player.currentCharacter].Atk, false);
            }
            else if (collider.gameObject.CompareTag("Revenant"))
            {
                player.DurabilityHit(3);
                collider.gameObject.GetComponent<RevenantController>().TookDamae(player.characters[player.currentCharacter].Atk, false);
            }

        }
    }
}
