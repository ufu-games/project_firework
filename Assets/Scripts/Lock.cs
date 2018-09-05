using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {

	public GameObject door;
	private bool m_isOpen;
	
	void Start () {
		door.SetActive(false);	
		m_isOpen = true;
	}

	public void PressLock() {
		if(m_isOpen) {
			door.SetActive(true);
		} else {
			door.SetActive(false);
		}

		m_isOpen = !m_isOpen;
	}
}
