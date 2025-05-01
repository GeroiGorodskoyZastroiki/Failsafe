using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public GameObject enemyWeapon;
    public FieldOfView FOV { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    [Header("State Settings")]
    public float normalSpeed = 3f;
    public float patrolSpeed = 3f;
    public float waitTime = 2f;
    public GameObject[] patrolPoints;
    public float lostPlayerTimer = 5f;
    public float enemyChaseSpeed = 8f;
    public bool afterChase = false; // ����, �����������, ��� ���� ��� � ��������� ������
    [Header("Search Settings")]
    public float searchRadius = 10f;
    public float searchDuration = 5f;
    public float timeToGet = 5f; // �����, �� ������� ���� ������ ��������� �� ����� ������
    public float changePointTimer = 5f; // ������ ��� ����� ����� ������
    public float offsetSearchinPoint = 5f; // ������ ��� ����� ����� ������

    // private EnemyStateFactory _stateFactory;
    private EnemyBaseState _currentState; // ��������� ����
    public EnemyBaseState CurrentState => _currentState; // �������� ��� ������� � �������� ���������
    [SerializeField] private string currentStateName; // ��� �������

    void Awake()
    {
        FOV = GetComponent<FieldOfView>();
        Agent = GetComponent<NavMeshAgent>();
        OnDetectPlayer();
    }

    void Start()
    {
        SwitchState(EnemyStateType.Patrol);
    }

    void Update()
    {
        CurrentState?.UpdateState(this); // ���������� ��������
        currentStateName = CurrentState?.GetType().Name; // ���������� ��� ���������
    }

    public void SwitchState(EnemyStateType newState)
    {
        _currentState?.ExitState(this); // ������ �� ����������� ���������
        _currentState = EnemyStateFactory.CreateState(newState);
        _currentState.EnterState(this);
    }

    private void OnDetectPlayer()
    {
        player.GetComponent<DetectionProgress>().OnDetected += () => SwitchState(EnemyStateType.Chase);
    }

}


