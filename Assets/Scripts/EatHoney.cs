using System.Collections;
using UnityEngine;

public class EatHoney : MonoBehaviour
{
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float mass;
    [SerializeField] private float maxForce;

    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    private GameObject closestHive;
    private string hiveTag = "Hive";
    Animator _animator;

    private bool canMove;
    Bear bear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        maxSpeed = 0.2f;
        mass = 5f;
        maxForce = 0.1f;
        canMove = true;

        xOffset = Random.Range(0, 1000);
        yOffset = Random.Range(0, 1000);

        _animator = GetComponent<Animator>();

        bear = this.GetComponent<Bear>();
        bear.StartCoroutine(bear.LoseHunger(0.5f));
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (bear.GetHunger() < 20)
            {
                FindNearestHive();
            }
            else
            {
                Wander();
            }

            velocity += acceleration;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            velocity.y = 0;
            this.transform.position += velocity;

            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        if (velocity.sqrMagnitude > Vector3.zero.sqrMagnitude && Mathf.Abs(velocity.sqrMagnitude) >= 0.1)
        {
            _animator.SetBool("Run Backward", false);
            _animator.SetBool("Sit", false);
            _animator.SetBool("Walk Forward", false);
            _animator.SetBool("Walk Backward", false);
            _animator.SetBool("Run Forward", true);
        }
        else if (velocity.sqrMagnitude > Vector3.zero.sqrMagnitude && Mathf.Abs(velocity.sqrMagnitude) <= 0.1)
        {
            _animator.SetBool("Run Backward", false);
            _animator.SetBool("Sit", false);
            _animator.SetBool("Run Forward", false);
            _animator.SetBool("Walk Backward", false);
            _animator.SetBool("Walk Forward", true);
        }
        else if (velocity.sqrMagnitude < Vector3.zero.sqrMagnitude && Mathf.Abs(velocity.sqrMagnitude) >= 0.1)
        {
            _animator.SetBool("Run Forward", false);
            _animator.SetBool("Sit", false);
            _animator.SetBool("Walk Forward", false);
            _animator.SetBool("Walk Backward", false);
            _animator.SetBool("Run Backward", true);
        }
        else if (velocity.sqrMagnitude < Vector3.zero.sqrMagnitude && Mathf.Abs(velocity.sqrMagnitude) <= 0.1)
        {
            _animator.SetBool("Run Forward", false);
            _animator.SetBool("Sit", false);
            _animator.SetBool("Walk Forward", false);
            _animator.SetBool("Run Backward", false);
            _animator.SetBool("Walk Backward", true);
        }
        else
        {
            _animator.SetBool("Run Forward", false);
            _animator.SetBool("Run Backward", false);
            _animator.SetBool("Walk Forward", false);
            _animator.SetBool("Walk Backward", false);
            _animator.SetBool("Sit", true);
        }

        acceleration = Vector3.zero;
    }

    private void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void FindNearestHive()
    {
        Collider[] closeObjectColliders = Physics.OverlapSphere(transform.position, 10);

        closestHive = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider closeObjectCollider in closeObjectColliders)
        {
            Debug.Log(closeObjectCollider);
            if (closeObjectCollider.tag == hiveTag)
            {
                float distance = Vector3.Distance(transform.position, closeObjectCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestHive = closeObjectCollider.gameObject;;
                    closestDistance = distance;
                }
            }
        }

        Vector3 direction = closestHive.transform.position - transform.position;
        direction.Normalize();

        Vector3 desiredVelocity = direction * this.maxSpeed;

        Vector3 steeringForce = desiredVelocity - this.velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        this.ApplyForce(steeringForce);
    }

    void Wander()
    {
        float perlinX = Mathf.PerlinNoise(xOffset, 0);
        float perlinY = Mathf.PerlinNoise(yOffset, 0);
        float xVelocity = Unity.Mathematics.math.remap(0, 1, -this.maxSpeed, this.maxSpeed, perlinX);
        float zVelocity = Unity.Mathematics.math.remap(0, 1, -this.maxSpeed, this.maxSpeed, perlinY);
        this.xOffset += 0.01f;
        this.yOffset += 0.01f;
        velocity.x = xVelocity;
        velocity.z = zVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hive")
        {
            this.velocity = Vector3.zero;
            StartCoroutine(Eat(3));
            StartCoroutine(bear.GetComponent<BearHUD>().UpdateTimeEating(3));
        }
    }

    IEnumerator Eat(float duration)
    {
        canMove = false;
        yield return new WaitForSeconds(duration);
        int healthGained = Random.Range(2, 8);
        bear.Heal(healthGained);
        int hungerGained = Random.Range(20, 40);
        bear.GainHunger(hungerGained);
        canMove = true;
    }
}
