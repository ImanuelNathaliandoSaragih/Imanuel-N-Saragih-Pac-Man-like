using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private BaseState _currentState;


    public PatrolState PatrolState = new PatrolState();

    public ChaseState ChaseState = new ChaseState();

    public RetreatState RetreatState = new RetreatState();

    [SerializeField]
    public List<Transform> Waypoints = new List<Transform>();

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent NavMeshAgent;

    [SerializeField]
    public float ChaseDistance;

    [SerializeField]
    public Player Player;

    [HideInInspector]
    public Animator Animator;
    private void Awake()

    {
        Animator = GetComponent<Animator>();
        _currentState = PatrolState;

        _currentState.EnterState(this);
        NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    private void Start()

    {

        if (Player != null)

        {

            Player.OnPowerUpStart += StartRetreating;

            Player.OnPowerUpStop += StopRetreating;

        }

    }

    private void Update()

    {
        if (Player != null)

        {

            Player.OnPowerUpStart += StartRetreating;

            Player.OnPowerUpStop += StopRetreating;

        }
        if (_currentState != null)

        {

            _currentState.UpdateState(this);

        }

    }
    public void SwitchState(BaseState state)

    {

        _currentState.ExitState(this);

        _currentState = state;

        _currentState.EnterState(this);

    }
    private void StartRetreating()

    {

        SwitchState(RetreatState);

    }


    private void StopRetreating()

    {

        SwitchState(PatrolState);

    }
    public void Dead()

    {

        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)

    {

        if (_currentState != RetreatState)

        {

            if (collision.gameObject.CompareTag("Player"))

            {

                collision.gameObject.GetComponent<Player>().Dead();

            }

        }

    }
}
