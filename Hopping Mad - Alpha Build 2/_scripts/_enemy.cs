using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(_controls))]
public class _enemy : MonoBehaviour {

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accellerationTimeAirborne = .2f;
    float accellerationTimeGrounded = .1f;
    float moveSpeed = 8f;

    bool paused = false;
    bool toPause = true;
    public float waitTime = 5f;
    float nextMoveTime;
    float walkTime = 5;
    public float minWalkTime = 5;
    int walkRange = 30;
    bool goRight;
    bool goLeft;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    _controls controller;
   
    Vector2 input = new Vector2(1, 0);
    Vector2 direction = new Vector2(0, -1);
    Vector3 startRight;
    Vector3 startLeft;
    float dist = 4;
    float width = 2.5f;
    private bool ledge;

    _hoppingStar pathfinder;
    _waypoint enemy;
    private bool pathing = true;
    GameObject[] waypoints;
    List<_waypoint> path;
    _waypoint startingPoint;
    public _waypoint playerPoint;
    float destinedX;
    float destinedY;

    [HideInInspector]
    public bool fighting = false;
    bool heavyAttack;
    bool lightAttack;
    _UIHealth health;
    _enemyAnimations checkAnim;
    float delay;

    GameObject player;
    _player playerCheck;
    GameObject playerHealth;
    _UIHealth playerHealthCheck;


void Start()
    {
        controller = GetComponent<_controls>();
        pathfinder = GetComponent<_hoppingStar>();
        enemy = GetComponent<_waypoint>();
        health = GetComponentInChildren<_UIHealth>();
        checkAnim = GetComponentInChildren<_enemyAnimations>();
        player = controller.CollidedWith();
        playerCheck = GetComponent<_player>();
        playerHealth = GameObject.FindGameObjectWithTag("UI");
        playerHealthCheck = playerHealth.GetComponent<_UIHealth>();




        if (playerPoint == null)
        {
            pathing = false;
        }
        if(playerPoint != null)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
            startingPoint = enemy.GetClosestWaypoint(waypoints);
        }


        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        if (toPause)
        {
            if (!(paused))
            {
                input = PausingMovement();
            }

            if (paused)
            {
                input = PausingMovement();
            }
        }

        if (controller.Fighting())
        {
            input = input * 0;
            checkAnim.IsFighting(true);
            heavyAttack = checkAnim.HeavyHitPlayer();
            lightAttack = checkAnim.LightHitPlayer();

            if (heavyAttack == true)
            {
                if(playerHealthCheck.StillAlive() == true)
                {
                    playerHealth.GetComponent<_UIHealth>().RemoveHealth(2);
                    heavyAttack = false;
                } 
            }

            if (lightAttack == true)
            {
                if(playerHealthCheck.StillAlive() == true)
                {
                    playerHealth.GetComponent<_UIHealth>().RemoveHealth(1);
                    lightAttack = false;
                }
            }

            if (controller.Fighting() == false)
            {
                checkAnim.IsFighting(false);
            }

            if (health.StillAlive() == false)
            {
                checkAnim.IsFighting(false);
                Destroy(gameObject);
            }
        }

        input.x = DetectEdges();
        // Pathfinding
        //make when player comes within distance
        if (pathing)
        {
            path = pathfinder.RunHoppingStar(startingPoint,playerPoint);
            if (path != null)
            {
                for (int i = 0; i < path.Count;i++)
                {
                    destinedX = transform.position.x;
                    destinedY = transform.position.y;
                    if (Approximate(destinedX, playerPoint.X,0.5f) && Approximate( destinedY,playerPoint.Y, 0.5f))
                    {
                        input.x = Mathf.Abs(input.x);
                        input.y = 0;
                        pathing = false;
                    }
                    //if (Approximate(destinedX, path[i].X, 0.5f) && Approximate(destinedY, path[i].Y, 0.5f))
                    //{
                    //    i++;
                    //}
                    if (path[i].X != destinedX && pathing)
                    {
                        destinedX = transform.position.x;
                        destinedY = transform.position.y;
                        if (destinedX > path[i].X)
                        {
                            input.x = -Mathf.Abs(input.x);
                        }
                        if (destinedX < path[i].X)
                        {
                            input.x = Mathf.Abs(input.x);
                        }
                        if (controller.collisions.below && destinedY < path[i].Y && ledge)
                        {
                            input.y = 1;
                        }
                        if (destinedY > path[i].Y)
                        {
                            input.y = 0;
                        }
                    }
                }
            }
        }

        float targetVelocityX = input.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accellerationTimeGrounded : accellerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    static bool Approximate(float a, float b, float margin)
    {
        return (Mathf.Abs(a - b) < margin);
;    }

    public Vector2 GetInput()
    {
        return input;
    }

    private Vector2 PausingMovement()
    {
        float randfloat = Random.Range(0, walkRange);
        if((Time.time + randfloat) > minWalkTime)
        {
            walkTime = Time.time + randfloat;
        }

        if (Time.time < nextMoveTime)
        {
            paused = true;
            
            return Vector2.zero;
        }

        if (input.x > 0 && Time.time >= walkTime)
        {
            nextMoveTime = Time.time + waitTime;
        }

        paused = false;
        if (goLeft)
        {
            return new Vector2(-1, 0);
        }
        if (goRight)
        {
            return new Vector2(1, 0);
        }

        return new Vector2(1, 0); 
    }

    private float DetectEdges()
    {
        float shift = input.x;

        startRight = transform.position;
        startRight.x += width;
        startLeft = transform.position;
        startLeft.x += -width;

        Debug.DrawRay(startRight, direction * dist, Color.black);
        Debug.DrawRay(startLeft, direction * dist, Color.black);

        RaycastHit2D hitR = Physics2D.Raycast(startRight, direction, dist);
        RaycastHit2D hitL = Physics2D.Raycast(startLeft, direction, dist);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (controller.collisions.below && input.y == 1)
        {
            velocity.y = jumpVelocity;
        }

        // Check for ledges
        if (!(hitR) || !(hitL))
        {
            ledge = true;
        }
        // Detect ledges and turn around except when pathing
        if (controller.collisions.below && ledge && hitL && !(pathing))
        {
            shift = -1;
            ledge = false;
            goLeft = true;
            goRight = false;
        }
        if (controller.collisions.below && ledge && hitR && !(pathing))
        {
            shift = 1;
            ledge = false;
            goLeft = false;
            goRight = true;
        }

        // Check for walls
        if (controller.collisions.left || controller.collisions.right)
        {
            shift = input.x * (- 1);
        }

        return shift;
    }

    
}
