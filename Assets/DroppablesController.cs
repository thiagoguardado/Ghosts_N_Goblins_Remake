using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppablesController : SingletonAwakeGeneric<DroppablesController> {

    public Pot pot;
    public GameObject doll;
    public GameObject specialDoll;
    public GameObject accesory;
    public GameObject[] weapons;

    protected override void Awake()
    {
        base.Awake();

        DropItemBehavior.Reset();
    }

    public void DropPot(Vector3 position)
    {
        Pot go = Instantiate(pot, position, Quaternion.identity);
        go.Release();
    }

    public GameObject GetNextItem()
    {
        return SelectItem(DropItemBehavior.NextItem());
    }

    private GameObject SelectItem(DroppableItem item)
    {
        switch (item)
        {
            case DroppableItem.Doll:
                return doll;
            case DroppableItem.Weapon:
                return weapons[Random.Range(0, weapons.Length)];
            case DroppableItem.Accesory:
                return accesory;
            case DroppableItem.SpecialDoll:
                return specialDoll;
            default:
                return null;
        }
    }



}
