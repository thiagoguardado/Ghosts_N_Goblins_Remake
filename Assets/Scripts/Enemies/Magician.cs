using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Enemy {

    [Header("Magician")]
    public float timeBlinking;
    public float timeOpened;
    public Animator animator;
    public MagicianSpell spellPrefab;

    protected override void Awake()
    {
        base.Awake();

        checkCollision.thisCollider.enabled = false;
    }

    // Use this for initialization
    override protected void Start () {

        base.Start();

        // notify event
        GameEvents.Enemies.MagicianSpawned.SafeCall();

        StartCoroutine(MagicianRoutine());

	}

    IEnumerator MagicianRoutine()
    {
        yield return new WaitForSeconds(timeBlinking);

        // open wings
        animator.SetTrigger("Open");

        // enable collider
        checkCollision.thisCollider.enabled = false;

        // cast spell
        CastSpell();

        yield return new WaitForSeconds(timeBlinking);

        Destroy(gameObject);
    }

    private void CastSpell()
    {
        MagicianSpell spell = Instantiate(spellPrefab, checkCollision.thisCollider.bounds.center, spellPrefab.transform.rotation);
        spell.spriteDirection.FaceDirection(spriteDirection.lookingDirection);
    }
}
