using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;
    private Rigidbody2D rb;
    [SerializeField]
    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 30);
        rb= GetComponent<Rigidbody2D>();
        // Get the rotation of the object around the z-axis
        float rotationZ = transform.rotation.eulerAngles.z;

        // Create a vector that points in the direction of the object's rotation
        
        if (gameObject.CompareTag("Dagger"))
        {
            rotationZ += 90;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
        }
        else
        {
            //var player = GameObject.Find("Player").GetComponent<AvatarController>();
            //var aux = AvatarController.set1 ? 2 : 5;
            //var a = GameController.GetItemByGuid(player.inventory.SetSlots[aux].ItemGuid) as Throwing;
            //damage += a.damage;
            rotationZ -= 90;
        }
        Vector3 direction = Quaternion.Euler(0, 0, rotationZ) * Vector3.right;

        // Move the object in the direction of its rotation
        rb.velocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject col = other.gameObject;
       
        if (col.name == "Walls")
        {
            Invoke("StopArrow", 0.02f);
        }
        else if(col.tag.Equals("Monster"))
        {
            
            other.gameObject.GetComponent<EnemyStats>().TookDamae(damage, false);
            Destroy(gameObject);
        }


    }

    private void StopArrow()
    {
       rb.velocity= Vector3.zero;

       
    }
}
