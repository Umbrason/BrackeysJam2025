using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float Velocity { get; set; }
    public Vector2 InputDirection { private get; set; }
    private Cached<Rigidbody> cached_RB;
    public Rigidbody RB => cached_RB[this];

    [SerializeField] private Spring.Config VelocitySpringConfig;
    private Vector2Spring VelocitySpring;
    void Start() => VelocitySpring = new(VelocitySpringConfig);
    void FixedUpdate()
    {
        VelocitySpring.RestingPos = Velocity * InputDirection;
        VelocitySpring.Step(Time.deltaTime);
        RB.velocity = VelocitySpring.Position._x0y();
    }

}
