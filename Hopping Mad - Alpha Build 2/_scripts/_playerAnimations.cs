using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _playerAnimations : MonoBehaviour {

    Animator animate;
    AnimationEvent comboHit;
    AnimationClip comboAttack;
    GameObject player;
    _player playerCheck;
    private bool isHorizontalAxisInUse = false;
    private bool isVerticalAxisInUse = false;
    private bool isLeftButtonInUse = false;
    private bool isRightButtonInUse = false;
    bool isRunning;
    bool isAttacking;
    bool isCombo;
    bool isCombo2;
    bool isActive;

    bool fighting = false;
    bool comboHitEnemy;


    void Start () {
        animate = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCheck = player.GetComponent<_player>();

        foreach (AnimationClip clip in animate.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Attack 3")
            {
                comboAttack = clip;
            }
        }

        comboHit = new AnimationEvent
        {
            functionName = "ComboHit",
            time = 1f,
            objectReferenceParameter = gameObject
        };

        comboAttack.AddEvent(comboHit);

    }
	
	void Update () {

        // Check if the player is falling
        if (playerCheck.IsFalling())
        {
            animate.SetInteger("Is Falling", 1);
        }
        if (!(playerCheck.IsFalling()))
        {
            animate.SetInteger("Is Falling", 0);
            animate.SetInteger("Is Landing", 1);
        }        

        // When not fighting, player animations play as normal
        if (!(fighting))
        {
            if (Input.GetAxis("Horizontal") != 0 && !(playerCheck.IsFalling()))
            {
                isRunning = true;
                if (isHorizontalAxisInUse == false)
                {
                    animate.Play("Run2", -1, 0f);
                    animate.SetInteger("Is Running", 1);                    
                    isHorizontalAxisInUse = true;
                }

            }

            if (Input.GetAxis("Vertical") > 0 && !(playerCheck.IsFalling()))
            {
                if (isVerticalAxisInUse == false)
                {
                    animate.Play("JumpStart", -1, 0f);
                    animate.SetInteger("Is Jumping", 1);
                    isVerticalAxisInUse = true;
                }

            }           
            
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            animate.SetInteger("Is Running", 0);
            isRunning = false;
            isHorizontalAxisInUse = false;
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            animate.SetInteger("Is Jumping", 0);
            isVerticalAxisInUse = false;       
        }

        if (!(playerCheck.IsFalling()) && !isRunning)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isAttacking = animate.GetCurrentAnimatorStateInfo(0).IsName("Base.Attack");
                isCombo = animate.GetCurrentAnimatorStateInfo(0).IsName("Base.Attack 2");
                isCombo2 = animate.GetCurrentAnimatorStateInfo(0).IsName("Base.Attack 3");
                if (isLeftButtonInUse == false && isAttacking == false)
                {
                    animate.Play("Attack", -1, 0f);
                    animate.SetInteger("Is Attacking", 1);
                    isLeftButtonInUse = true;
                }
                if(isLeftButtonInUse == false && isAttacking == true && isCombo == false)
                {
                    Reset();
                    animate.Play("Attack 2", -1, 0f);
                    animate.SetInteger("Is Combo", 1);
                    isLeftButtonInUse = true;
                }
                if (isCombo2)
                {
                    comboHit.time = animate.GetCurrentAnimatorStateInfo(0).length/2;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                isActive = animate.GetCurrentAnimatorStateInfo(0).IsName("Base.Block");
                if (isRightButtonInUse == false && isActive == false)
                {
                    animate.Play("Block", -1, 0f);
                    animate.SetInteger("Is Blocking", 1);
                    isRightButtonInUse = true;
                }
            }


            if (Input.GetKeyDown("e"))
            {
                isActive = animate.GetCurrentAnimatorStateInfo(0).IsName("Base.Dodge");
                if (isLeftButtonInUse == false && isActive == false)
                {
                    animate.Play("Dodge", -1, 0f);
                    animate.SetInteger("Is Dodging", 1);
                    playerCheck.LeaveCombat();
                    isLeftButtonInUse = true;
                }
            }
        }

        if (!(isCombo2))
        {
            animate.SetInteger("Is Combo", 0);
        }
        
        if (!(isActive) || !(isAttacking))
        {
            animate.SetInteger("Is Attacking", 0);
            animate.SetInteger("Is Dodging", 0);
            animate.SetInteger("Is Blocking", 0);
            isLeftButtonInUse = false;
            isRightButtonInUse = false;
        }
    }
    public void IsFighting(bool truth)
    {
        fighting = truth;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
    public void ComboHit()
    {
        comboHitEnemy = true;
    }
    public bool ComboHitEnemy()
    {
        return comboHitEnemy;
    }
    private void Reset()
    {
        comboHitEnemy = false;
    } 
}
