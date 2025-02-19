using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    void Awake()
    {
        MaxHealth.RegisterFloor(1);
        MaxHealth.OnChanged += HealthPool.Resize;
        Speed.OnChanged += v => Movement.Velocity = v;
        Speed.RegisterFloor(.5f);
        Firerate.OnChanged += v => Shooting.Firerate = v;
        Firerate.RegisterFloor(.5f);
        SpreadDegrees.OnChanged += v => Shooting.SpreadDegrees = v;
        SpreadDegrees.RegisterCeil(360);
        BulletsPerShot.OnChanged += v => Shooting.BulletsPerShot = v;
        BulletsPerShot.RegisterFloor(1);
        DamagePerBullet.OnChanged += v => Shooting.DamagePerBullet = v;
        DamagePerBullet.RegisterFloor(1);
        BulletRadius.OnChanged += v => Shooting.BulletRadius = v;
        BulletRadius.RegisterFloor(.5f);
        BulletBounces.OnChanged += v => Shooting.BulletBounces = v;
        BulletLifeTime.OnChanged += v => Shooting.BulletLifeTime = v;
        BulletLifeTime.RegisterFloor(.5f);
        BulletVelocity.OnChanged += v => Shooting.BulletVelocity = v;
        BulletVelocity.RegisterFloor(1f);
    }

    #region health
    Cached<HealthPool> cached_HealthPool;
    HealthPool HealthPool => cached_HealthPool[this];
    [field: Header("Health")]
    [field: SerializeField] public ModifyableInt MaxHealth { get; private set; } = new(5);
    #endregion

    #region movement
    Cached<PlayerMovement> cached_movement;
    PlayerMovement Movement => cached_movement[this];
    [field: Header("Movement")]
    [field: SerializeField] public ModifyableFloat Speed { get; private set; } = new(8);
    #endregion

    #region Shooting
    Cached<Shooting> cached_shooting;
    Shooting Shooting => cached_shooting[this];
    [field: Header("Shooting")]
    [field: SerializeField] public ModifyableFloat Firerate { get; private set; } = new(1);
    [field: SerializeField] public ModifyableInt SpreadDegrees { get; private set; } = new(5);
    [field: SerializeField] public ModifyableFloat BulletsPerShot { get; private set; } = new(1);
    [field: SerializeField] public ModifyableInt DamagePerBullet { get; private set; } = new(1);
    [field: SerializeField] public ModifyableFloat BulletRadius { get; private set; } = new(.5f);
    [field: SerializeField] public ModifyableInt BulletBounces { get; private set; } = new(3);
    [field: SerializeField] public ModifyableFloat BulletLifeTime { get; private set; } = new(4);
    [field: SerializeField] public ModifyableFloat BulletVelocity { get; private set; } = new(40);
    #endregion
}

