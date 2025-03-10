using UnityEngine;

public class EatHoney : MonoBehaviour
{
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float mass;
    [SerializeField] private float maxForce;

    private GameObject hive;
    [SerializeField] private string hiveTag = "Hive";
    Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        maxSpeed = 0.5f;
        mass = 5f;
        maxForce = 0.1f;

        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        hive = GameObject.FindGameObjectWithTag(hiveTag);
        FindNearestHive();

        if (hive == null) return;

        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        this.transform.position += velocity;

        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);

        if (velocity.sqrMagnitude > Vector3.zero.sqrMagnitude)
        {
            _animator.SetBool("Run Forward", true);
        }
        else if (velocity.sqrMagnitude < Vector3.zero.sqrMagnitude)
        {
            _animator.SetBool("Run Backward", true);
        }
        else
        {
            _animator.SetBool("Run Forward", false);
            _animator.SetBool("Run Backward", false);
        }

        acceleration = Vector3.zero;
    }

    private void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void FindNearestHive()
    {
        GameObject[] hives = GameObject.FindGameObjectsWithTag(hiveTag);

        if (hives.Length == 0)
        {
            hive = null;
            return;
        }

        hive = hives[0];
        float closestDistance = Vector3.Distance(transform.position, hive.transform.position);

        foreach (GameObject hive in hives)
        {
            float distance = Vector3.Distance(transform.position, hive.transform.position);
            if (distance < closestDistance)
            {
                this.hive = hive;
                closestDistance = distance;
            }
        }

        Vector3 direction = hive.transform.position - transform.position;
        direction.Normalize();

        Vector3 desiredVelocity = direction * this.maxSpeed;

        Vector3 steeringForce = desiredVelocity - this.velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        this.ApplyForce(steeringForce);
    }
}
