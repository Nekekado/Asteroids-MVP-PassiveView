using UnityEngine;
using Asteroids.Model;
using System;
using System.Collections.Generic;

public class PresentersFactory : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Presenter _laserGunBulletTemplate;
    [SerializeField] private Presenter _defaultGunBulletTemplate;
    [SerializeField] private Presenter _asteroidTemplate;
    [SerializeField] private Presenter _asteroidPartTemplate;
    [SerializeField] private Presenter _nloTemplate;
    [SerializeField] private Presenter _blueSoldierTemplate;
    [SerializeField] private Presenter _redSoldierTemplate;

    public void CreateBullet(Bullet bullet)
    {
        if(bullet is LaserGunBullet)
            CreatePresenter(_laserGunBulletTemplate, bullet);
        else
            CreatePresenter(_defaultGunBulletTemplate, bullet);
    }

    public void CreateAsteroidParts(AsteroidPresenter asteroid)
    {
        for (int i = 0; i < 4; i++)
            CreatePresenter(_asteroidPartTemplate, asteroid.Model.CreatePart());
    }

    public void CreateNlo(Nlo nlo)
    {
        CreatePresenter(_nloTemplate, nlo);
    }

    public void CreateAsteroid(Asteroid asteroid)
    {
        AsteroidPresenter presenter = CreatePresenter(_asteroidTemplate, asteroid) as AsteroidPresenter;
        presenter.Init(this);
    }

    private Presenter CreatePresenter(Presenter template, Transformable model)
    {
        Presenter presenter = Instantiate(template);
        presenter.Init(model, _camera);

        return presenter;
    }
    private void CreatSoldierArmy(int countSoldiers, List<Soldier> soldiers, Vector2 spawnPos)
    {
        for(int i = 0; i < countSoldiers; i++)
        {
            Soldier soldier = new Soldier(spawnPos, Config.NloSpeed);
            soldiers.Add(soldier);
        }
    }

    public void CreateToWarningSides(int countSoldiers, Vector2 posRedArmy, Vector2 posBlueArmy)
    {
        List<Soldier> redArmy = new List<Soldier>(countSoldiers);
        List<Soldier> blueArmy = new List<Soldier>(countSoldiers);

        CreatSoldierArmy(countSoldiers, redArmy, posRedArmy);
        CreatSoldierArmy(countSoldiers, blueArmy, posBlueArmy);

        for(int i = 0; i < countSoldiers; i++)
        {
            blueArmy[i].AddTargets(redArmy);
            CreatePresenter(_blueSoldierTemplate, blueArmy[i]);

            redArmy[i].AddTargets(blueArmy);
            CreatePresenter(_redSoldierTemplate, redArmy[i]);
        }
    }
}
