using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _enemySpawning : MonoBehaviour {

    public int numEnemies = 0;
    public float spawnTime = 3f;
    public float spawnDelay = 5f;
    public Vector2[] spawnPositions;
    [HideInInspector]
    public bool dead = true;
    int killed;

    public GameObject _enemy1;
    public GameObject _enemy2;
    private List<GameObject> alive = new List<GameObject>();
    int max;
    int i = 0;
    int spawnIndex = 0;

    void Start ()
    {
        if (dead)
        {
            InvokeRepeating("Spawn", spawnTime, spawnDelay);
        }
    }

    private void Update()
    {
        for (int j = 0; j < i; j++)
        {
            if (alive[j] == null)
            {
                alive.Remove(alive[j]);
                killed++;
                i--;
                dead = true;
            }
        }
        if (i > numEnemies)
        {
            CancelInvoke();
        }
        if (killed == numEnemies)
        {
            numEnemies += 2;
        }
        else if (dead)
        {
            InvokeRepeating("Spawn", spawnTime, spawnDelay);
        }

        
    }

    void Spawn ()
    {
        //if (spawnIndex > spawnPositions.Length)
        //{
        //    spawnIndex = 0;
        //}
        max = spawnPositions.Length;
        spawnIndex = Random.Range(0, max);
        alive.Add(Instantiate(_enemy1, spawnPositions[spawnIndex], Quaternion.identity) as GameObject);
        i++;
        spawnIndex++;
        dead = false;
    }

    
}
