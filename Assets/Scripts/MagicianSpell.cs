using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianSpell : MonoBehaviour {

    [Header("MagicianSpell")]
    public SpriteDirection spriteDirection;
    public float speed;
    public Collider2D collider;

    private void Update()
    {
        transform.Translate(spriteDirection.WorldLookingDirection * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            // turn player into frog
            PlayerController.Instance.TurnFromHumanToFrog();

            // destroy
            Destroy(gameObject);
        }


    }
    
}
