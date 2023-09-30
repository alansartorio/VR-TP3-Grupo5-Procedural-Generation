using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        var enemy = transform.parent.parent;
        var healthManager = enemy.GetComponent<EnemyBehaviour>();
        slider.value = healthManager.Health / healthManager.MaxHealth;
        healthManager.OnHealthChange.AddListener(healthManager =>
        {
            slider.value = healthManager.Health / healthManager.MaxHealth;
        });
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = target.position + offset;
    }
}