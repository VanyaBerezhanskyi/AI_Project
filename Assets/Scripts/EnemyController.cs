#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float attackAccuracy = 3f;

    public float fovRadius = 5f;
    public float fovAngle = 45f;

    private Transform player;
    private Vector3 lastKnownPlayerPosition;
    private NavMeshAgent navMeshAgent;
    private ShootingСharacter shootingCharacter;

    private enum State
    {
        Patrol,
        Investigate,
        Attack
    }

    private State curentState = State.Patrol;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        shootingCharacter = GetComponent<ShootingСharacter>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastKnownPlayerPosition = transform.position;
    }

    private void Update()
    {
        if (IsPlayerInFOV())
        {
            curentState = State.Attack;
            lastKnownPlayerPosition = player.position;
        }
        else
        {
            if (curentState == State.Attack)
            {
                curentState = State.Investigate;
            }
        }

        switch (curentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Investigate:
                Investigate();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    private bool IsPlayerInFOV()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < fovRadius)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < fovAngle / 2f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, directionToPlayer, out hit, fovRadius))
                {
                    if (hit.transform == player)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void Patrol()
    {
        Vector3 patrolPoint = lastKnownPlayerPosition;

        if (navMeshAgent.remainingDistance == 0 || navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            patrolPoint += new Vector3(Random.Range(-patrolRadius, patrolRadius), 0, Random.Range(-patrolRadius, patrolRadius));

            navMeshAgent.destination = patrolPoint;
        }
    }

    private void Investigate()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.destination = lastKnownPlayerPosition;

        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
        {
            curentState = State.Patrol;
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackAccuracy)
        {
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = player.position;
        }

        shootingCharacter.Shoot();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyController enemy = (EnemyController)target;

        Handles.color = new Color(1, 0, 0, 0.1f);
        
        Vector3 forwardPointMinusHalfAngle = Quaternion.Euler(0, -enemy.fovAngle / 2f, 0) * enemy.transform.forward;

        Handles.DrawSolidArc(enemy.transform.position, Vector3.up, forwardPointMinusHalfAngle, enemy.fovAngle, enemy.fovRadius);

        Handles.color = Color.white;

        Vector3 handlePoint = enemy.transform.position + enemy.transform.forward * enemy.fovRadius;

        enemy.fovRadius = Handles.ScaleValueHandle(enemy.fovRadius, handlePoint, enemy.transform.rotation, 1f, Handles.ConeHandleCap, 0.25f);
    }
}
#endif
