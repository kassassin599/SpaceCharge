using UnityEngine;


public class Health : MonoBehaviour
{
  public int maxHealth = 100;
  [SerializeField]
  private int currentHealth;

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
    if (GetComponent<PlayerController>())
    {
      if (FindFirstObjectByType<PlayerHealthBar>())
        FindFirstObjectByType<PlayerHealthBar>().slider.value = currentHealth;
    }
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    if (GetComponent<PlayerController>())
    {
      FindFirstObjectByType<PlayerHealthBar>().slider.value = currentHealth;
    }
    if (currentHealth <= 0)
    {
      if (GetComponent<PlayerController>())
      {
        GameManager.instance.EnableEndScreen(false);
        GameManager.instance.storyPanelGO.SetActive(false);
        GameManager.instance.DisableGO();
      }
      else
        Die();
    }
  }

  void Die()
  {
    gameObject.SetActive(false);
  }
}
