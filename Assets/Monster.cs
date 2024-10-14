using System.Collections;
using UnityEngine;

public class MonsterEnemy : MonoBehaviour
{
  public GameObject bulletPrefab;  // The bullet prefab to instantiate
  public float fireRate = 0.2f;    // Time between each bullet rain
  public float bulletSpeed = 5f;   // Speed of the bullets
  public float sprayAngle = 30f;        // Maximum angle for bullet spray (right to left)
  public Transform[] bulletSpawnPoints; // Array of spawn points for bullet pattern
  public float angleIncrement = 5f;     // How much the angle changes each shot

  private float currentAngle;           // The current angle of the bullet spray
  private bool increasingAngle = true;  // Whether the angle is increasing or decreasing

  public float xOffset = 0.5f;     // Optional offset from the left edge
  public float yOffset = -0.5f;    // Optional offset from the top edge

  private float screenWidth;
  private Vector3[] positions;   // Positions for top left, top mid, and top right
  private int currentPositionIndex = 0; // To track current position index


  void OnEnable()
  {
    // Calculate screen width in world units
    screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;

    //// Define positions for top-left, top-middle, and top-right
    //positions = new Vector3[]
    //{
    //        new Vector3(-screenWidth / 2 + xOffset, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + yOffset, 0),
    //        new Vector3(0, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + yOffset, 0),  // Top-middle
    //        new Vector3(screenWidth / 2 - xOffset, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + yOffset, 0)  // Top-right
    //};

    // Start firing bullets in a sweeping pattern
    StartCoroutine(FireBulletsInSweep());
    StartCoroutine(SwitchPosition());

  }

  void LateUpdate()
  {
    // Define positions for top-left, top-middle, and top-right
    positions = new Vector3[]
    {
            new Vector3(-screenWidth / 2 + xOffset, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + yOffset, 0),
            new Vector3(0, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + yOffset, 0),  // Top-middle
            new Vector3(screenWidth / 2 - xOffset, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + yOffset, 0)  // Top-right
    };

    // Ensure the monster stays in the current position
    transform.position = positions[currentPositionIndex];
  }
  IEnumerator FireBulletsInSweep()
  {
    while (true)
    {
      // Loop through all the bullet spawn points and fire a bullet from each one
      foreach (Transform spawnPoint in bulletSpawnPoints)
      {
        FireSweepingBullet(spawnPoint);
      }

      // Wait before the next pattern fires
      yield return new WaitForSeconds(fireRate);

      // Update the current angle based on whether we're increasing or decreasing
      if (increasingAngle)
      {
        currentAngle += angleIncrement;
        if (currentAngle >= sprayAngle)
        {
          currentAngle = sprayAngle;
          increasingAngle = false;  // Start decreasing
        }
      }
      else
      {
        currentAngle -= angleIncrement;
        if (currentAngle <= -sprayAngle)
        {
          currentAngle = -sprayAngle;
          increasingAngle = true;   // Start increasing again
        }
      }
    }
  }

  IEnumerator SwitchPosition()
  {
    while (true)
    {
      // Wait for a random time between 5 to 10 seconds
      yield return new WaitForSeconds(Random.Range(5f, 10f));

      // Move to the next position (top-left, top-middle, or top-right)
      currentPositionIndex = (currentPositionIndex + 1) % positions.Length;
    }
  }

  void FireSweepingBullet(Transform spawnPoint)
  {
    // Instantiate a bullet at the spawn point
    GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

    // Convert the sweeping angle to a direction vector
    Vector2 direction = new Vector2(Mathf.Sin(currentAngle * Mathf.Deg2Rad), -1).normalized;

    // Set the bullet's velocity using both horizontal and downward components
    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
  }

  private void OnDisable()
  {
    StopAllCoroutines();
  }
}
