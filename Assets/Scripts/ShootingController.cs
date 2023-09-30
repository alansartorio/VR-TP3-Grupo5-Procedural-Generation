using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{
    public float fireRate = 10f;
    public float damagePerShot = 1;
    public float weaponRange = 10f;
    public Transform gunBarrel;

    [SerializeField] private InputActionReference triggerAction;

    [SerializeField] private AudioClip shootingSound; // Variable serializada para el clip de audio.
    private AudioSource audioSource;

    private float nextFireTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource.

        // Asegúrate de que el componente de Audio Source esté configurado.
        if (audioSource == null)
        {
            Debug.LogError("El componente AudioSource no está asignado en el Inspector.");
        }
        else
        {
            // Asigna el clip de audio si se proporciona en el Inspector.
            if (shootingSound != null) audioSource.clip = shootingSound;
        }

        triggerAction.action.performed += StartShooting;
        triggerAction.action.canceled += StopShooting;
    }


    private void Update()
    {
        if (triggerAction.action.ReadValue<float>() > 0.5f && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void StopShooting(InputAction.CallbackContext obj)
    {
        foreach (var p in gameObject.GetComponentsInChildren<ParticleSystem>()) p.Stop();

        audioSource.Stop();
    }

    private void StartShooting(InputAction.CallbackContext obj)
    {
        foreach (var p in gameObject.GetComponentsInChildren<ParticleSystem>()) p.Play();

        if (audioSource != null && audioSource.clip != null && !audioSource.isPlaying) audioSource.Play();
    }


    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit, weaponRange))
        {
            var hitDistance = Vector3.Distance(gunBarrel.position, hit.point);

            // Verificar si la distancia es menor o igual al rango del arma.
            if (hitDistance <= weaponRange)
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Intenta obtener el componente de daño en el objeto impactado
                    var target = hit.transform.GetComponent<EnemyBehaviour>();

                    if (target != null)
                        // Si el objeto impactado tiene un componente de daño, aplica daño
                        target.Damage(damagePerShot);
                }
        }
    }
}