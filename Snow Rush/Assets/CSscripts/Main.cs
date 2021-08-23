using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Main : MonoBehaviour
{
	public GameObject player;

	public LayerMask groundMask;
	public Transform groundChecker;
	public float groundRadius = 0.4f;

	public Tilemap snowTilemap;

	public Tile snowTopTile;
    public Tile snowTile;

	public float moveSpeed = 4;
	public float jumpHeight = 5;

	bool grounded = false;

	void Start()
	{
		generateLevel(50, 15);
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

		if(Input.GetKey(KeyCode.Space))
		{
			if(grounded == true)
			{
				Vector3 playerVel = player.GetComponent<Rigidbody2D>().velocity;

				player.GetComponent<Rigidbody2D>().velocity = new Vector3(playerVel.x, jumpHeight, playerVel.z);
			}
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

			createPlatform(point, point + new Vector3Int(randSizeX, randSizeY, 0));

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

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(groundChecker.position, groundRadius);
	}

	//adds one to second arg
	/*
	//Example:
	//rand(1, 10);
	//will return a number 1 through 10 not 1 through 9
	*/
	int rand(int min, int max)
	{
		return Random.Range(min, max + 1);
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
