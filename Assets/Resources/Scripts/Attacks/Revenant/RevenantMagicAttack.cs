using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantMagicAttack : MonoBehaviour
{
    RevenantController revenant;
    void Start()
    {
        revenant = GameObject.Find("Revenant").GetComponent<RevenantController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<AvatarController>().TookDamae(revenant.character.Atk, true);
        }
        else if (collider.gameObject.name == "Walls")
        {
            Destroy(gameObject);
        }
    }
}
