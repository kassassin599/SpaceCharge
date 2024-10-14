using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
  public GameObject platformPrefab;       // Platform prefab
  public GameObject enemyPrefab;          // Enemy prefab
  public float platformWidth = 3f;        // Width of each platform
  public float heightDifference = 5f;     // Vertical distance between platforms
  public Transform spawnPoint;            // Spawn point for the platforms
  public int numberOfPlatforms = 10;      // Number of platforms to spawn in each batch
  public Transform player;                // Reference to the player object to track position

  private List<GameObject> platforms = new List<GameObject>();  // List to store platforms
  private int currentBatch = 0;           // Keep track of which batch of platforms player is on

  void Start()
  {
    // Initial platform spawning
    for (int i = 0; i < numberOfPlatforms; i++)
    {
      SpawnPlatform(i);
    }
  }

  void Update()
  {
    CheckForPlatformPass();
  }

  void SpawnPlatform(int index)
  {
    // Y position for each platform, based on index and height difference
    float yPos = spawnPoint.position.y + index * heightDifference;

    // Random X position for the platform
    float randomX = Random.Range(-platformWidth-3, platformWidth+3);

    // Spawn a single platform
    GameObject platform = Instantiate(platformPrefab, new Vector3(randomX, yPos, 0), Quaternion.identity);

    // Add the platform to the list for tracking
    platforms.Add(platform);

    // Spawn enemies on the platform
    SpawnEnemyOnPlatform(platform);
  }

  void SpawnEnemyOnPlatform(GameObject platform)
  {
    // Get platform position
    Vector3 platformPosition = platform.transform.position;

    // Spawn an enemy on top of the platform (slightly above the platform)
    Vector3 enemyPosition = new Vector3(platformPosition.x, platformPosition.y + .75f, 0);  // Adjust height as needed
    Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
  }

  void CheckForPlatformPass()
  {
    // Get the index of the second-to-last platform in the batch
    int secondLastIndex = (currentBatch + 1) * numberOfPlatforms - 2;

    // Check if player has passed the second-to-last platform
    if (player.position.y > platforms[secondLastIndex].transform.position.y)
    {
      // Spawn the next batch of platforms
      SpawnNextBatch();
    }
  }

  void SpawnNextBatch()
  {
    currentBatch++;

    for (int i = 0; i < numberOfPlatforms; i++)
    {
      int newIndex = currentBatch * numberOfPlatforms + i;
      SpawnPlatform(newIndex);
    }
  }
}