using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject menu;
	public GameObject pauseMenu;
	public GameObject optionsMenu;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Return to character select screen
	public void Return()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("SelectionOfCharacter");
	}

	//Pause
	public void Pause()
	{
		if (optionsMenu.activeSelf)
		{
			optionsMenu.SetActive(false);
			pauseMenu.SetActive(true);
		}
		menu.SetActive(!menu.activeSelf);
		if (menu.activeSelf)
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1;
		}
	}

	public void Options()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		optionsMenu.SetActive(!optionsMenu.activeSelf);
	}
}
