using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Стандартное поведение противника пока он не обнаружил игрока
/// </summary>
public class DefaultState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;
    EnemyController _enemyController;

    public bool IsPatroling()
    {
        return true;
    }
    public DefaultState(Sensor[] sensors, Transform transform, EnemyController enemyController)
    {
        _sensors = sensors;
        _transform = transform;
        _enemyController = enemyController;
    }

    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter DefaultState");
    }
    
}