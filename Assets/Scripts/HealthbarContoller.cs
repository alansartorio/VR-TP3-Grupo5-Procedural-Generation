using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    private Camera _mainCamera;
    private Image _hpBarImage;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Start()
    {
        Transform hbContent = transform.Find("Canvas").Find("Content");
        _hpBarImage = hbContent.Find("Hp Bar").GetComponent<Image>();
        var enemy = transform.parent;
        var healthManager = enemy.GetComponent<EnemyBehaviour>();
        SetFillValues(healthManager);
        healthManager.OnHealthChange.AddListener(SetFillValues);
    }

    void Update() {
        transform.rotation = _mainCamera.transform.rotation;
    }

    private void SetFillValues(EnemyBehaviour healthManager)
    {
        var totalHealth = healthManager.MaxHealth;
        _hpBarImage.fillAmount = healthManager.Health / totalHealth;
    }
}
