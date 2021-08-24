using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarManager
{
    public static int levelSeed;
	public static int level; //starts at 0

	public static void save()
	{
		PlayerPrefs.SetInt("Level", level);
	}

	public static void load()
	{
		if(PlayerPrefs.HasKey("Level"))
		{
			level = PlayerPrefs.GetInt("Level");
		}
	}
}