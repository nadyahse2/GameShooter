using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] float health;
    public Slider slider;
    private float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = health; // �������������� ������� ��������
        slider.minValue = 0;
        slider.maxValue = health; // ������������� ������������ �������� ��������
        slider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            EndGame();
            return;  // ������� �� Update, ����� �������� ���������� ��������
        }

        // ���������� �������� ������ ����
        slider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage); // ���������� �������� ��������
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Min(health, currentHealth + amount);
    }
    public void SetMaxHealth(float maxHealth)
    {
        health = maxHealth;
        slider.maxValue = health;
        currentHealth = health;
    }
    public void EndGame()
    {
      #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
      #else
        Application.Quit();
      #endif
    }
}
