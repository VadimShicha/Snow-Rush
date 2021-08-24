using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
	public GameObject player;
	public static GameObject playerInstance;

	public LayerMask groundMask;
	public Transform groundChecker;
	public float groundRadius = 0.4f;

	public Tilemap snowTilemap;

	public Tile snowTopTile;
    public Tile snowTile;

	public GameObject flag;

	public float moveSpeed = 4;
	public float jumpHeight = 5;

	public float deathCheckerY = -5;

	bool grounded = false;
	bool died = false;
	int currentLevel = 0;


	void Start()
	{
		playerInstance = gameObject;
		generateLevel(5, VarManager.levelSeed);
	}

	void Update()
	{
		float xAxisInput = Input.GetAxisRaw("Horizontal");

		if(Physics2D.OverlapCircle(groundChecker.position, groundRadius, groundMask) != null)
			grounded = true;
		else
			grounded = false;
		

		if(xAxisInput != 0)
		{
			Vector3 playerVel = player.GetComponent<Rigidbody2D>().velocity;

			player.GetComponent<Rigidbody2D>().velocity = new Vector3(xAxisInput * moveSpeed, playerVel.y, playerVel.z);

			if(xAxisInput > 0)
				player.GetComponent<SpriteRenderer>().flipX = false;
			else if(xAxisInput < 0)
				player.GetComponent<SpriteRenderer>().flipX = true;
		}
		else
		{
			Vector3 playerVel = player.GetComponent<Rigidbody2D>().velocity;

			player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, playerVel.y, playerVel.z);
		}

		if(Input.GetKey(KeyCode.Space))
		{
			if(grounded == true)
			{
				Vector3 playerVel = player.GetComponent<Rigidbody2D>().velocity;

				player.GetComponent<Rigidbody2D>().velocity = new Vector3(playerVel.x, jumpHeight, playerVel.z);
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("LevelSelectScene");
		}

		//death checker
		if(player.transform.position.y <= deathCheckerY)
		{
			if(died == false)
			{
				died = true;
				die();
			}
		}
	}

	void die()
	{
		print("You died!");
	}

	void win()
	{
		print("You win!");
		VarManager.level = VarManager.levelSeed;
		VarManager.levelSeed = VarManager.level;
		VarManager.save();

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void loadLevel(int seed)
	{
		generateLevel(20, seed);
	}

	void generateLevel(int platformAmount, int seed)
	{
		//the current position of the platform creator (Z arg is useless)
		Vector3Int point = Vector3Int.zero;

		System.Random rand = new System.Random(seed);

		for(int i = 0; i < platformAmount; i++)
		{
			int randDistanceX = rand.Next(4) + 2;
			int randDistanceY = rand.Next(4) - 2;

			int randSizeX = rand.Next(12) + 4;
			int randSizeY = rand.Next(2) + 1;

			createPlatform(point, point + new Vector3Int(randSizeX, randSizeY, 0));

			//add ending
			if(i == platformAmount - 1)
			{
				//GameObject flagClone = Instantiate(flag);
				//flagClone.name = flag.name + "Clone";
				flag.transform.position = new Vector3(point.x + Mathf.RoundToInt(randSizeX / 2), point.y + randSizeY + 0.5f, point.z);
			}

			point.x += randDistanceX + randSizeX;
			point.y += randDistanceY + randSizeY;
		}
	}

	void createPlatform(Vector3Int pos1, Vector3Int pos2)
	{
		for(int x = pos1.x; x < pos2.x; x++)
		{
			for(int y = pos1.y; y < pos2.y; y++)
			{
				if(y == pos2.y - 1)
				{
					snowTilemap.SetTile(new Vector3Int(x, y, 0), snowTopTile);
				}
				else
				{
					snowTilemap.SetTile(new Vector3Int(x, y, 0), snowTile);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision == flag.GetComponent<Collider2D>())
		{
			win();
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(groundChecker.position, groundRadius);
	}
	/*
	float hash(float key)
	{
		float value = key;

		value /= key + 0.4f;
		value *= 4.3f;

		int lastValue = Mathf.RoundToInt(value);

		for(int i = 0; i < lastValue; i++)
		{
			value -= key / 0.2f;
			value += key * 0.4f;
			value *= 1.64f;
		}

		value *= 0.5f;
		value += 1.4f;

		return Mathf.Abs(value);
	}
	*/
}
