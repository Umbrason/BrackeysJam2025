using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootBullet : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    public readonly ModifyableFloat bulletRadius = new(.5f);
    public readonly ModifyableFloat timerInterval = new(.01f);
    public readonly ModifyableFloat initialLifeTime = new(4);
    public readonly ModifyableInt maxBounces = new(3);
    public readonly ModifyableInt damage = new(1);
    public readonly ModifyableFloat initialVelocity = new(40);
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        int bulletsSpawnedThisFrame = Mathf.FloorToInt(time / timerInterval.Current);
        int i = 0;
        while (time >= timerInterval.Current)
        {

            fireBullet((i++ + .5f) / (float)bulletsSpawnedThisFrame);
            time -= timerInterval.Current;
        }
    }

    void fireBullet(float offset)
    {
        var dir = transform.forward;
        var speed = dir * initialVelocity.Current;
        var startPos = transform.position + transform.forward * (.8f + bulletRadius.Current + .1f) + offset * Time.deltaTime * speed;
        Bullet newBullet = Instantiate(bullet, startPos, transform.rotation);
        newBullet.Init(maxBounces.Current, initialLifeTime.Current, speed, (uint)damage.Current, bulletRadius.Current);
    }

}
