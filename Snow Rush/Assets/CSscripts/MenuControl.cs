using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
	void Awake()
	{
		VarManager.load();
	}

	public void playButtonClick()
	{
		SceneManager.LoadScene("LevelSelectScene");
	}
}
