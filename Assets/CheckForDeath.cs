using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForDeath : MonoBehaviour 
{
	[SerializeField] Canvas c;
	[SerializeField] Player p;
	
	// Update is called once per frame
	void Update () 
	{
		c.enabled = !p.gameObject.activeSelf;
		if (c.enabled)
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void Respawn()
	{
		p.gameObject.SetActive(true);
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
