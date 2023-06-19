using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : MonoBehaviour
{
    private AvatarController player;
    public float hitValue = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<AvatarController>();    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<EnemyStats>().TookDamae(player.characters[player.currentCharacter].Atk * hitValue, false);
            Debug.Log("Working as intended");
        }
        else if (collision.CompareTag("Revenant"))
            collision.gameObject.GetComponent<RevenantController>().TookDamae(player.characters[player.currentCharacter].Atk * hitValue, false);

    }
}
