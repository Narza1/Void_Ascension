using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    private Transform player;
    public float angleCorrection;
    // Start is called before the first frame update
    void Start()
    {
        player = (GameObject.FindGameObjectWithTag("Player")).transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objectPosition = transform.position;

        // Calcular la direcci�n hacia la posici�n del rat�n
        Vector3 direction = player.position - objectPosition;

        // Calcular el �ngulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleCorrection;

        // Aplicar la rotaci�n al objeto
        Vector3 newRotation = new Vector3(transform.localEulerAngles.x, -angle, transform.localEulerAngles.z);
        transform.localEulerAngles = newRotation;
    }
}
