using UnityEngine;
using PrimeTween;
public class PlayerBullet : MonoBehaviour
{
  private Rigidbody2D rb;
  public int damage = 10;
  //[SerializeField]
  //private GameObject hitEffectPrefab;

  private GameObject monsterEnemy;

  private Animator animator;

  private void OnEnable()
  {
    Invoke("DestroyBullet", 5f);
  }

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    animator = GetComponentInChildren<Animator>();
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
        rb.velocity = Vector3.zero;

        //GameObject hitHiffectGO = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        //Destroy(hitHiffectGO, 1f);
        animator.SetTrigger("Hit");

        targetHealth.TakeDamage(damage);
        Invoke("DestroyBullet",.1f);
      }
    }
    if (collision.GetComponent<MonsterEnemy>() || collision.GetComponent<RadialMonsterEnemy>())
    {
      if (FindObjectOfType<MonsterHealthBar>())
      {
        rb.velocity = Vector3.zero;

        //GameObject hitHiffectGO = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        animator.SetTrigger("Hit");

        monsterEnemy = collision.transform.GetChild(0).gameObject;

        monsterEnemy.GetComponent<SpriteRenderer>().color = Color.red;

        collision.GetComponent<MonsterEnemy>().SetMonsterOGColor();

        Tween.ShakeLocalPosition(monsterEnemy.transform, strength: new Vector3(.3f, .3f), duration: .1f, frequency: 3);

        //Destroy(hitHiffectGO, 1f);

        FindObjectOfType<MonsterHealthBar>().TakeDamage(damage);
        //DestroyBullet();
        Invoke("DestroyBullet", .1f);

      }
    }
    if (collision.gameObject.layer == 6 /*Obstacles*/)
    {
      DestroyBullet();
    }
  }
}
