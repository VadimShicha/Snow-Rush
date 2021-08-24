using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectControl : MonoBehaviour
{
	public GameObject player;

	public const int LEVELSPERPAGE = 15;
	public const int PAGES = 5 - 1;

	public GameObject[] levels = new GameObject[LEVELSPERPAGE];

	int currentPage = 0;

	void Start()
	{
		displayPage();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			changePage(-1);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			changePage(1);
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("MenuScene");
		}
	}
	/// <summary>
	/// -1 = lastPage, 1 = nextPage
	/// </summary>
	public void changePage(int axis)
	{
		if(currentPage >= 0 || currentPage < PAGES)
		{
			//last page
			if(axis < 0)
			{
				if(currentPage > 0)
				{
					currentPage += axis;
				}
				//goto last page
				else
				{
					currentPage = PAGES;
				}
			}
			//next page
			else if(axis > 0)
			{
				if(currentPage < PAGES)
				{
					currentPage += axis;
				}
				else
				{
					currentPage = 0;
				}
			}

			displayPage();
		}
	}

	void displayPage()
	{
		for(int i = 0; i < LEVELSPERPAGE; i++)
		{
			GameObject levelLock = levels[i].transform.Find("Lock").gameObject;
			Button button = levels[i].transform.Find("LevelButton" + (i + 1)).GetComponent<Button>();
			TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();

			if(VarManager.level + 1 >= (currentPage * LEVELSPERPAGE) + i)
			{
				text.text = ((currentPage * LEVELSPERPAGE) + (i + 1)).ToString();
				
				int index = i;
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => {levelButtonClick((currentPage * LEVELSPERPAGE) + index); });

				text.gameObject.SetActive(true);
				button.interactable = true;
				levelLock.SetActive(false);
			}
			else
			{
				text.gameObject.SetActive(false);
				button.interactable = false;
				levelLock.SetActive(true);
			}
		}
	}

	void levelButtonClick(int index)
	{
		print(index + 1);
		VarManager.levelSeed = index;
		SceneManager.LoadScene("SampleScene");
	}

	public void currentLevelButtonClick()
	{
		levelButtonClick(VarManager.level + 1);
	}
}
