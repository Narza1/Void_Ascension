using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    AvatarController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<AvatarController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("Monster"))
        {
           collider.gameObject.GetComponent<EnemyStats>().TookDamae(10, true);
            Destroy(gameObject);
        }
        else if (collider.gameObject.name == "Wall")
        {
            Destroy(gameObject);
        }
    }
}


