using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	
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
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			Move(Vector3.down);
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			Move(Vector3.left);
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			Move(Vector3.up);
		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			Move(Vector3.right);
		}
	}
}
