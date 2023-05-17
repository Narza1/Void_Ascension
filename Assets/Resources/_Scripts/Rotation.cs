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

        // Obtener la posición del ratón en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del ratón en la pantalla a una posición en el mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

        // Obtener la posición del objeto en el mundo
        Vector3 objectPosition = transform.position;

        // Calcular la dirección hacia la posición del ratón
        Vector3 direction = worldMousePosition - objectPosition;

        // Calcular el ángulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;

        // Aplicar la rotación al objeto
        Vector3 newRotation = new Vector3(transform.localEulerAngles.x, -angle, transform.localEulerAngles.z);
        transform.localEulerAngles = newRotation;



    }

}
