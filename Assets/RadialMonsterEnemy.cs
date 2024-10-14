using System.Collections;
using UnityEngine;

public class RadialMonsterEnemy : MonoBehaviour
{
  public GameObject bulletPrefab;       // The bullet prefab to instantiate
  public float fireRate = 0.2f;         // Time between each bullet rain cycle
  public float bulletSpeed = 5f;        // Speed of the bullets
  public Transform[] bulletSpawnPoints; // Array of spawn points around the monster
  public float sprayAngle = 45f;        // Maximum angle for bullet spray sweep (degrees)
  public float angleIncrement = 5f;     // How much the angle changes for the sweep


  public float movementSpeed = 2f;      // Speed of the monster's movement
  public float moveDelay = 1f;          // Delay at each position before moving again

  private Vector3 centerPosition;       // Center of the screen
  private Vector3 leftPosition;         // Middle-left of the screen
  private bool movingToLeft = true;     // Direction of movement

  private float[] currentAngles;        // Track current sweeping angle for each spawn point
  private bool[] increasingAngle;       // Track if angle is increasing or decreasing for each spawn point

  // Enum for predefined directions
  private enum Direction { Up, Down, Left, Right };
  private Direction[] spawnPointDirections; // Directions of each spawn point

  private void OnEnable()
  {
    // Set the monster's position to the center of the screen
    Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
    screenCenter.z = 0;
    transform.position = screenCenter;

    // Initialize the center and left positions
    centerPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
    leftPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.5f, Camera.main.nearClipPlane));

    centerPosition.z = 0;
    leftPosition.z = 0;

    // Initialize angles and directions for each spawn point
    currentAngles = new float[bulletSpawnPoints.Length];
    increasingAngle = new bool[bulletSpawnPoints.Length];
    spawnPointDirections = new Direction[bulletSpawnPoints.Length];

    // Assign default directions to each spawn point
    AssignDirections();

    // Start the movement coroutine
    StartCoroutine(MoveBetweenPoints());

    // Start firing bullets in the sweeping pattern
    StartCoroutine(FireBulletsInSweep());
  }

  IEnumerator MoveBetweenPoints()
  {
    while (true)
    {
      Vector3 targetPosition = movingToLeft ? leftPosition : centerPosition;

      // Move the monster to the target position
      while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
      {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        yield return null;
      }

      // Once reached, wait for a delay before moving back
      yield return new WaitForSeconds(moveDelay);

      // Toggle direction
      movingToLeft = !movingToLeft;
    }
  }

  //private void LateUpdate()
  //{
  //  // Set the monster's position to the center of the screen
  //  Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
  //  screenCenter.z = 0;
  //  transform.position = screenCenter;
  //}

  private void AssignDirections()
  {
    // Example setup: Assuming 4 spawn points (top, bottom, left, right)
    // You can customize these according to the number and positions of your spawn points.
    if (bulletSpawnPoints.Length == 4)
    {
      spawnPointDirections[0] = Direction.Up;     // Top
      spawnPointDirections[1] = Direction.Down;   // Bottom
      spawnPointDirections[2] = Direction.Left;   // Left
      spawnPointDirections[3] = Direction.Right;  // Right
    }
    // For additional spawn points, you can add more directions or handle them similarly.
  }

  IEnumerator FireBulletsInSweep()
  {
    while (true)
    {
      for (int i = 0; i < bulletSpawnPoints.Length; i++)
      {
        FireSweepingBullet(bulletSpawnPoints[i], i);
      }

      // Wait before the next firing cycle
      yield return new WaitForSeconds(fireRate);
    }
  }

  void FireSweepingBullet(Transform spawnPoint, int index)
  {
    // Instantiate a bullet at the spawn point
    GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

    // Get the base direction for this spawn point (Up, Down, Left, or Right)
    Vector2 baseDirection = GetBaseDirection(spawnPointDirections[index]);

    // Apply sweeping angle to the base direction
    Vector2 direction = ApplySweepToDirection(baseDirection, index);

    // Set the bullet's velocity
    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

    // Update sweeping angle for this spawn point
    if (increasingAngle[index])
    {
      currentAngles[index] += angleIncrement;
      if (currentAngles[index] >= sprayAngle)
      {
        currentAngles[index] = sprayAngle;
        increasingAngle[index] = false;  // Start decreasing
      }
    }
    else
    {
      currentAngles[index] -= angleIncrement;
      if (currentAngles[index] <= -sprayAngle)
      {
        currentAngles[index] = -sprayAngle;
        increasingAngle[index] = true;   // Start increasing again
      }
    }
  }

  Vector2 GetBaseDirection(Direction direction)
  {
    // Return the base direction vector based on the spawn point's direction
    switch (direction)
    {
      case Direction.Up:
        return Vector2.up;
      case Direction.Down:
        return Vector2.down;
      case Direction.Left:
        return Vector2.left;
      case Direction.Right:
        return Vector2.right;
      default:
        return Vector2.zero;
    }
  }

  Vector2 ApplySweepToDirection(Vector2 baseDirection, int index)
  {
    // Rotate the base direction by the current sweeping angle
    float angleInRadians = currentAngles[index] * Mathf.Deg2Rad;
    float cosAngle = Mathf.Cos(angleInRadians);
    float sinAngle = Mathf.Sin(angleInRadians);

    // Apply rotation to the base direction vector
    return new Vector2(
        baseDirection.x * cosAngle - baseDirection.y * sinAngle,
        baseDirection.x * sinAngle + baseDirection.y * cosAngle
    ).normalized;
  }

  private void OnDisable()
  {
    StopAllCoroutines();
  }
}
