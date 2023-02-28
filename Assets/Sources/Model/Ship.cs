using System;
using UnityEngine;

namespace Asteroids.Model
{
    public class Ship : Transformable, IUpdatable
    {
        public Ship(Vector2 position, float rotation) : base(position, rotation) { }

        private uint _health = 3;
        private float _takenDamageTime = 0f;
        private readonly float _damageDelay = 0.5f;
        private readonly float _unitsPerSecond = 0.001f;
        private readonly float _maxSpeed = 0.0015f;
        private readonly float _secondsToStop = 1f;
        private readonly float _degreesPerSecond = 180;

        public Vector2 Acceleration { get; private set; }
        public uint Health => _health;

        public void Accelerate(float deltaTime)
        {
            Acceleration += Forward * (_unitsPerSecond * deltaTime);
            Acceleration = Vector2.ClampMagnitude(Acceleration, _maxSpeed);
        }

        public void Slowdown(float deltaTime)
        {
            Acceleration -= Acceleration * (deltaTime / _secondsToStop);
        }

        public void Rotate(float direction, float deltaTime)
        {
            if (direction == 0)
                throw new InvalidOperationException(nameof(direction));

            direction = direction > 0 ? 1 : -1;

            Rotate(direction * deltaTime * _degreesPerSecond);
        }

        public void TakeDamage()
        {
            if(_takenDamageTime + _damageDelay < Time.time)
            {
                _health--;
                _takenDamageTime = Time.time;
            }
        }

        public void Update(float deltaTime)
        {
            Move(Acceleration);
        }

        private void Move(Vector2 delta)
        {
            var nextPosition = Position + delta;

            nextPosition.x = Mathf.Repeat(nextPosition.x, 1);
            nextPosition.y = Mathf.Repeat(nextPosition.y, 1);

            MoveTo(nextPosition);
        }
    }
}
