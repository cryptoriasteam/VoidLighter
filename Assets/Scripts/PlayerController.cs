using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector2 input;

    Rigidbody2D rb;


    public float killCooldown = 0.4f;
    private float killCooldownTimer = 0f;

    public float killRange = 1f;

    public int playerHealth = 3;


public GameObject heart1;
public GameObject heart2;
public GameObject heart3;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
   
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y).normalized;

      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager2D.Instance.StartPing();
        }

       
        if (killCooldownTimer > 0)
            killCooldownTimer -= Time.deltaTime;

       
        if (Input.GetMouseButtonDown(0) && killCooldownTimer <= 0f)
        {
            TryKillEnemyAtMouse();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }



void TryKillEnemyAtMouse()
{

    float camDist = -Camera.main.transform.position.z;
    Vector3 wp = Camera.main.ScreenToWorldPoint(new Vector3(
        Input.mousePosition.x,
        Input.mousePosition.y,
        camDist
    ));
    Vector2 mousePos = new Vector2(wp.x, wp.y);


    float mouseAimRadius = 1.0f;   

    Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, mouseAimRadius);

    Collider2D bestTarget = null;
    float bestDist = Mathf.Infinity;

    foreach (var hit in hits)
    {
        if (hit.CompareTag("Enemy"))
        {
            float d = Vector2.Distance(mousePos, hit.transform.position);

            if (d < bestDist)
            {
                bestDist = d;
                bestTarget = hit;
            }
        }
    }

    if (bestTarget == null)
        return;


    float snipeRange = 3.5f;  


    float distPlayer = Vector2.Distance(transform.position, bestTarget.transform.position);

    if (distPlayer <= snipeRange)
    {
        Destroy(bestTarget.gameObject);
        GameManager2D.Instance.AddScore(1);
        GameManager2D.Instance.PlayKillSound();

        killCooldownTimer = killCooldown;
    }
}


public void DamagePlayer(int amount)
{
    if (GameManager2D.Instance.IsGameOver) return;


    playerHealth -= amount;

    UpdateHearts();

    if (playerHealth <= 0)
    {
        GameManager2D.Instance.GameOver();

    }
}

void UpdateHearts()
{
    if (playerHealth <= 2) heart3.SetActive(false);
    if (playerHealth <= 1) heart2.SetActive(false);
    if (playerHealth <= 0) heart1.SetActive(false);
}



}
