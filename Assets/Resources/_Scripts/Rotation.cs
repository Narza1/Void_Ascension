using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

        // Obtener la posici�n del rat�n en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posici�n del rat�n en la pantalla a una posici�n en el mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

        // Obtener la posici�n del objeto en el mundo
        Vector3 objectPosition = transform.position;

        // Calcular la direcci�n hacia la posici�n del rat�n
        Vector3 direction = worldMousePosition - objectPosition;

        // Calcular el �ngulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;

        // Aplicar la rotaci�n al objeto
        Vector3 newRotation = new Vector3(transform.localEulerAngles.x, -angle, transform.localEulerAngles.z);
        transform.localEulerAngles = newRotation;



    }

}
