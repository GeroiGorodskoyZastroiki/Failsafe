using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    /// <summary>
    /// Условие появления противника
    /// </summary>
    public interface ISpawnCondition
    {
        /// <summary>
        /// Проверка выполнения условия
        /// </summary>
        /// <returns>true если условие выполнено</returns>
        public bool IsTriggered();
        /// <summary>
        /// Сбросить параметры условия
        /// </summary>
        public void Reset();
    }

    public abstract class SpawnCondition : ISpawnCondition
    {
        public abstract bool IsTriggered();

        public virtual void Reset()
        {
        }
    }

    public class ConstantCondition : SpawnCondition
    {
        private bool _value;
        public ConstantCondition(bool value)
        {
            _value = value;
        }

        public override bool IsTriggered() => _value;
    }

    public class TimerCondition : SpawnCondition
    {
        private float _startTime;
        private float _delay;
        public TimerCondition(float delay)
        {
            _delay = delay;
            Reset();
        }
        public override bool IsTriggered() => _startTime + _delay < Time.time;

        public override void Reset()
        {
            _startTime = Time.time;
        }
    }

    public class RandomCondition : ISpawnCondition
    {
        private float _chance;

        public RandomCondition(float chance)
        {
            _chance = chance;
            Reset();
        }

        private float _randomNumber;
        public bool IsTriggered() => _chance >= _randomNumber;

        public void Reset()
        {
            _randomNumber = UnityEngine.Random.value;
        }
    }

    public class AndCondition : ISpawnCondition
    {
        private ISpawnCondition[] _triggers;

        public AndCondition(params ISpawnCondition[] triggers)
        {
            _triggers = triggers;
        }

        public bool IsTriggered()
        {
            foreach (var trigger in _triggers)
            {
                if (!trigger.IsTriggered())
                {
                    return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Reset();
            }
        }
    }

    public class OrCondition : ISpawnCondition
    {
        private ISpawnCondition[] _triggers;

        public OrCondition(params ISpawnCondition[] triggers)
        {
            _triggers = triggers;
        }

        public bool IsTriggered()
        {
            foreach (var trigger in _triggers)
            {
                if (trigger.IsTriggered())
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Reset();
            }
        }
    }

    public class EnemySpawnedCondition : SpawnCondition
    {
        private List<SpawnCandidate> _spawnedEnemies;
        private SpawnCandidate _enemy;

        public EnemySpawnedCondition(List<SpawnCandidate> spawnedEnemies, SpawnCandidate enemy)
        {
            _spawnedEnemies = spawnedEnemies;
            _enemy = enemy;
        }

        public override bool IsTriggered()
        {
            foreach (var spawnedEnemy in _spawnedEnemies)
            {
                if (spawnedEnemy == _enemy) return true;
            }
            return false;
        }
    }

    public class SpawnPointPresentCondition : SpawnCondition
    {
        private Dictionary<SpawnPointType, bool> _spawnPoints;
        private SpawnPointType _spawnPointType;

        public SpawnPointPresentCondition(Dictionary<SpawnPointType, bool> spawnPoints, SpawnPointType spawnPointType)
        {
            _spawnPoints = spawnPoints;
            _spawnPointType = spawnPointType;
        }

        public override bool IsTriggered() => _spawnPoints[_spawnPointType];
    }

    public class TriggerCondition : ISpawnCondition
    {
        private Func<bool> _trigger;
        private Action _resetTrigger;

        public TriggerCondition(Func<bool> trigger, Action resetTrigger = null)
        {
            _trigger = trigger;
            _resetTrigger = resetTrigger;
        }

        public bool IsTriggered() => _trigger();

        public void Reset()
        {
            _resetTrigger?.Invoke();
        }
    }

}