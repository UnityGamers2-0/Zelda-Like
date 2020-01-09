using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

    private GameObject[] characterList;

    private int index;

	// Use this for initialization
	private void Start ()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");

        characterList = new GameObject[transform.childCount];

        //Fill array with the character models
        for(int i = 0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;

        //Disable visability (so you don't see the characters unless you click the arrow )
        foreach(GameObject go in characterList)
            go.SetActive(false);

        if (characterList[index])
            characterList[index].SetActive(true);
	}

    public void ToggleLeft()
    {
		//Toggle off the current model
		characterList[index].SetActive(false);

        index--;
        if(index < 0)
            index = characterList.Length - 1;

		//Toggle on the new model
		characterList[index].SetActive(true);
    }

	public void ToggleRight()
	{
		//Toggle off the current model
		characterList[index].SetActive(false);

		index++;
		if(index == characterList.Length)
			index = 0;

		//Toggle on the new model
		characterList[index].SetActive(true);
	}

	//Changes scene after user selects a character. Spawns user in main world
	public void ConfirmButton()
	{
		PlayerPrefs.SetInt ("CharacterSelected", index);
		SceneManager.LoadScene ("3d test");
	}
}
