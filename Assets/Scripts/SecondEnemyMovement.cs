using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemyMovement : MonoBehaviour {

	private Vector3[] possibleMovements = { Vector3.down, Vector3.left, Vector3.right, Vector3.up };
	private int m_stepsToMoveAway = 10;
	private float m_distanceToBeAway = 40f;
	private GameObject m_dogReference;

	void Start() {
		m_dogReference = GameObject.FindGameObjectWithTag("Player");
	}

	private float CalculateManhattanDistance(Vector3 a, Vector3 b) {
		float mDistance = 0;
		mDistance += Mathf.Abs(b.x - a.x);
		mDistance += Mathf.Abs(b.y - a.y);

		return mDistance;
	}

	private IEnumerator MoveWithListRoutine(List<Vector3> movements) {
		foreach(Vector3 move in movements) {
			Move(move);
			yield return null;
		}
	}

	private class aStarNode {
		public Vector3 position;
		public float f;
		public aStarNode parent;
		public Vector3 movementFromParent;
		public int step;


		public aStarNode(Vector3 position, float f, aStarNode parent, Vector3 movementFromParent, int step) {
			this.position = position;
			this.f = f;
			this.parent = parent;
			this.movementFromParent = movementFromParent;
			this.step = step;
		}
	}

	private void MakeListOfMovements(Vector3 startingPosition) {
		List<aStarNode> openList = new List<aStarNode>();
		List<Vector2> exploredPositions = new List<Vector2>();
		List<aStarNode> stepedList = new List<aStarNode>();
		openList.Add(new aStarNode(startingPosition, 0f, null, Vector3.zero, 0));

		aStarNode theChosenOne = null;
		int algorithmSteps = 0;

		while(openList.Count > 0) {
			// get the node with BIGEST F on the list
			int selectedIndex = 0;
			

			for(int i = 0; i < openList.Count; i++) {
				if(openList[i].f > openList[selectedIndex].f) {
					selectedIndex = i;
				}
			}

			aStarNode selectedNode = openList[selectedIndex];
			openList.RemoveAt(selectedIndex);
			exploredPositions.Add(selectedNode.position);
			// Debug.Log(selectedNode.position);

			// if it's not a wall then add the sucessors
			Collider2D collision = Physics2D.OverlapCircle(selectedNode.position, .1f);
			Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, .1f);

			bool hasWall = false;
			bool hasGround = false;
			foreach(Collider2D t_collision in collisions) {
				if(t_collision.tag == "Walls") {
					hasWall = true;
				} else if(t_collision.tag == "Ground") {
					hasGround = true;
				}
			}

			if(hasGround && !hasWall) {
				stepedList.Add(selectedNode);

				foreach(Vector3 movement in possibleMovements) {
					if(Physics2D.OverlapCircle(selectedNode.position + movement, .1f) != null) {
						if(Physics2D.OverlapCircle(selectedNode.position + movement, .1f).tag == "Ground") {
							if(!exploredPositions.Contains((selectedNode.position+movement))) {
							// Debug.Log("Pushing to the list: " + (selectedNode.position + movement));
							openList.Add(new aStarNode(selectedNode.position + movement,
							CalculateManhattanDistance(m_dogReference.transform.position, selectedNode.position + movement), selectedNode, movement, selectedNode.step+1));
							}
						}
					}
				}
			}

			algorithmSteps++;
			if(algorithmSteps > 50) {
				break;
			}
		}

		// CREATING THE MOVEMENT LIST
		List<Vector3> movements = new List<Vector3>();
		bool canDo = true;

		theChosenOne = stepedList[0];
		for(int i = 1; i < stepedList.Count; i++) {
			if(stepedList[i].f > theChosenOne.f) {
				theChosenOne = stepedList[i];
			}
		}
		
		aStarNode node = theChosenOne;

		while(canDo) {
			Vector3 movement = GetOpposite(node.movementFromParent);
			movements.Insert(0, node.movementFromParent);
			
			if(node.parent != null) {
				node = node.parent;
			} else {
				canDo = false;
				break;
			}
		}

		// // Debug.Log("movements to take");
		// for(int i = 0; i < movements.Count; i++) {
		// 	// Debug.Log(movements[i]);
		// }
		StartCoroutine(MoveWithListRoutine(movements));
	}

	private Vector3 GetOpposite(Vector3 move) {
		if(move == Vector3.up) {
			return Vector3.down;
		} else if(move == Vector3.right) {
			return Vector3.left;
		} else if(move == Vector3.down) {
			return Vector3.up;
		} else if(move == Vector3.left) {
			return Vector3.right;
		}

		return Vector3.zero;
	}

	public void GetAwayFromDog() {
		StopAllCoroutines();
		MakeListOfMovements(transform.position);
		//StartCoroutine(GetAwayRoutine());
	}

	private void Move(Vector3 direction) {
		transform.position += direction;
		Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, .1f);

		bool hasWall = false;
		foreach(Collider2D t_collision in collisions) {
			if(t_collision.tag == "Walls") {
				hasWall =true;
			}
		}

		if(hasWall) {
			transform.position -= direction;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow) ||
		   Input.GetKeyDown(KeyCode.UpArrow) ||
		   Input.GetKeyDown(KeyCode.RightArrow) ||
		   Input.GetKeyDown(KeyCode.DownArrow)) {
			Move(possibleMovements[Random.Range(0, possibleMovements.Length)]);
		   }
	}
}
