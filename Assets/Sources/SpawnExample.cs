using UnityEngine;
using Asteroids.Model;

public class SpawnExample : MonoBehaviour
{
    [SerializeField] private PresentersFactory _factory;
    [SerializeField] private Root _root;
    [SerializeField] private int _minSoldiersInArmy;
    [SerializeField] private int _maxSoldierInArmy;

    private int _index;
    private float _secondsPerIndex = 1f;

    private void Update()
    {
        int newIndex = (int)(Time.time / _secondsPerIndex);

        if(newIndex > _index)
        {
            _index = newIndex;
            OnTick();
        }
    }

    private void OnTick()
    {
        float chance = Random.Range(0, 100);

        if (chance < 80)
        {
            _factory.CreateNlo(new Nlo(_root.Ship, GetRandomPositionOutsideScreen(), Config.NloSpeed));

            int countSoldiers = Random.Range(_minSoldiersInArmy, _maxSoldierInArmy);
            Vector2 posRedArmy = GetRandomPositionOutsideScreen();
            Vector2 posBlueArmy = GetRandomPositionOutsideScreen();
            _factory.CreateToWarningSides(countSoldiers, posRedArmy, posBlueArmy);
        }
        else
        {
            Vector2 position = GetRandomPositionOutsideScreen();
            Vector2 direction = GetDirectionThroughtScreen(position);

            _factory.CreateAsteroid(new Asteroid(position, direction, Config.AsteroidSpeed));
        }
    }

    private Vector2 GetRandomPositionOutsideScreen()
    {
        return Random.insideUnitCircle.normalized + new Vector2(0.5F, 0.5F);
    }

    private static Vector2 GetDirectionThroughtScreen(Vector2 postion)
    {
        return (new Vector2(Random.value, Random.value) - postion).normalized;
    }
}
