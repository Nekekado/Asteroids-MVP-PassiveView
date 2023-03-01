using UnityEngine;

public class ShipPresenter : Presenter
{
    private Root _root;

    public void Init(Root root)
    {
        _root = root;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(_root.Ship.Health > 0)
            {
                _root.Ship.TakeDamage();
            }
            else
            {
                _root.DisableShip();
            }
        }
    }
}
