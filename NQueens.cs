using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NQueens : MonoBehaviour {

	public int queenCount;//Number of Queens

	public float spaceBetweenQueens; //Space between Dodecahedron-Dodecahedron and Dodecahedron-Queen

	public Transform dodecahedron;//Dodecahedron Object

	public Transform queen;//Queen Object

	public Transform laser;//Laser Object

	public Transform pleseboPill;//PleseboPill for the player

	public LifeStone lifeStone;//LifeStone

	//[HideInInspector]
	public bool moveGrid = false;

	Stack<Vector2> stack;

	List<Position> positions;//Solution coordinates for given n queens

	List<Transform> queens;//Queens

	List<Vector3> emptyDodecahedronPositions;//all dodecahedrons

	Transform[,] dodecahedronMap;//Just chess table's positions
	
	Vector2 mapSize;

	int lifeStoneCount = 0;

	void Awake() {
		positions = new List<Position>();
		stack = new Stack<Vector2>();
		queens = new List<Transform>();
		emptyDodecahedronPositions = new List<Vector3>();
		mapSize = new Vector2(queenCount, queenCount);
		dodecahedronMap = new Transform[queenCount, queenCount];

	}
	void Start()
	{
		FindPositions();//Solves the n-queens
		GenerateDodecahedronsNQueensOrder();//Makes a table with dodecahedrons and puts queens propriate positions on table
		CreateWalls();//Creating walls with Dodecahedrons
		InvokeRepeating("SpawnRandomDodecahedrons", 1f, 1f);//Spawning dodecahedrons inside cage
		InvokeRepeating("SpawnRandomPleseboPills", 1f, .1f);//Spawning plesebopills inside cage
		CreateLaserGrid();//Creating Laser Grid for NQueens
	}

	void Update()
	{
		if (moveGrid)
		{
			Transform grid = transform.FindChild("LaserGridHolder");
			grid.position = Vector3.Lerp(GameObject.Find("BarrierDown").transform.localPosition,
										 GameObject.Find("BarrierUp").transform.localPosition, Mathf.PingPong(Time.time * .05f, 1f));
		}

	}

	void CreateLaserGrid()
	{
		string holderName = "LaserGridHolder";
		if (transform.FindChild(holderName))
		{
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}

		Transform laserGridHolder = new GameObject(holderName).transform;

		laserGridHolder.parent = transform;

		for(int i = 0; i < queens.Count; i++)
		{
				CreateLaserOnQueen(queens[i].transform.position , laserGridHolder);
		}
	}

	void CreateLaserOnQueen(Vector3 position , Transform holder)
	{
		for(int i = 0; i < 8; i++)
		{
			Transform laserObj = Instantiate(laser) as Transform;
			laserObj.position = position + Vector3.down * 120;
			laserObj.rotation = Quaternion.Euler(0, i * 45, 0);
			laserObj.parent = holder;
		}
	}

	//Spawning randomly dodecahedrons inside cage
	void SpawnRandomDodecahedrons()
	{
		int getIndex = Random.Range(0, emptyDodecahedronPositions.Count);

		Transform tempDodecahedron = Instantiate(dodecahedron) as Transform;

		tempDodecahedron.position = emptyDodecahedronPositions[getIndex];

		tempDodecahedron.parent = transform.FindChild("QueenHolder");

		Destroy(tempDodecahedron.gameObject, 3f);
	}

	//Spawning randomly plesebo pills inside cage
	void SpawnRandomPleseboPills()
	{
		Vector3 bossPos = GameObject.FindGameObjectWithTag("Boss").transform.position;

		Transform pill = Instantiate(pleseboPill, bossPos + Random.onUnitSphere * 500, Quaternion.identity) as Transform;

		pill.parent = transform.FindChild("QueenHolder");

		Destroy(pill.gameObject, 15f);
	}

	void GenerateDodecahedronsNQueensOrder()
	{

		string holderName = "QueenHolder";
		if (transform.FindChild(holderName))
		{
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}

		Transform queenHolder = new GameObject(holderName).transform;

		queenHolder.parent = transform; 

		for(int i = 0; i < mapSize.x; i++)
		{
			for(int j = 0; j < mapSize.y; j++)
			{
				//Creating Board With Dodecahedrons
				Vector3 dodecahedronPosition = new Vector3(-mapSize.x + 0.5f - i, 0, mapSize.y + 0.5f - j) * spaceBetweenQueens;

				dodecahedronMap[i, j] = Instantiate(dodecahedron) as Transform;

				dodecahedronMap[i, j].position = dodecahedronPosition;

				dodecahedronMap[i, j].parent = queenHolder;

				//Positioning Queens
				for (int k = 0; k < queenCount; k++)
				{
					if(positions[k].xPos == i && positions[k].yPos == j)
					{
						Transform queenObj = Instantiate(queen, dodecahedronMap[i, j].position + Vector3.up * spaceBetweenQueens * 0.7f,
														 Quaternion.identity) as Transform;

						queens.Add(queenObj);

						queenObj.parent = queenHolder;
					}
				}
			}
		}

	}

	void CreateWalls()
	{
		for(int y = 0; y < queenCount + 1; y++)
		{
			for (int x = -1; x < queenCount + 1; x++)
			{
				for (int z = -1; z < queenCount + 1; z++)
				{
					//LEFT RIGHT FORWARD BACKWARD WALLS
					if((x == -1 || z == -1 || x == queenCount || z == queenCount) && y != queenCount)
					{
						Vector3 wallDodecahedronPos = new Vector3(-queenCount + 0.5f - x , y , queenCount + 0.5f - z ) * spaceBetweenQueens;

						Transform wallDodecahedronObj = Instantiate(dodecahedron) as Transform;

						wallDodecahedronObj.position = wallDodecahedronPos;

						wallDodecahedronObj.parent = transform.FindChild("QueenHolder");

						CreateLifeStones(wallDodecahedronObj);

					}
					//CEILING WALL
					else if(y == queenCount)
					{
						Vector3 wallDodecahedronPos = new Vector3(-queenCount + 0.5f - x, y, queenCount + 0.5f - z) * spaceBetweenQueens;

						Transform wallDodecahedronObj = Instantiate(dodecahedron) as Transform;

						wallDodecahedronObj.position = wallDodecahedronPos;

						wallDodecahedronObj.parent = transform.FindChild("QueenHolder");

						CreateLifeStones(wallDodecahedronObj);

					}
					else
					{
						Vector3 emptyArea = new Vector3(-queenCount + 0.5f - x, y, queenCount + 0.5f - z) * spaceBetweenQueens;
						emptyDodecahedronPositions.Add(emptyArea);
					}

				}
			}
		}
	}

	void CreateLifeStones(Transform dodecahedron)
	{
		if (lifeStoneCount <= queenCount / 2)
		{
			int random = Random.Range(0, 1000);

			if (random > 900)
			{
				LifeStone lifeStoneObj = Instantiate(lifeStone) as LifeStone;
				lifeStoneObj.transform.position = dodecahedron.position + Vector3.up * 20;
				dodecahedron.GetComponent<Dodecahedron>().isContainsLifeStone = true;
				lifeStoneCount++;
			}
		}
	}

	//Solves n-queens
	void FindPositions()
	{
		Vector2 tempPos = new Vector2(0, 0);

		while(stack.Count != queenCount && IsInMapRange(tempPos))
		{
			if (CheckForValidPosition(tempPos))
			{
				stack.Push(tempPos);
				tempPos.x++;
				tempPos.y = 0;
			}
			else
			{
				if((int)tempPos.y < queenCount - 1)
				{
					tempPos.y++;
				}
				else{
					tempPos = stack.Pop();
					while((int)tempPos.y == queenCount - 1)
					{
						tempPos = stack.Pop();
					}
					tempPos.y++;
				}
			}
		}

		for(int i = 0; i < stack.Count; i++)
		{
			Position position = new Position((int)stack.ToArray()[i].x, (int)stack.ToArray()[i].y);
			positions.Add(position);
		}
		
	}
	//Controlling new Queen's position 
	bool CheckForValidPosition(Vector2 targetPos)
	{
		bool foundSafe = true;
		for(int index = 0;index < stack.Count; index++)
		{
			if(((int)stack.ToArray()[index].y == (int)targetPos.y) ||
			   (((int)stack.ToArray()[index].x - (int)stack.ToArray()[index].y) == (targetPos.x - targetPos.y)) ||
			   (((int)stack.ToArray()[index].x + (int)stack.ToArray()[index].y) == (targetPos.x + targetPos.y)))
			   {
				foundSafe = false;
				break;
			   }
		}

		return foundSafe;

	}
	//Controlling the giving coordinate is in table range ? 
	bool IsInMapRange(Vector2 pos)
	{
		return (int)pos.x >= 0 && (int)pos.x >= 0 && (int)pos.x < queenCount && (int)pos.x < queenCount;
	}


}

public class Position
{
	public int xPos, yPos;
	public Position(int x, int y)
	{
		xPos = x;
		yPos = y;
	}
}