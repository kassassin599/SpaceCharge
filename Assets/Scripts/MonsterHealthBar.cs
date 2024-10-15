using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
  public int maxHealth = 100;
  [SerializeField]
  private int currentHealth;

  [SerializeField]
  private Slider healthSlider;

  public int CurrentHealth
  {
    get => currentHealth;
    set
    {
      if (value >= maxHealth)
        return;
      currentHealth = value;
    }
  }

  void Start()
  {
    currentHealth = maxHealth;
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;

    healthSlider.value = currentHealth;

    if (currentHealth <= 0)
    {
      //Die();
      GameManager.instance.EnableEndScreen(true);
      GameManager.instance.storyPanelGO.SetActive(false);
      GameManager.instance.DisableGO();
    }
  }

  void Die()
  {
    gameObject.SetActive(false);
  }
}
