using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] BulletPool pool;

    public float Firerate { get; set; }
    public int SpreadDegrees { get; set; }
    public float BulletsPerShot { get; set; }
    public int DamagePerBullet { get; set; }
    public float BulletRadius { get; set; }
    public int BulletBounces { get; set; }
    public float BulletLifeTime { get; set; }
    public float BulletVelocity { get; set; }
    public Vector2 FireDirection { private get; set; } = Vector2.up;
    private float shotCounter;

    void FixedUpdate()
    {
        shotCounter += Time.deltaTime * Firerate;
        var shotsThisFrame = Mathf.FloorToInt(shotCounter);
        var i = 0;
        while (shotCounter >= 1)
        {
            FireShot(i++ / (float)shotsThisFrame);
            shotCounter -= 1;
        }
    }

    private void FireShot(float timeOffset)
    {
        for (int i = 0; i < BulletsPerShot; i++)
        {
            var angle = Random.Range(-SpreadDegrees, SpreadDegrees);
            var dir = Quaternion.Euler(0, angle, 0) * FireDirection._x0y();
            var speed = dir * BulletVelocity;
            var startPos = transform.position + Vector3.up + dir * (.8f + BulletRadius + .1f) + timeOffset * Time.deltaTime * speed;
            Bullet newBullet = pool.Pull(startPos, Quaternion.LookRotation(dir));
            newBullet.Init(BulletBounces, BulletLifeTime, speed, (uint)DamagePerBullet, BulletRadius);
        }
    }

}
