using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnImpact : MonoBehaviour
{
    [SerializeField] private float lifeTime = 4;
    [SerializeField] private int maxBounces = 3;
    private Quaternion freezeRotation;

    void Start()
    {
        freezeRotation = transform.rotation;
    }
    void Update()
    {
        transform.rotation = freezeRotation;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
      if (collision.collider.tag == "Obstacle")
      {
          maxBounces--;
          if (maxBounces <= 0)
          {
              Destroy(gameObject);
          }
      }
        
    }


}
