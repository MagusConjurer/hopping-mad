using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (_controls))]
public class _player : MonoBehaviour {  

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accellerationTimeAirborne = .2f;
    float accellerationTimeGrounded = .1f;
    float moveSpeed = 20;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    protected _controls controller;
    _waypoint playerWP;
    _playerAnimations checkAnim;
    _rotate playerRotation;
    _checkMouseClicks mouse;

    GameObject[] waypoints;
    _waypoint connect;

    GameObject enemy;
    bool singleClick;
    bool heldClick;
    bool singleAttack = false;
    bool comboAttack;

    GameObject uimanager;
    Text score;
    int kills;

    void Start () {

        controller = GetComponent<_controls>();
        playerWP = GetComponent<_waypoint>();
        checkAnim = GetComponentInChildren<_playerAnimations>();
        playerRotation = GetComponentInChildren<_rotate>();
        mouse = GetComponent<_checkMouseClicks>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        uimanager = GameObject.FindGameObjectWithTag("UI");
        score = uimanager.GetComponentInChildren<Text>();
        connect = playerWP.GetClosestWaypoint(waypoints);

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (!(controller.fighting) && Input.GetAxis("Vertical") > 0 && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        if (controller.fighting)
        {
            input = input * 0;
            enemy = controller.CollidedWith();
            checkAnim.IsFighting(true);

            singleClick = mouse.GetSingle();
            comboAttack = checkAnim.ComboHitEnemy();
            Debug.Log(comboAttack);

            if (singleClick && checkAnim.IsAttacking() == false)
            {
                enemy.GetComponentInChildren<_UIHealth>().RemoveHealth(1);
            }
            if (comboAttack)
            {
                enemy.GetComponentInChildren<_UIHealth>().RemoveHealth(2);
            }
            if (enemy == null || enemy.GetComponentInChildren<_UIHealth>().StillAlive() == false)
            {
                kills += 1;
                score.text = "Kills : " + kills;
                checkAnim.IsFighting(false);
                controller.fighting = false;
            }


        }
        //if(playerWP.Connected.Count < 1)
        //{
        //    playerWP.Connected.Add(connect);
        //}
        //if (playerWP.Connected.Count > 1)
        //{
        //    connect = connect.GetClosestWaypoint(waypoints);
        //    playerWP.Connected.Clear();
        //    playerWP.Connected.Add(connect);
        //}


        float targetVelocityX = input.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accellerationTimeGrounded:accellerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}

    public bool SingleAttack()
    {
        singleAttack = checkAnim.IsAttacking();
        Debug.Log(singleAttack);
        return singleAttack;
    }

    public void LeaveCombat()
    {
        checkAnim.IsFighting(false);
        controller.LeaveCombat();
        if(playerRotation.ExitDirection() < 0)
        {
            transform.position = transform.position + new Vector3(1, 0, 0);
        }
        if(playerRotation.ExitDirection() > 0)
        {
            transform.position = transform.position + new Vector3(-1, 0, 0);
        }
        
    }

    public bool IsFalling()
    {
        bool falling;
        if (controller.collisions.below)
        {
            falling = false;
        }
        else
        {
            falling = true;
        }
        return falling;
    }
}
