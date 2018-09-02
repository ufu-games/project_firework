using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemyMovement : MonoBehaviour {

	private Vector3[] possibleMovements = { Vector3.down, Vector3.left, Vector3.right, Vector3.up };

	private void GetAwayFromDog() {
		// take X amount of movements
		// all movements maximize the distance from the player
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
