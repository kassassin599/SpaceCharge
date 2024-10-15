using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
  private ParticleSystem ps;

  void Start()
  {
    ps = GetComponent<ParticleSystem>();
  }

  void Update()
  {
    // Destroy the particle system GameObject when it has stopped playing
    if (ps != null && !ps.IsAlive())
    {
      Destroy(gameObject);
    }
  }
}
