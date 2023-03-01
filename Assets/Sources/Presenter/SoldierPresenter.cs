using Asteroids.Model;
using UnityEngine;

public class SoldierPresenter : Presenter
{
    private bool CheckIsSoldier(Presenter presenter, out Soldier soldier)
    {
        var transformable = presenter.Model;

        if(transformable is Soldier)
        {
            soldier = (Soldier)transformable;

            return true;
        }

        soldier = null;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        print("Столкнулся");

        if (collider.gameObject.TryGetComponent(out Presenter presenter))
        {
            if (CheckIsSoldier(presenter, out Soldier soldier))
            {
                soldier.Die();
                Destroy(presenter.gameObject);
            }
        }
    }
}
