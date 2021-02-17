using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MainCharacter : MonoBehaviour
{
    Vector3 originalPos;
    Animator animator;
    CharacterManipulatorScript manipulatorScript;
    bool gameplayRunning;
    [SerializeField] float IKOn;
    void Awake()
    {
        originalPos = transform.position;
        manipulatorScript = GetComponent<CharacterManipulatorScript>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventManager.StartListening("Gameplay Started", EnableGameplay);
        EventManager.StartListening("Level Ended", DisableGameplay);
        EventManager.StartListening("Player Died", EnableRagdoll);

    }
    private void OnDisable()
    {
        EventManager.StopListening("Gameplay Started", EnableGameplay);
        EventManager.StopListening("Level Ended", DisableGameplay);
        EventManager.StopListening("Player Died", EnableRagdoll);


    }

    private void Won()
    {
        DisableGameplay();
        animator.SetBool("Won", true);
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;
        manipulatorScript.ToggleDeath();
    }


    public void DisableGameplay()
    {
        gameplayRunning = false;
        //  DisableIK();
    }
    void EnableGameplay()
    {
        gameplayRunning = true;
        EnableIK();
    }
    public void DisableIK()
    {
        IKOn = 0;
    }
    public void EnableIK()
    {
        IKOn = 1;
    }

    public void ResetCharacter()
    {
        if (manipulatorScript._ragdoll)
        {
            manipulatorScript.ToggleDeath();
        }
        transform.rotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        animator.SetBool("Won", false);
        animator.enabled = true;
        DisableGameplay();

    }
}
