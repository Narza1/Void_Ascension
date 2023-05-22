using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    //
    //
    //
    ////hay que arreglar el maldito collider
    [SerializeField]
    private float attackRange = 2f, movementSpeed = 3f, fuck2 = 0;
    [SerializeField]
    public AnimationColliderFix fuck;

    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    private void Start()
    {
        fuck = GameObject.Find("MushroomMon").GetComponent<AnimationColliderFix>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        animator = GameObject.Find("MushroomMon").GetComponent<Animator>();
    }
    private void Awake()
    {
        player = (GameObject.FindGameObjectWithTag("Player"));
    }
    private void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        // Normalizar la dirección para obtener un vector de dirección unitario
        Vector3 normalizedDirection = new Vector3(direction.x, direction.y, 0);
        normalizedDirection.Normalize();




        normalizedDirection = new Vector3(normalizedDirection.x, normalizedDirection.y, 0);



        // Multiplicar el vector de dirección por el valor de offset deseado
        Vector2 newOffset = normalizedDirection * fuck2;

        // Asignar el nuevo offset al CapsuleCollider
        cd.offset = newOffset;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //Debug.Log(distanceToPlayer);

        if (distanceToPlayer <= attackRange)
        {
            fuck2 = fuck.offset;

            animator.SetBool("isMoving", false);
            AttackPlayer();
        }
        else
        {
            fuck2 = 0;
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
        if (collision.gameObject.CompareTag("Player") && fuck.isAttacking)
        {
            Debug.Log("Hit cabron");
            player.GetComponent<AvatarController>().TookDamae(gameObject.GetComponent<EnemyStats>().Atk, false);
        }
    }

}
