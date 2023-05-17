using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;
   

    void Start()
    {
        float rotationZ = transform.rotation.eulerAngles.z;

        // Create a vector that points in the direction of the object's rotation
        Vector3 direction = Quaternion.Euler(0, 0, rotationZ + 90) * Vector3.right;

        // Move the object in the direction of its rotation
       GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
    }


   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
