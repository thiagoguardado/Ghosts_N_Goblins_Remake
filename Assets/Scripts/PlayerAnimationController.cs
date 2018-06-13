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
    public Animator playerAnimator; //animator attached to player
    public bool isRunning = false;
    public bool isCrouched = false;
    public bool isJumping = false;
    public bool throwSomething = false;
    public bool isClimbing = false;
    public bool isOnEndOfLadder = false;

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
            animationLayerNameToInt.Add((AnimationLayer)i, playerAnimator.GetLayerIndex(((AnimationLayer)i).ToString()));
        }
    }

    // change current layer
    void ChangeAnimatorLayer(AnimationLayer newLayer)
    {

        foreach (var item in animationLayerNameToInt)
        {
            if (item.Key == newLayer)
            {
                playerAnimator.SetLayerWeight(item.Value, 1);
            }
            else
            {
                playerAnimator.SetLayerWeight(item.Value, 0);
            }
        }
        

    }

    public void StartClimbingLadder(bool isOnTop)
    {
        isClimbing = true;
        isOnEndOfLadder = isOnTop;
        playerAnimator.ResetTrigger("FinishedLadder");
    }

    public void FinishClimbingLadder(bool finishedOnTop)
    {
        if(finishedOnTop)
            playerAnimator.SetTrigger("FinishedLadder");
        isClimbing = false;
        isOnEndOfLadder = false;
    }

    private void UpdateAnimatorValues()
    {
        playerAnimator.SetBool("isRunning", isRunning);
        playerAnimator.SetBool("isCrouched", isCrouched);
        playerAnimator.SetBool("isJumping", isJumping);
        if (throwSomething)
        {
            playerAnimator.SetTrigger("throw");
            throwSomething = false;
        }
        playerAnimator.SetBool("isClimbing", isClimbing);
        playerAnimator.SetBool("isOnEndOfLadder", isOnEndOfLadder);

    }

}
