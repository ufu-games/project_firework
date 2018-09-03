using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour {

	public int initialMaskSize = 6;
	public int expandedMaskSize = 12;
	public Transform maskObject;
	private AudioSource m_audioSource;
	private GameObject m_enemyReference;

	void Start() {
		m_audioSource = GetComponent<AudioSource>();
		m_enemyReference = GameObject.FindGameObjectWithTag("Enemy");
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

	private IEnumerator BarkRoutine() {
		maskObject.localScale = new Vector3(expandedMaskSize, expandedMaskSize, expandedMaskSize);
		yield return new WaitForSeconds(0.1f);
		maskObject.localScale = new Vector3(initialMaskSize, initialMaskSize, initialMaskSize);
	}
	
	void Bark() {
		m_audioSource.Play();

		if(Vector2.Distance(m_enemyReference.transform.position, transform.position) < ((expandedMaskSize / 2) + 1)) {
			// Debug.Log(Vector2.Distance(m_enemyReference.transform.position, transform.position));
			// Debug.Log(expandedMaskSize);
			Debug.Log("Saw Enemy!");
			m_enemyReference.GetComponent<SecondEnemyMovement>().GetAwayFromDog();
		}
		StartCoroutine(BarkRoutine());
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			Move(Vector3.up);
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			Move(Vector3.right);
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			Move(Vector3.down);
		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			Move(Vector3.left);
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			Bark();
		}
	}
}
