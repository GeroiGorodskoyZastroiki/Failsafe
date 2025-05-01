using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{ 
    float _lostPlayerTimer; // ������ ������ ������

    /// <summary>
    /// ����������� ��� ����� � ��������� �������������.
    /// </summary>
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.Agent.speed = 8f; // ��������� �������� �������������
        enemy.Agent.isStopped = false; // ��������� ��������
        _lostPlayerTimer = enemy.lostPlayerTimer; // ������������� ������� ������ ������


    }

    /// <summary>
    /// ����������� ��� ������ �� ��������� �������������.
    /// </summary>
    public override void ExitState(EnemyStateMachine enemy)
    {

    }

    /// <summary>
    /// ��������� ������ ��������� �������������.
    /// </summary>
    public override void UpdateState(EnemyStateMachine enemy)
    {        
         if(enemy.FOV.canSeePlayerFar || enemy.FOV.canSeePlayerNear) ChasePlayer(enemy); // ������ ������������� ������
        else
        {
            LosePlayer(enemy); // ������ ������ ������
        }


    }

    /// <summary>
    /// ������ ������������� ������.
    /// </summary>
    private void ChasePlayer(EnemyStateMachine enemy)
    {
        if (enemy.Agent == null || enemy.player == null) return;

        if (enemy.Agent.destination != enemy.player.transform.position)
        {
            enemy.Agent.SetDestination(enemy.player.transform.position);
        }
    }

    /// <summary>
    /// ������ ������ ������.
    /// </summary>
    private void LosePlayer(EnemyStateMachine enemy)
    {
        if (enemy.Agent == null) return;

        if (_lostPlayerTimer > 0)
        {
            _lostPlayerTimer -= Time.deltaTime;
            ChasePlayer(enemy); // ���������� �������� � ������
        }
        else
        {
            ResetChaseState(enemy);
        }
    }

    /// <summary>
    /// ����� ��������� �������������.
    /// </summary>
    private void ResetChaseState(EnemyStateMachine enemy)
    {
        _lostPlayerTimer = enemy.lostPlayerTimer; // ����� �������
        enemy.afterChase = true; // ���������� ���� ����� �������������
        enemy.SwitchState(EnemyStateType.Search); // ������������� �� ��������� ������
    }

    private void AttackStateSwitch(EnemyStateMachine enemy)
    {
        UnityEngine.Debug.Log("Switching to Attack State");
    }
}
