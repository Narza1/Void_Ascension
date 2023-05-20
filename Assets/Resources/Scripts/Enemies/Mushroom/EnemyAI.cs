using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private float attackRange = 40f, movementSpeed = 1f;
    [SerializeField]
    private bool isAttacking;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Awake()
    { 
        player = (GameObject.FindGameObjectWithTag("Player"));
    }
    private void Update()
    {


        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //Debug.Log(distanceToPlayer);

        if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isMoving", false);
            AttackPlayer();
        }
        else
        {
            animator.SetBool("isMoving", true);

            MoveToPlayer();
        }
    }



    private void AttackPlayer()
    {
       
        rb.velocity = Vector2.zero; rb.angularVelocity = 0f;
        animator.SetTrigger("attack");

    }

    private void MoveToPlayer()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection.z = 0f;
        playerDirection.Normalize();

        // Calcular la velocidad de movimiento
        Vector3 desiredVelocity = playerDirection * movementSpeed;

        // Aplicar la velocidad usando un Rigidbody
        rb.velocity = desiredVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {

            player.GetComponent<AvatarController>().TookDamage(gameObject.GetComponent<EnemyStats>().Atk,false);
        }
    }

}
