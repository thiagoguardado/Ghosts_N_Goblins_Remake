using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAnimationController : MonoBehaviour {

    // possible player states
    public enum AnimationLayer
    {
        Dressed,
        Naked,
        DressedGreen,
        NakedGreen
    }

    public Animator playerAnimator;                                 //animator attached to player


    [Header("Animator Control")]
    public AnimationLayer currentLayer = AnimationLayer.Dressed;    // current layer playing
    public bool isRunning = false;
    public bool isCrouched = false;
    public bool isJumping = false;
    public bool throwSomething = false;

    private AnimationLayer previousLayer = AnimationLayer.Dressed;  // previous layer playing
    private Dictionary<AnimationLayer, int> animationLayerNameToInt = new Dictionary<AnimationLayer, int>();    // layer name to int dictionay
    

    private void Awake()
    {
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

    }

}
