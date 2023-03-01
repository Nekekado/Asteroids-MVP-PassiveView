using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Model
{
    public class Soldier : Enemy
    {
        public event Action<Soldier> Dead;

        private Soldier _target;
        private readonly float _speed;
        private readonly Config.SoldiersTeam _team;

        public Config.SoldiersTeam Team => _team;
        public bool HasTarget => _target != null;

        public Soldier(Vector2 position, float speed, Config.SoldiersTeam team) : base(position, 0)
        {
            _speed = speed;
            _team = team;
        }

        public override void Update(float deltaTime)
        {
            if (_target != null)
            {
                Vector2 nextPosition = Vector2.MoveTowards(Position, _target.Position, _speed * deltaTime);
                MoveTo(nextPosition);
                LookAt(_target.Position);
            }
        }

        private void LookAt(Vector2 point)
        {
            Rotate(Vector2.SignedAngle(Quaternion.Euler(0, 0, Rotation) * Vector3.up, (Position - point)));
        }

        public void Die()
        {
            Dead?.Invoke(this);
        }

        private void OnTargetSet()
        {
            _target.Dead += OnTargetLost;
        }

        public void SetTarget(Soldier target)
        {
            _target = target;
            OnTargetSet();
        }

        private void OnTargetLost(Soldier soldier)
        {
            _target.Dead -= OnTargetLost;
            _target = null;
        }
    }
}
