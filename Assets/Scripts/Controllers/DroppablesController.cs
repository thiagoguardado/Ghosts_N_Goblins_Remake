using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppablesController : SingletonAwakeGeneric<DroppablesController> {

    public Pot potPrefab;
    public GameObject collectibleDoll;
    public GameObject collectiblSpecialDoll;
    public GameObject collectibleAccesory;
    public GameObject[] collectibleWeapon;

    protected override void Awake()
    {
        base.Awake();

        DropItemBehavior.Reset();
    }

    public void DropPot(Vector3 position)
    {
        Pot go = Instantiate(potPrefab, position, Quaternion.identity);
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
                return collectibleDoll;
            case DroppableItem.Weapon:
                return collectibleWeapon[Random.Range(0, collectibleWeapon.Length)];
            case DroppableItem.Accesory:
                return collectibleAccesory;
            case DroppableItem.SpecialDoll:
                return collectiblSpecialDoll;
            default:
                return null;
        }
    }



}
