using System;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    uint damage;
    int remainingBounces;
    float lifeTime;
    public Vector3 velocity;
    Cached<Rigidbody> cached_RB;
    Rigidbody Rigidbody => cached_RB[this];

    [SerializeField] private AudioClipGroup RicochetSFX;
    [SerializeField] private SpriteRenderer[] srs;
    [SerializeField] private Color FFModeColorTint;

    int noFFLayer;
    int FFLayer;

    void Awake()
    {
        noFFLayer = LayerMask.NameToLayer("PlayerProjectiles");
        FFLayer = LayerMask.NameToLayer("EnemyProjectiles");
    }

    public void Init(int bounces, float lifetime, Vector3 velocity, uint damage, float size)
    {
        this.remainingBounces = bounces;
        this.lifeTime = lifetime;
        this.velocity = velocity;
        this.damage = damage;
        Rigidbody.velocity = velocity;
        Rigidbody.WakeUp();
        transform.localScale = Vector3.one * size;
        gameObject.SetActive(true);
        SetLayer(noFFLayer);
        foreach (var sr in srs) sr.color = Color.white;
    }
    public event Action<Bullet> OnDespawn;

    private void SetLayer(int id)
    {
        gameObject.layer = id;
        transform.GetChild(0).gameObject.layer = id;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Despawn();
    }


    void FixedUpdate()
    {
        Rigidbody.velocity = velocity;
        transform.forward = velocity;
        transform.localScale = Vector3.one * (transform.localScale.x + UpgradeBulletSize.GrowthPerSecond * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
            normal += collision.GetContact(i).normal;
        normal = normal._x0z().normalized;

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            SFXPool.PlayAt(RicochetSFX, transform.position);
            velocity = Vector3.Reflect(velocity, normal);
            Rigidbody.MovePosition(Rigidbody.position + velocity.normalized * .05f);
            if (remainingBounces <= 0)
            {
                Despawn();
                return;
            }
            SetLayer(FFLayer);
            foreach (var sr in srs) sr.color = FFModeColorTint;
            remainingBounces--;
        }
        else
        {
            var hitbox = collision.collider.GetComponent<Hitbox>();
            hitbox?.RegisterDamageEvent(HealthEvent.Damage(damage, false, gameObject));
            Despawn();
        }
    }

    void OnCollisionStay(Collision collision) => OnCollisionEnter(collision);

    void Despawn()
    {
        Rigidbody.Sleep();
        Rigidbody.velocity = default;
        gameObject.SetActive(false);
        OnDespawn?.Invoke(this);
    }
}
