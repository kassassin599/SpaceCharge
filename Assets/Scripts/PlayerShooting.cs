using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
  public GameObject bulletPrefab;
  public Transform gunPoint;  // Position from where the bullet will be fired
  public float bulletSpeed = 10f;
  public SpriteRenderer playerSpriteRenderer;  // Reference to the player's SpriteRenderer

  bool facingRight = true;

  void Update()
  {
    AimAtMouse();

    if (Input.GetMouseButtonDown(0))  // Left mouse button click
    {
      Shoot();
    }
  }

  void AimAtMouse()
  {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;

    Vector3 aimDirection = (mousePos - transform.position).normalized;
    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    if (mousePos.x < transform.position.x && facingRight)
    {
      Flip();
    }
    else if(mousePos.x > transform.position.x && !facingRight)
    {
      Flip();
    }
  }

  void Flip()
  {
    Vector3 currentScale = gameObject.transform.localScale;
    currentScale.y *= -1;
    gameObject.transform.localScale = currentScale;

    facingRight = !facingRight;
  }

  void Shoot()
  {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;
    Vector2 direction = (mousePos - transform.position).normalized;

    GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
  }
}
