using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    //
    //
    //
    ////hay que arreglar el maldito collider
    [SerializeField]
    private float attackRange = 2f, movementSpeed = 3f, offset = 0;
    [SerializeField]
    public AnimationColliderFix animationColliderFix;

    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    private void Start()
    { var monster = gameObject.transform.Find("MushroomMonsterRot/Monster").gameObject;
        animationColliderFix = monster.GetComponent<AnimationColliderFix>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

        animator = monster.GetComponent<Animator>();

    }
    private void Awake()
    {
        player = (GameObject.FindGameObjectWithTag("Player"));
    }
    private void Update()
    {
       
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //Debug.Log(distanceToPlayer);


        if (distanceToPlayer <= attackRange)
        {
            offset = animationColliderFix.offset;

            animator.SetBool("isMoving", false);
            AttackPlayer();

            Vector3 direction = player.transform.position - transform.position;

            // Normalizar la dirección para obtener un vector de dirección unitario
            Vector3 normalizedDirection = new Vector3(direction.x, direction.y, 0);
            normalizedDirection.Normalize();

            // Multiplicar el vector de dirección por el valor de offset deseado
            Vector2 newOffset = normalizedDirection * offset;

            // Asignar el nuevo offset al CapsuleCollider
            cd.offset = newOffset;
        }
        else
        {
           cd.offset = Vector2.zero;
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
        if (collision.gameObject.CompareTag("Player") && animationColliderFix.isAttacking)
        {
            player.GetComponent<AvatarController>().TookDamae(gameObject.GetComponent<EnemyStats>().Atk, false);
        }
    }

}
