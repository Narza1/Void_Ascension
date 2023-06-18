using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantMagicAttack : MonoBehaviour
{
    RevenantController player;
    void Start()
    {
        player = GameObject.Find("Revenant").GetComponent<RevenantController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<AvatarController>().TookDamae(10, true);
        }
        else if (collider.gameObject.name == "Walls")
        {
            Destroy(gameObject);
        }
    }
}
