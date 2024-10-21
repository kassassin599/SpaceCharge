using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
  [SerializeField]
  private float speed = 1.0f;
  [SerializeField]
  private TweenSettings tweenSettings;
  [SerializeField]
  private PlayerController player;

  private void Start()
  {
    player = FindObjectOfType<PlayerController>();

    Tween.Position(transform, player.transform.position, tweenSettings).OnComplete(() => Destroy(gameObject));
  }


  private void Update()
  {
    transform.Rotate(transform.right, speed * Time.deltaTime);
  }
}
