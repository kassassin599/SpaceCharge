using UnityEngine;

public class Bullet : MonoBehaviour
{
  public int damage = 10;
  private Rigidbody2D rb;

  //void OnCollisionEnter2D(Collision2D collision)
  //{
  //  Health targetHealth = collision.gameObject.GetComponent<Health>();
  //  if (targetHealth != null)
  //  {
  //    targetHealth.TakeDamage(damage);
  //  }
  //  Destroy(gameObject);  // Destroy bullet on impact
  //}

  public Sprite[] bulletSprites;

  private void OnEnable()
  {
    Invoke("DestroyBullet", 5f);

    //int randomSprite = Random.Range(0, bulletSprites.Length);
    //GetComponent<SpriteRenderer>().sprite = bulletSprites[randomSprite];
  }


  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    // Move the bullet in the current direction it was set
    //rb.velocity = transform.right * bulletSpeed;

    // Rotate the bullet sprite to face the direction it's moving
    RotateBullet();

    int randomSprite = Random.Range(0, bulletSprites.Length);
    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bulletSprites[randomSprite];
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
      if (targetHealth.GetComponent<PlayerController>())
      {
        targetHealth.TakeDamage(damage);
        DestroyBullet();
      }
    }
    if (collision.gameObject.layer == 6 /*Obstacles*/)
    {
      DestroyBullet();
    }
  }
}

