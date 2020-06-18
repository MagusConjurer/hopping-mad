using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagement : MonoBehaviour {

    GameObject[] pauseObjects;
    GameObject[] deathObjects;
    GameObject UI;
    _UIHealth playerHealth;
    Scene scene;

    void Start () {

        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowWhenPaused");
        deathObjects = GameObject.FindGameObjectsWithTag("ShowWhenDead");
        UI = GameObject.FindGameObjectWithTag("UI");
        scene = SceneManager.GetActiveScene();
        if(scene.name != "MainMenu")
        {
            playerHealth = UI.GetComponent<_UIHealth>();
        }
        HidePaused();
        HideDead();
	}
	
	// Update is called once per frame
	void Update () {
        // Get the pause menu by pressing Esc
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowPaused();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                HidePaused();
            }
        }

        if (playerHealth != null && playerHealth.StillAlive() == false)
        {
            Time.timeScale = 0;
            ShowDead();
        }
	}
    // Reloads the level
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Pauses the scene
    public void PauseControl()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HidePaused();
        }
    }
    public void ShowPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }
    public void HidePaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void ShowDead()
    {
        foreach(GameObject d in deathObjects)
        {
            d.SetActive(true);
        }
    }
    public void HideDead()
    {
        foreach (GameObject d in deathObjects)
        {
            d.SetActive(false);
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
