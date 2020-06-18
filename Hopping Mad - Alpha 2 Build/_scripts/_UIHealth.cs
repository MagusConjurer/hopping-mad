using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UIHealth : MonoBehaviour {

    Transform objectToFollow;
    public Vector3 localOffset;
    public Vector3 screenOffset;
    Vector3 enemyPos;
    Vector3 playerPos;
    RectTransform _myCanvas;

    public int totalHP = 3;
    bool alive = true;
    float UIHeart = 45;
    Image[] healthBar;
    public Sprite empty;

    private void Start()
    {
        objectToFollow = transform.parent;
        _myCanvas = transform.GetComponent<RectTransform>();
        enemyPos = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, 0);
        playerPos = new Vector3(0, _myCanvas.sizeDelta.y, 0);
        if (objectToFollow.tag == "Enemy")
        {
            SetHealth(totalHP, enemyPos);
        }
        if (transform.tag == "UI")
        {
            SetHealth(totalHP, playerPos);
            for (int i = 0; i < healthBar.Length; i++)
            {
                healthBar[i].rectTransform.sizeDelta = new Vector2(UIHeart, UIHeart);
                healthBar[i].transform.position = playerPos + new Vector3(UIHeart * i, 0, 0);
            }
        }
        
    }
    void Update () {

        // For following enemies
        if(objectToFollow.tag == "Enemy")
        {
            HealthAbove();
            //hit remove health
        }
        // For player display
        if(transform.tag == "UI")
        {
            // hit remove health
        }

        if(totalHP > 0)
        {
            alive = true;
        }

        if(totalHP == 0)
        {
            alive = false;
        }
        
	}

    void HealthAbove()
    {
        Vector3 worldPoint = objectToFollow.TransformPoint(localOffset);

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPoint);

        viewportPoint.z = 0;

        transform.localPosition = viewportPoint + screenOffset;
    }

    public void SetHealth(int amountHealth, Vector3 pos)
    {
        totalHP = amountHealth;
        healthBar = new Image[totalHP];
        for (int i = 0; i < totalHP; i++)
        {
            healthBar[i] = Instantiate(Resources.Load<Image>("_healthFull"), transform, false);
            healthBar[i].transform.position = pos + new Vector3(i, 0, 0); 
        }
    }

    private void RemoveHealth(int damage, Vector3 pos)
    {
        Debug.Log("here?");
        for(int i = totalHP - 1; i >= 0;i--)
        {
            Debug.Log("Remove");
            if(damage > 0)
            {
                Debug.Log("Health");
                //Image empty = Instantiate(Resources.Load<Image>("_healthEmpty"), transform, false);
                //healthBar[i].transform.position = pos + new Vector3 (i, 0, 0);
                healthBar[i].sprite = empty;
                totalHP -= 1;
                damage--;
            }
        }
    }
    public void RemoveHealth(int damage)
    {
        RemoveHealth(damage, enemyPos);
    }

    public bool StillAlive()
    {
        return alive;
    }
}
