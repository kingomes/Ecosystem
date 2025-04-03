using UnityEngine;

public class Swarm : MonoBehaviour
{
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float mass;
    [SerializeField] private float maxForce;

    private GameObject bear;
    private string bearTag = "Bear";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        maxSpeed = 0.3f;
        mass = 0.5f;
        maxForce = 0.1f;
        bear = GameObject.FindGameObjectWithTag(bearTag);
    }

    void FixedUpdate()
    {
        if (bear != null) {
            SwarmBear();
        }

        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        this.transform.position += velocity;

        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);

        acceleration = Vector3.zero;
    }

    private void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void SwarmBear()
    {

        float distance = Vector3.Distance(transform.position, bear.transform.position);

        if (distance < 5f)
        {
            Vector3 direction = bear.transform.position - transform.position;
            direction.Normalize();

            Vector3 desiredVelocity = direction * this.maxSpeed;

            Vector3 steeringForce = desiredVelocity - this.velocity;
            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

            this.ApplyForce(steeringForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bear")
        {
            this.GetComponent<Bee>().Sting(other.gameObject.GetComponent<Bear>());
        }
    }
}
