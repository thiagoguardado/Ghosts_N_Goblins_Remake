using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationController : MonoBehaviour {

    // possible player states
    public enum AnimationLayer
    {
        Armored,
        Naked,
        ArmoredGreen,
        NakedGreen
    }

    private PlayerController playerController;

    [Header("Animator Control")]
    public Animator playerHumanAnimator; //animator attached to player
    public Animator playerFrogAnimator;

    public bool isRunning = false;
    public bool isCrouched = false;
    public bool isJumping = false;
    private bool isDead
    {
        get
        {
            return playerController.currentArmorStatus == PlayerController.PlayerArmor.Dead;
        }
    }
    private bool isOnVictoryPose
    {
        get
        {
            return playerController.IsOnVictoryPose;
        }
    }
    public bool throwSomething = false;
    public bool isClimbing = false;
    public bool isOnEndOfLadder = false;
    public bool isLeavingLadder = false;
    public bool isInvincible { get { return !playerController.isReceivingDamage; } }
    private bool isFrog { get { return playerController.IsFrog; } }

    // animation layer
    private AnimationLayer previousLayer = AnimationLayer.Armored;  // previous layer playing
    private AnimationLayer currentLayer = AnimationLayer.Armored;    // current layer playing
    private Dictionary<AnimationLayer, int> animationLayerNameToInt = new Dictionary<AnimationLayer, int>();    // layer name to int dictionay
    private PlayerController.PlayerArmor previousPlayerArmor;

    private void Awake()
    {

        playerController = GetComponent<PlayerController>();

        previousPlayerArmor = playerController.currentArmorStatus;

        FillDictionary();
    }

    private void Update()
    {
        ListenToCurrentLayerChange();
        UpdateAnimatorValues();
    }

    // listens to a layer change
    private void ListenToCurrentLayerChange()
    {

        // change animation layer depending on player armor status
        if (previousPlayerArmor != playerController.currentArmorStatus)
        {

            switch (playerController.currentArmorStatus)
            {
                case PlayerController.PlayerArmor.Armored:
                    currentLayer = AnimationLayer.Armored;
                    break;
                case PlayerController.PlayerArmor.Naked:
                    currentLayer = AnimationLayer.Naked;
                    break;
                default:
                    break;
            }

            previousPlayerArmor = playerController.currentArmorStatus;

        }



        if (currentLayer != previousLayer)
        {
            ChangeAnimatorLayer(currentLayer);
            previousLayer = currentLayer;
        }
        
    }

    // fill layers dictionary (name to int)
    void FillDictionary()
    {
        for (int i = 0; i < Enum.GetValues(typeof(AnimationLayer)).Length; i++)
        {
            animationLayerNameToInt.Add((AnimationLayer)i, playerHumanAnimator.GetLayerIndex(((AnimationLayer)i).ToString()));
        }
    }

    // change current layer
    void ChangeAnimatorLayer(AnimationLayer newLayer)
    {

        foreach (var item in animationLayerNameToInt)
        {
            if (item.Key == newLayer)
            {
                playerHumanAnimator.SetLayerWeight(item.Value, 1);
            }
            else
            {
                playerHumanAnimator.SetLayerWeight(item.Value, 0);
            }
        }
        

    }

    public void StartClimbingLadder(bool isOnTop)
    {
        isClimbing = true;
        isOnEndOfLadder = isOnTop;
        isLeavingLadder = isOnTop;
        playerHumanAnimator.ResetTrigger("FinishedLadder");
    }

    public void FinishClimbingLadder(bool finishedOnTop)
    {
        if(finishedOnTop)
            playerHumanAnimator.SetTrigger("FinishedLadder");

    }

    public void ResetClimbingAnimationVariables()
    {
        isClimbing = false;
        isOnEndOfLadder = false;
        isLeavingLadder = false;
    }

    private void UpdateAnimatorValues()
    {
        playerHumanAnimator.SetBool("isDead", isDead);
        playerHumanAnimator.SetBool("isRunning", isRunning);
        playerHumanAnimator.SetBool("isCrouched", isCrouched);
        playerHumanAnimator.SetBool("isJumping", isJumping);
        if (throwSomething)
        {
            playerHumanAnimator.SetTrigger("Throw");
            throwSomething = false;
        }
        playerHumanAnimator.SetBool("isClimbing", isClimbing);
        playerHumanAnimator.SetBool("isOnEndOfLadder", isOnEndOfLadder);
        playerHumanAnimator.SetBool("isLeavingLadder", isLeavingLadder);
        playerHumanAnimator.SetBool("isInvincible", isInvincible);
        playerHumanAnimator.SetBool("Victory", isOnVictoryPose);
        playerHumanAnimator.SetBool("isHidden", isFrog);

        playerFrogAnimator.SetBool("isRunning", isRunning);
        playerFrogAnimator.SetBool("isJumping", isJumping);
        playerFrogAnimator.SetBool("isDead", isDead);
        playerFrogAnimator.SetBool("isInvincible", isInvincible);
        playerFrogAnimator.SetBool("isHidden", !isFrog);
    }

    public void TriggerHit()
    {
        playerHumanAnimator.SetTrigger("Hit");
    }

    public void LeaveHitState()
    {
        playerHumanAnimator.SetTrigger("FinishHit");
    }

    public void ChangeBetweenHumanAndFrog(bool activateHuman)
    {

    //    playerHumanSpriteRenderer.gameObject.SetActive(activateHuman);
    //    playerFrogSpriteRenderer.gameObject.SetActive(!activateHuman);
        
    }

}
