using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] int maximum = 1000;
    [SerializeField] int prewarm = 500;
    [SerializeField] Bullet bulletTemplate;
    List<Bullet> Available = new();

    void Awake()
    {
        for (int i = 0; i < prewarm; i++)
            CreateBullet();
    }

    private void CreateBullet()
    {
        var bullet = Instantiate(bulletTemplate, transform);
        bullet.gameObject.SetActive(false);
        bullet.OnDespawn += ReturnToPool;
        Available.Add(bullet);
    }

    public Bullet Pull(Vector3 pos, Quaternion rot)
    {
        if (Available.Count == 0)
            CreateBullet();
        var bullet = Available[^1];
        Available.RemoveAt(Available.Count - 1);
        bullet.transform.position = pos;
        bullet.transform.rotation = rot;
        return bullet;
    }

    private void ReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        Available.Add(bullet);
    }

}