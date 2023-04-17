using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    Animator anim;
    InputManager input;

    private void Awake()
    {
        
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        input = InputManager.Instance;
    }

    private void Update()
    {
        if (input.PlayerChangeWeapon())
        {
            PlaySwithOutAnim();
        }
    }

    public void PlaySwithOutAnim()
    {
        anim.SetBool("hasChanged", true);
    }

}
