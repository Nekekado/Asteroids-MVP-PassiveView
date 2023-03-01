using UnityEngine;
using Asteroids.Model;
using System;
using System.Collections.Generic;

public class PresentersFactory : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Root _root;
    [SerializeField] private Presenter _laserGunBulletTemplate;
    [SerializeField] private Presenter _defaultGunBulletTemplate;
    [SerializeField] private Presenter _asteroidTemplate;
    [SerializeField] private Presenter _asteroidPartTemplate;
    [SerializeField] private Presenter _nloTemplate;
    [SerializeField] private Presenter _blueSoldierTemplate;
    [SerializeField] private Presenter _redSoldierTemplate;
    [SerializeField] private Transform _redSoldiers;
    [SerializeField] private Transform _blueSoldiers;

    private List<Soldier> _redTeam = new List<Soldier>();
    private List<Soldier> _blueTeam = new List<Soldier>();

    public List<Soldier> GetSoldierTeam(Config.SoldiersTeam team) => 
        team != Config.SoldiersTeam.red ? _blueTeam : _redTeam;

    public List<Soldier> GetEnemySoldierTeam(Config.SoldiersTeam team) =>
        team != Config.SoldiersTeam.red ? _redTeam : _blueTeam;

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
    private Presenter CreatePresenter(Presenter template, Transformable model, Transform parent)
    {
        Presenter presenter = Instantiate(template, parent);
        presenter.Init(model, _camera);

        return presenter;
    }
    private void CreatSoldierArmy(int countSoldiers, List<Soldier> soldiers, Vector2 spawnPos, Config.SoldiersTeam team)
    {
        for(int i = 0; i < countSoldiers; i++)
        {
            Soldier soldier = new Soldier(spawnPos, Config.NloSpeed, team);
            soldier.Dead += OnDead;
            SetTarget(soldier);
            TrySetTargets();
            soldiers.Add(soldier);
            Presenter template = team == Config.SoldiersTeam.red ? _redSoldierTemplate : _blueSoldierTemplate;
            Transform parent = team == Config.SoldiersTeam.red ? _redSoldiers : _blueSoldiers;
            CreatePresenter(template, soldier, parent);
        }

        AddSoldiersInTeam(team, soldiers);
    }

    private void SetTarget(Soldier soldier)
    {
        List<Soldier> enemyArmy = GetEnemySoldierTeam(soldier.Team);
        if (enemyArmy.Count > 0)
        {
            int indexTarget = UnityEngine.Random.Range(0, enemyArmy.Count);
            soldier.SetTarget(enemyArmy[indexTarget]);
        }
    }

    public void CreateToWarningSides(int countSoldiers, Vector2 posRedArmy, Vector2 posBlueArmy)
    {
        List<Soldier> redArmy = new List<Soldier>(countSoldiers);
        List<Soldier> blueArmy = new List<Soldier>(countSoldiers);

        CreatSoldierArmy(countSoldiers, redArmy, posRedArmy, Config.SoldiersTeam.red);
        CreatSoldierArmy(countSoldiers, blueArmy, posBlueArmy, Config.SoldiersTeam.blue);
    }

    private void OnDead(Soldier soldier)
    {
        DeleteSoldierFromTeam(soldier.Team, soldier);
        soldier.Dead -= OnDead;
        TrySetTargets();
    }

    private void TrySetTargets()
    {
        TrySetTargetsForTeam(Config.SoldiersTeam.blue);
        TrySetTargetsForTeam(Config.SoldiersTeam.red);
    }

    private void TrySetTargetsForTeam(Config.SoldiersTeam teamName)
    {
        List<Soldier> team = GetSoldierTeam(teamName);
        List<Soldier> enemyTeam = GetEnemySoldierTeam(teamName);

        if (team.Count == 0 || enemyTeam.Count == 0)
            return;

        foreach (var soldier in team)
        {
            if(soldier.HasTarget == false)
            {
                int indexTarget = UnityEngine.Random.Range(0, enemyTeam.Count);
                soldier.SetTarget(enemyTeam[indexTarget]);
            }
        }
    }

    public void AddSoldiersInTeam(Config.SoldiersTeam team, List<Soldier> soldiers)
    {
        if (team == Config.SoldiersTeam.red)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                _redTeam.Add(soldiers[i]);
            }
        }
        else
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                _blueTeam.Add(soldiers[i]);
            }
        }
    }

    public void DeleteSoldierFromTeam(Config.SoldiersTeam team, Soldier soldier)
    {
        if (team == Config.SoldiersTeam.red)
        {
            _redTeam.Remove(soldier);
        }
        else
        {
            _blueTeam.Remove(soldier);
        }
    }
}
