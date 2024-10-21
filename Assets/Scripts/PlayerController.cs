using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float verticalSpeed = 5f;
  public float horizontalSpeed = 3f;
  private Rigidbody2D rb;

  bool goingUp = false;

  public MonsterEnemy monsterEnemy;
  //public RadialMonsterEnemy radialME;

  private Camera mainCamera;
  private Vector2 screenBounds;

  [SerializeField]
  private ParticleSystem effect;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    mainCamera = Camera.main;

    // Calculate screen bounds based on the camera's viewport
    screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
  }

  void Update()
  {
    // Move left and right using arrow keys or "A" and "D"
    float move = Input.GetAxis("Horizontal") * horizontalSpeed;
    rb.velocity = new Vector2(move, rb.velocity.y);

    // Fly upward when space is pressed
    if (Input.GetKey(KeyCode.Space))
    {
      rb.velocity = new Vector2(rb.velocity.x, verticalSpeed);
      goingUp = true;
      effect.Play();
    }
    else { effect.Stop(); }


    //if (rb.velocity.y <= 1 && goingUp)
    //{
    //  monsterEnemy.gameObject.SetActive(false);
    //  radialME.gameObject.SetActive(true);
    //  goingUp = false;
    //}
    //else if (rb.velocity.y > 1 && goingUp)
    //{
    //  monsterEnemy.gameObject.SetActive(true);
    //  radialME.gameObject.SetActive(false);
    //  goingUp = true;
    //}

    // Restrict player to stay within the camera bounds
    ClampPlayerToCameraBounds();
  }
  void ClampPlayerToCameraBounds()
  {
    Vector3 playerPosition = transform.position;

    // Get the camera bounds
    float minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    float maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    float minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
    float maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

    // Clamp the player position within the camera bounds
    playerPosition.x = Mathf.Clamp(playerPosition.x, minX, maxX);
    playerPosition.y = Mathf.Clamp(playerPosition.y, minY, maxY);

    // Apply the clamped position back to the player
    transform.position = playerPosition;
  }


}
