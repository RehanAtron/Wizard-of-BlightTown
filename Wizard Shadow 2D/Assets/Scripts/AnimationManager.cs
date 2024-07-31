using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] public Animator animator;
    private string currentState;
    public void ChangeAnimation(string newState)
    {
        if (newState == currentState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
