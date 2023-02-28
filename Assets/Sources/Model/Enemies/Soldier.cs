using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Model
{
    public class Soldier : Enemy
    {
        private List<Soldier> _targets;
        private readonly float _speed;

        public Soldier(Vector2 position, float speed) : base(position, 0)
        {
            _speed = speed;
        }

        public override void Update(float deltaTime)
        {
            if (_targets.Count != 0)
            {
                Transformable target = _targets[Random.Range(0, _targets.Count)];

                Vector2 nextPosition = Vector2.MoveTowards(Position, target.Position, _speed * deltaTime);
                MoveTo(nextPosition);
                LookAt(target.Position);
            }
        }

        private void LookAt(Vector2 point)
        {
            Rotate(Vector2.SignedAngle(Quaternion.Euler(0, 0, Rotation) * Vector3.up, (Position - point)));
        }

        public void AddTargets(List<Soldier> targets)
        {
            if(targets.Count != 0)
            {
                for(int i = 0; i < targets.Count; i++)
                {
                    _targets.Add(targets[i]);
                }
            }
        }

        public void Deletetarget(Soldier target)
        {
            _targets.Remove(target);
        }
    }
}
