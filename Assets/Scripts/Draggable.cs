using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Draggable : MonoBehaviour {

	public bool m_IsBeingDragged = false;
	private int m_NumOfCollidersInside = 0;

	/// <summary>
	/// checks to see if this object is inside another
	/// </summary>
	/// <returns>true if this object is inside another</returns>
	public bool isInCollider() {
		return m_NumOfCollidersInside != 0;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		m_NumOfCollidersInside++;
	}

	private void OnCollisionExit2D(Collision2D collision) {
		m_NumOfCollidersInside--;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		m_NumOfCollidersInside++;
	}

	private void OnTriggerExit2D(Collider2D collision) {
		m_NumOfCollidersInside--;
	}



}
