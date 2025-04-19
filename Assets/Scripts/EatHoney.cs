using System.Collections;
using UnityEngine;

public class EatHoney : MonoBehaviour
{
    // bear's forces
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float mass;
    [SerializeField] private float maxForce;

    // offsets and increments for perlin noise movement
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    private float xIncrement;
    private float yIncrement;

    // how long the bear walks in a particular direction
    private float walkIntervalDuration;
    private float walkIntervalTimer;

    // how long until the bear turns around
    private float turnIntervalDuration;
    private float turnIntervalTimer;

    // hive game objects to be eaten
    private GameObject closestHive;
    private string hiveTag = "Hive";

    // makes the animations work
    Animator _animator;

    // boolean to stop the bear from moving while eating and then allow it again after it's done
    private bool canMove;
    // the duration of the bear eating
    private float duration;

    // the bear and its UI elements
    Bear bear;
    [SerializeField] private BearHUD bearHUD;
    [SerializeField] private ProgressBar eatingBar;
    
    // how far the bear can see hives
    float searchRadius;
    float maxSearchRadius;
    float searchGrowthRate;

    // 
    private float circleDistance;
    private float circleRadius;
    private float wanderAngleChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        maxSpeed = 0.2f;
        mass = 5f;
        maxForce = 0.1f;
        canMove = true;

        xOffset = Random.Range(-1000, 1000);
        yOffset = Random.Range(-1000, 1000);

        _animator = GetComponent<Animator>();

        bear = this.GetComponent<Bear>();
        bear.StartCoroutine(bear.LoseHunger(0.5f));

        eatingBar.gameObject.SetActive(false);

        walkIntervalDuration = Random.Range(2, 5);
        walkIntervalTimer = 0;

        turnIntervalDuration = Random.Range(2, 5);
        turnIntervalTimer = 0;

        searchRadius = 10f;
        maxSearchRadius = 100f;
        searchGrowthRate = 5f;
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

    public float GetDuration()
    {
        return this.duration;
    }

    private void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void FindNearestHive()
    {
        Collider[] closeObjectColliders = Physics.OverlapSphere(transform.position, searchRadius);

        closestHive = null;
        float closestDistance = Mathf.Infinity;
        bool closestFound = false;

        foreach (Collider closeObjectCollider in closeObjectColliders)
        {
            if (closeObjectCollider.tag == hiveTag)
            {
                closestFound = true;
                float distance = Vector3.Distance(transform.position, closeObjectCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestHive = closeObjectCollider.gameObject;;
                    closestDistance = distance;
                }
            }
        }
        if (!closestFound)
        {
            // increase search radius because there are not any hives near
            searchRadius = Mathf.Min(searchRadius + searchGrowthRate * Time.deltaTime, maxSearchRadius);

            Wander();
            return;
        }

        // reset the search radius when a hive is found
        searchRadius = 10f;

        // seek the hive
        Vector3 direction = closestHive.transform.position - transform.position;
        direction.Normalize();

        Vector3 desiredVelocity = direction * this.maxSpeed;

        Vector3 steeringForce = desiredVelocity - this.velocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        this.ApplyForce(steeringForce);
    }

    void Wander()
    {
        //reduce the time left on the timers
        walkIntervalTimer -= Time.deltaTime;
        turnIntervalTimer -= Time.deltaTime;

        // perlin noise setup
        float perlinX = Mathf.PerlinNoise(xOffset, 0);
        float perlinY = Mathf.PerlinNoise(yOffset, 0);
        float xVelocity = Unity.Mathematics.math.remap(0, 1, -this.maxSpeed, this.maxSpeed, perlinX);
        float zVelocity = Unity.Mathematics.math.remap(0, 1, -this.maxSpeed, this.maxSpeed, perlinY);
        
        // change the speed of movement
        if (walkIntervalTimer <= 0)
        {
            this.xIncrement = Random.Range(-0.01f, 0.01f);
            this.yIncrement = Random.Range(-0.01f, 0.01f);
            walkIntervalTimer = walkIntervalDuration;
        }

        // turn around
        if (turnIntervalTimer <= 0)
        {
            this.xOffset *= -1;
            this.yOffset *= -1;

            // Bias toward center
            Vector2 centerVelocity = Vector2.zero;
            Vector2 offset = new Vector2(xOffset, yOffset);
            offset = Vector2.SmoothDamp(offset, Vector2.zero, ref centerVelocity, 5f); // 5 sec to fully settle
            xOffset = offset.x;
            yOffset = offset.y;

            turnIntervalTimer = turnIntervalDuration;
        }

        this.xOffset += this.xIncrement;
        this.yOffset += this.yIncrement;
        velocity.x = xVelocity;
        velocity.z = zVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hive" && bear.GetHunger() < 20)
        {
            this.velocity = Vector3.zero;
            StartCoroutine(Eat(3));
        }
    }

    IEnumerator Eat(float duration)
    {
        this.duration = duration;
        eatingBar.gameObject.SetActive(true);
        canMove = false;

        while (this.duration > 0)
        {
            StartCoroutine(bearHUD.UpdateTimeEating());
            yield return new WaitForSeconds(1f);
            this.duration -= 1;
        }

        eatingBar.gameObject.SetActive(false);

        int healthGained = Random.Range(2, 8);
        bear.Heal(healthGained);

        int hungerGained = Random.Range(20, 40);
        bear.GainHunger(hungerGained);

        canMove = true;
    }
}
