using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
  private Rigidbody2D rb;
  public int damage = 10;

  private void OnEnable()
  {
    Invoke("DestroyBullet", 5f);
  }

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    // Move the bullet in the current direction it was set
    //rb.velocity = transform.right * bulletSpeed;

    // Rotate the bullet sprite to face the direction it's moving
    RotateBullet();
  }

  void RotateBullet()
  {
    // Calculate the angle of the velocity vector
    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

    // Apply the rotation to the bullet's transform
    transform.rotation = Quaternion.Euler(0, 0, angle);
  }

  private void DestroyBullet() => Destroy(gameObject);     // Destroy bullet on impact

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Health targetHealth = collision.gameObject.GetComponent<Health>();
    if (targetHealth != null)
    {
      if (targetHealth.GetComponent<Enemy>())
      {
        targetHealth.TakeDamage(damage);
        DestroyBullet();
      }
    }
    if (collision.GetComponent<MonsterEnemy>() || collision.GetComponent<RadialMonsterEnemy>())
    {
      if (FindObjectOfType<MonsterHealthBar>())
      {
        FindObjectOfType<MonsterHealthBar>().TakeDamage(damage);
        DestroyBullet();
      }
    }
    if (collision.gameObject.layer == 6 /*Obstacles*/)
    {
      DestroyBullet();
    }
  }
}
