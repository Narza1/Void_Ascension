using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject caster;
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
        Debug.Log(caster.name);
        if (true )//añadir alguna condicion mas rollo que el tag no sea revenant
        {

            //idea de hacer return
            if (collider.gameObject.CompareTag("Monster"))
            {
                collider.gameObject.GetComponent<EnemyStats>().TookDamae(player.characters[player.currentCharacter].Atk, false);
            }
            else if (collider.gameObject.CompareTag("Revenant"))
            {
                collider.gameObject.GetComponent<RevenantController>().TookDamae(player.characters[player.currentCharacter].Atk, false);
            }

        }
    }
}
