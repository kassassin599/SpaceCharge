using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public GameObject bulletPrefab;
  public Transform bulletSpawnPoint;  // The specific point where bullets will be spawned
  public float fireRate = 1f;  // Time between shots
  public float bulletSpeed = 5f;
  public float detectionRange = 10f;  // Max distance the enemy can see the player
  public LayerMask obstacleMask;      // LayerMask for obstacles that block line of sight
  public SpriteRenderer enemySpriteRenderer;  // Reference to enemy's SpriteRenderer
  public float spreadAngle = 15f;  // The angle difference between the bullets
  public float moveSpeed = 2f;       // Speed at which the enemy moves
  public float moveDistance = 3f;     // Distance the enemy moves left and right
  private Vector3 initialPosition;   // Initial position of the enemy

  private Transform player;
  private bool playerDetected = false; // Flag to indicate if the player is detected

  public GameObject heartPrefab;

  void Start()
  {
    player = GameObject.FindWithTag("Player").transform;
    initialPosition = transform.position; // Store the initial position
    StartCoroutine(ShootAtPlayer());
  }

  void Update()
  {
    MoveEnemy();
  }

  void MoveEnemy()
  {
    if (!playerDetected)
    {
      // Move left and right
      float newX = initialPosition.x + Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;
      transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
  }

  IEnumerator ShootAtPlayer()
  {
    while (true)
    {
      // Only shoot if the enemy is visible on the screen
      if (IsPlayerInFront() && PlayerInLineOfSight())
      {
        FlipSprite();  // Flip the sprite depending on player position
        Shoot();
      }

      yield return new WaitForSeconds(fireRate);
    }
  }

  void Shoot()
  {
    Vector3 direction = (player.position - transform.position).normalized;

    // Shoot three bullets with a spread
    for (int i = -1; i <= 1; i++)  // Three bullets: one straight, one left, one right
    {
      float angleOffset = spreadAngle * i;
      Vector3 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

      GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
      bullet.GetComponent<Rigidbody2D>().velocity = spreadDirection * bulletSpeed;
    }
  }
  void FlipSprite()
  {
    // Flip the sprite based on the player's position relative to the enemy
    if (player.position.x < transform.position.x)
    {
      //enemySpriteRenderer.flipX = true;  // Player is on the left, flip sprite
      enemySpriteRenderer.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
    else
    {
      //enemySpriteRenderer.flipX = false;  // Player is on the right, face normally
      enemySpriteRenderer.gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

    }
  }

  bool IsPlayerInFront()
  {
    // Check if the player is within the detection range and in front of the enemy
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if (distanceToPlayer <= detectionRange)
    {
      // The player is within the detection range, proceed to check line of sight
      return true;
    }

    return false;
  }

  bool PlayerInLineOfSight()
  {
    // Cast a ray from the enemy to the player to check if there are obstacles in between
    Vector2 directionToPlayer = (player.position - transform.position).normalized;
    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    // Raycast to detect obstacles between enemy and player
    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

    // If the raycast doesn't hit anything or hits the player, the player is visible
    if (hit.collider == null || hit.collider.CompareTag("Player"))
    {
      return true;
    }

    return false;  // There's an obstacle blocking the line of sight
  }

  private void OnDisable()
  {
    //Instantiate(heartPrefab, transform.position, Quaternion.identity);
    if (player)
      player.GetComponent<Health>().CurrentHealth += 30;
  }

}

