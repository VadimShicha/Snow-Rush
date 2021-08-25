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
	public Tilemap iceTilemap;

	public Tile snowTopTile;
    public Tile snowTile;
	public Tile iceTile;

	public GameObject flag;
	public GameObject saw;

	public float moveSpeed = 4;
	public float jumpHeight = 5;
	public float iceFriction = 0.8f;

	public float deathCheckerY = -5;

	bool grounded = false;
	bool died = false;
	bool onIce = false;
	bool won = false;

	void Start()
	{
		playerInstance = gameObject;
		generateLevel(5, VarManager.currentLevel);
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
			if(onIce == false)
			{
				Vector3 playerVel = player.GetComponent<Rigidbody2D>().velocity;

				player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, playerVel.y, playerVel.z);
			}
		}

		if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse2))
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
			die();
			died = true;
		}
	}

	public void die()
	{
		if(died == false)
		{
			print("You died!");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			died = true;
		}
	}

	void win()
	{
		//won used bc win function will get called twice
		if(won == false)
		{
			print("You win!");

			if(VarManager.currentLevel == VarManager.level)
			{
				VarManager.level++;
				VarManager.save();
			}

			VarManager.currentLevel++;

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

			won = true;
		}
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

			int randIceChance = rand.Next(3);
			int randIceSizeX = rand.Next(2) + 3;
			int randIceStartX = 0;

			int randSawChance = rand.Next(5);

			if(randIceChance == 1)
			{
				if((randSizeX - randIceSizeX) - 3 > 0)
				{
					randIceStartX = rand.Next((randSizeX - randIceSizeX) - 3) + 3;
				}

				//ice platform
				createPlatform(new Vector3Int(point.x + randIceStartX, point.y + randSizeY - 1, 0), point + new Vector3Int(randIceStartX + randIceSizeX, randSizeY, 0), iceTilemap, iceTile, iceTile);

				createPlatform(new Vector3Int(point.x + randIceStartX, point.y + randSizeY - 1, 0), point + new Vector3Int(randIceStartX + randIceSizeX, randSizeY, 0), snowTilemap, null, null);
			}

			if(randSawChance == 1)
			{
				GameObject sawClone = Instantiate(saw);
				sawClone.name = saw.name + "Clone";
				sawClone.transform.position = new Vector3(point.x + Mathf.RoundToInt(randSizeX / 2), point.y + randSizeY, 0);
			}
			
			
			createPlatform(point, point + new Vector3Int(randSizeX, randSizeY, 0), snowTilemap, snowTopTile, snowTile);
			
			//add ending
			if(i == platformAmount - 1)
			{
				//GameObject flagClone = Instantiate(flag);
				//flagClone.name = flag.name + "Clone";
				flag.transform.position = new Vector3(point.x + Mathf.RoundToInt(randSizeX / 2) - 1, point.y + randSizeY + 0.5f, point.z);
			}

			point.x += randDistanceX + randSizeX;
			point.y += randDistanceY + randSizeY;
		}
	}

	void createPlatform(Vector3Int pos1, Vector3Int pos2, Tilemap tilemap, Tile topTile, Tile tile)
	{
		for(int x = pos1.x; x < pos2.x; x++)
		{
			for(int y = pos1.y; y < pos2.y; y++)
			{
				if(y == pos2.y - 1)
				{
					tilemap.SetTile(new Vector3Int(x, y, 0), topTile);
				}
				else
				{
					tilemap.SetTile(new Vector3Int(x, y, 0), tile);
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

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject == iceTilemap.gameObject)
		{
			onIce = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(collision.gameObject == iceTilemap.gameObject)
		{
			onIce = false;
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
