using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private Vector2 direction;

    [SerializeField]
    [Range(0.0f, 10f)]
    private float movementSpeed = 1f;

    private readonly float movementSpeedModifier = 0.9f;
    private readonly float gravityScaleModifier = 0.75f;
    private readonly float sizeModifier = 0.5f;

    private float Damage { get => Ratio(5); }
    public int Points { get => (int)(Ratio(10)); }

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
    }

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player) 
            await player.Damage(Damage);
    }

    public void SplitUp(int parts = 2)
    {
        if (transform.localScale.x > 0.5f)
        {
            movementSpeed *= movementSpeedModifier;
            rb.gravityScale *= gravityScaleModifier;
            foreach (int _ in Enumerable.Range(1, parts))
            {
                Vector3 position = RandomVector(transform.position.x - (transform.localScale.x / 2), transform.position.x + (transform.localScale.x / 2), transform.position.y - (transform.localScale.y / 2), transform.position.y + (transform.localScale.y / 2));
                InitializeBall(position, transform.localScale * sizeModifier);
            }
        }

        Die();
    }

    private GameObject InitializeBall(Vector3 position, Vector3 size)
    {
        direction = RandomDirection(rb.velocity.normalized);
        GameObject ball = Instantiate(gameObject, position, Quaternion.identity);
        ball.transform.localScale = size;
        return ball;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private Vector2 RandomDirection(Vector2 vector) => new Vector2(Random.Range(0.3f, 1f) * (vector.x < 0 ? -1f : 1f), Random.Range(0.3f, 1f));
    private Vector3 RandomVector(float min1, float max1, float min2, float max2) => new Vector3(Random.Range(min1, max1), Random.Range(min2, max2), 0);

    private float Ratio(float multiplier) => (((transform.localScale.x / 2) * multiplier * 100) / rb.velocity.magnitude);
}
