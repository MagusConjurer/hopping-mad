using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _enemyAnimations : MonoBehaviour {

    Animator animating;
    AnimationEvent heavyHit;
    AnimationEvent lightHit;
    AnimationClip heavyAttack;
    AnimationClip lightAttack;
    Vector2 enemyInput;
    private bool horizontal;

    bool fighting;
    bool heavyAttacking;
    bool heavyHitPlayer;
    bool heavyAttackFinished;
    bool lightAttacking;
    bool lightHitPlayer;
    bool lightAttackFinished;
    bool canceled;

    int randInt;
    float attackTimer = 0;

	void Start () {

        animating = GetComponent<Animator>();
        enemyInput = gameObject.GetComponentInParent<_enemy>().GetInput();
        

        foreach(AnimationClip clip in animating.runtimeAnimatorController.animationClips)
        {
            if(clip.name == "AttackUpdated")
            {
                heavyAttack = clip;
            }

            if(clip.name == "LightAttack")
            {
                lightAttack = clip;
            }
        }

        heavyHit = new AnimationEvent
        {
            functionName = "HeavyHit",
            time = 1f,
            objectReferenceParameter = gameObject
        };

        lightHit = new AnimationEvent
        {
            functionName = "LightHit",
            time = 1f,
            objectReferenceParameter = gameObject
        };


        heavyAttack.AddEvent(heavyHit);
        lightAttack.AddEvent(lightHit);

        randInt = Random.Range(0, 2);
    }
	
	// Update is called once per frame
	void Update () {

        enemyInput = gameObject.GetComponentInParent<_enemy>().GetInput();
        if (enemyInput.x > 0)
        {
            if (horizontal == false)
            {              
                if (animating.GetInteger("Is Walking") == 0)
                {
                    animating.Play("walk", -1, 0f);
                }
                animating.SetInteger("Is Walking", 1);
                horizontal = true;
            }
            fighting = false;
        }
        if (enemyInput.x < 0)
        {
            if(horizontal == false)
            { 
                if (animating.GetInteger("Is Walking") == 0)
                {
                    animating.Play("walk", -1, 0f);
                }
                animating.SetInteger("Is Walking", 1);
                horizontal = true;
            }
            fighting = false;
        }
        
        if (horizontal)
        {
            horizontal = false;
        }
        if (enemyInput.x == 0)
        {
            animating.SetInteger("Is Walking", 0);
        }


        if (fighting && enemyInput.x == 0)
        {
                        
            lightAttacking = animating.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LightAttack");
            heavyAttacking = animating.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack");

            attackTimer -= Time.deltaTime / 3;
            

            if (randInt == 0)
            {
               // Debug.Log(attackTimer);
                if (!(lightAttacking))
                {
                    animating.Play("LightAttack", -1, 0f);
                    animating.SetInteger("Is Attacking", 1);
                    lightHit.time = animating.GetCurrentAnimatorStateInfo(0).length;
                }

                if (lightAttacking)
                {
                    animating.SetInteger("Is Attacking", 0);                    
                    Reset();
                }

                if (attackTimer <= 0)
                {
                    attackTimer = heavyHit.time;
                    randInt = Random.Range(0, 2);
                    Debug.Log(randInt);
                }
            }

            if(randInt == 1)
            {
               // Debug.Log(attackTimer);
                if (!(heavyAttacking))
                {
                    animating.Play("Attack", -1, 0f);
                    animating.SetInteger("Is Attacking", 1);
                    heavyHit.time = animating.GetCurrentAnimatorStateInfo(0).length;

                }

                if (heavyAttacking)
                {
                    animating.SetInteger("Is Attacking", 0);
                    Reset();
                }

                if (attackTimer <= 0)
                {
                    attackTimer = lightHit.time;
                    randInt = Random.Range(0, 2);
                    Debug.Log(randInt);
                }
            }
            
        }
        if (!(fighting))
        {
            attackTimer = 0;
            animating.SetInteger("Is Attacking", 0);
        }

    }

    public void IsFighting(bool truth)
    {
        fighting = truth;
    }

    public void HeavyHit()
    {
        heavyHitPlayer = true;
    }

    public bool HeavyHitPlayer()
    {
        return heavyHitPlayer;
    }

    public void LightHit()
    {
        lightHitPlayer = true;
    }

    public bool LightHitPlayer()
    {
        return lightHitPlayer;
    }

    public void Reset()
    {
        heavyHitPlayer = false;
        lightHitPlayer = false;
    }

    public void Attack(bool truth)
    {
        heavyAttacking = truth;
    }

    public void CancelAttack(bool playerAttack)
    {
        canceled = playerAttack;
    }
}
