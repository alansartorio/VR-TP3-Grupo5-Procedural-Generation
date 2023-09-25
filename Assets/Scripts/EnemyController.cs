using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public GameObject bossPrefab; // Arrastra el prefab del jefe desde el Assets en el Inspector.

    private void Update()
    {
        // Raycast hacia adelante desde el enemigo.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            // Verifica si el objeto alcanzado por el raycast es otro enemigo.
            if (hit.collider.CompareTag("Enemy") && hit.collider.gameObject != null)
            {
                // Destruye el enemigo actual.
                Destroy(gameObject);
                
                // Destruye al enemigo alcanzado por el raycast.
                Destroy(hit.collider.gameObject);
                
                // Genera un nuevo jefe.
                Instantiate(bossPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
