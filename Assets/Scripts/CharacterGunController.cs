using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGunController : MonoBehaviour
{
    Gun gun;
    Animator animator;
    bool reloading;
    [SerializeField] GameObject gunMagMesh;
    private void Start()
    {
        gun = GetComponentInChildren<Gun>();
        animator = GetComponent<Animator>();
    }

    public void Shot()
    {
        animator.SetTrigger("Shot");
    }
    public void Reload()
    {
        if (!reloading)
        {
            StartCoroutine(ReloadRoutine());
        }
    }
    IEnumerator ReloadRoutine()
    {
        reloading = true;
        gunMagMesh.SetActive(true);
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(0.28f);
        float length = animator.GetCurrentAnimatorStateInfo(1).length;
        yield return new WaitForSeconds(length / 5);
        gun.EjectMag();
        yield return new WaitForSeconds(length * 0.8f);
        reloading = false;
        gunMagMesh.SetActive(false);
        gun.Reloaded();

    }
}
