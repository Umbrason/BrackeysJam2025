using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    uint damage;
    int remainingBounces;
    float lifeTime;
    public Vector3 velocity;
    Cached<Rigidbody> cached_RB;
    Rigidbody Rigidbody => cached_RB[this];

    public void Init(int bounces, float lifetime, Vector3 direction, uint damage, float size)
    {
        this.remainingBounces = bounces;
        this.lifeTime = lifetime;
        this.velocity = direction;
        this.damage = damage;
        transform.localScale = Vector3.one * size;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        Rigidbody.velocity = velocity;
        transform.forward = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
            normal += collision.GetContact(i).normal;
        normal = normal._x0z().normalized;

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            velocity = Vector3.Reflect(velocity, normal);
            Rigidbody.MovePosition(Rigidbody.position + velocity.normalized * .05f);
            remainingBounces--;
            if (remainingBounces <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            var hitbox = collision.collider.GetComponent<Hitbox>();
            hitbox?.RegisterDamageEvent(HealthEvent.Damage(damage));
            Destroy(gameObject);
        }
    }


}
