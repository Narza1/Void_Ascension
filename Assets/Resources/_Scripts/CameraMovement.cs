using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
       var pos = transform.position.y;
        pos++;
        transform.position = new(transform.position.x, pos);
        //transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, this.transform.position.z);
    }
}
