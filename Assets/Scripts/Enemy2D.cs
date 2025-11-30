using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    public float speed = 2f;
    public float revealDistance = 3f;
    public float killDistance = 1f;

    private Transform player;
    private SpriteRenderer sr;

    private float lifeTimer = 15f; 

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager2D.Instance.IsGameOver) return;

    
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }


        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        float dist = Vector2.Distance(player.position, transform.position);

float sqrDist = (player.position - transform.position).sqrMagnitude;

if (sqrDist <= killDistance * killDistance)
{
    GameManager2D.Instance.DamagePlayer(1);
    Destroy(gameObject);
    return;
}



        bool visible = false;

        if (dist <= revealDistance)
            visible = true;

        if (GameManager2D.Instance.IsPingActive)
            visible = true;

        sr.enabled = visible;
    }
    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        GameManager2D.Instance.DamagePlayer(1);
        Destroy(gameObject);
    }
}

}
