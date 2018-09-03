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

	private float CalculateManhattanDistance(Vector2 a, Vector2 b) {
		float mDistance = 0;
		mDistance += Mathf.Abs(b.x - a.x);
		mDistance += Mathf.Abs(b.y - a.y);

		return mDistance;
	}

	private IEnumerator MoveWithListRoutine(List<Vector2> movements) {
		foreach(Vector2 move in movements) {
			Move(move);
			yield return null;
		}
	}

	private class aStarNode {
		public Vector2 position;
		public float f;
		public aStarNode parent;
		public Vector2 movementFromParent;


		public aStarNode(Vector2 position, float f, aStarNode parent, Vector2 movementFromParent) {
			this.position = position;
			this.f = f;
			this.parent = parent;
			this.movementFromParent = movementFromParent;
		}
	}

	private void MakeListOfMovements(Vector2 startingPosition) {
		List<aStarNode> openList = new List<aStarNode>();
		List<aStarNode> closedList = new List<aStarNode>();
		openList.Add(new aStarNode(startingPosition, 0f, null, Vector2.zero));
		aStarNode theChosenOne = null;
		int step = 0;

		while(openList.Count > 0) {
			// get the node with BIGEST F on the list
			int selectedIndex = Random.Range(0, openList.Count);
			aStarNode selectedNode = openList[selectedIndex];
			openList.RemoveAt(selectedIndex);
			closedList.Add(selectedNode);

			// if it's not a wall then add the sucessors
			Collider2D collision = Physics2D.OverlapCircle(transform.position, .1f);
			Debug.Log(collision);

			if(collision == null) {
				foreach(Vector2 movement in possibleMovements) {
					openList.Add(new aStarNode(selectedNode.position + movement,
					CalculateManhattanDistance(m_dogReference.transform.position, selectedNode.position + movement), selectedNode, movement));
				}
			}

			step++;
			if(step >= 50) {
				break;
			}
		}

		// CREATING THE MOVEMENT LIST
		List<Vector2> movements = new List<Vector2>();
		bool canDo = true;

		theChosenOne = closedList[0];
		for(int i = 1; i < closedList.Count; i++) {
			if(closedList[i].f > theChosenOne.f) {
				theChosenOne = closedList[i];
			}
		}
		
		aStarNode node = theChosenOne;

		while(canDo) {
			movements.Insert(0, node.movementFromParent);
			
			if(node.parent != null) {
				node = node.parent;
			} else {
				canDo = false;
				break;
			}
		}

		StartCoroutine(MoveWithListRoutine(movements));
	}

	public void GetAwayFromDog() {
		StopAllCoroutines();
		MakeListOfMovements(transform.position);
		//StartCoroutine(GetAwayRoutine());
	}

	private void Move(Vector3 direction) {
		transform.position += direction;

		Collider2D collision = Physics2D.OverlapCircle(transform.position, .1f);
		
		if(collision != null) {
			if(collision.tag == "Walls") {
				transform.position -= direction;
			}
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
