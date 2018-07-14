using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObjectTriggerWhenPlayerNaked : ObjectDestroyedTriggeredAction
{

    public Collider2D playerTrigger;
    public GameObject invisibleObject;

    private void Awake()
    {
        playerTrigger.gameObject.SetActive(false);
        invisibleObject.SetActive(false);
    }

    public override void Trigger()
    {
        playerTrigger.gameObject.SetActive(true);

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.attachedRigidbody.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (PlayerController.Instance.currentArmorStatus == PlayerController.PlayerArmor.Naked)
            {
                invisibleObject.SetActive(true);
                invisibleObject.transform.parent = null;
            }

            Destroy(gameObject);
        }
    }

}
