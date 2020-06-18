using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _rotate : MonoBehaviour {

    GameObject player;
    GameObject enemy;
    Vector2 enemyMovement;
    bool inCombat;
    int leavingCombat;
    float direction;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBase");

        if (gameObject.tag == "EnemyBase")
        {
            enemy = gameObject;
        }

    }

    void Update () {

        
        if (gameObject == enemy)
        {
            inCombat = enemy.GetComponentInParent<_controls>().Fighting();
            if (inCombat)
            {
                direction = player.transform.position.x - enemy.transform.position.x;
                if (direction > 0)
                {
                    transform.eulerAngles = new Vector3(0, 90);
                }
                if (direction < 0)
                {
                    transform.eulerAngles = new Vector3(0, 270);
                }
            }
            else
            {
                enemyMovement = enemy.GetComponentInParent<_enemy>().GetInput();
                if(enemyMovement.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 90);
                }
                if (enemyMovement.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, 270);
                }
            }


        }

        if (gameObject == player)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.eulerAngles = new Vector3(0, 90);
                leavingCombat = 1;
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.eulerAngles = new Vector3(0, 270);
                leavingCombat = -1;
            }
        }


    }

    private bool IsFacing()
    {
        return false;
    }

    public int ExitDirection()
    {
        return leavingCombat;
    }
}
