using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewObjects : MonoBehaviour {

	public Transform m_ListOfSpots;
	public List<Draggable> m_ListOfObjects;

	private Draggable[] m_SpwanedObjects;

	// Use this for initialization
	void Awake() {
		m_SpwanedObjects = new Draggable[m_ListOfSpots.childCount];

	}

	private void Update() {
		for (int i = 0; i < m_SpwanedObjects.Length; i++) {
			if(m_SpwanedObjects[i] == null) {
				spwanObject(i);
			}
		}
	}

	private void spwanObject(int a_Slot) {
		m_SpwanedObjects[a_Slot] = Instantiate(m_ListOfObjects[Random.Range(0, m_ListOfObjects.Count)], m_ListOfSpots.GetChild(a_Slot).position, Quaternion.identity);

		Rigidbody2D rb2d = m_SpwanedObjects[a_Slot].GetComponent<Rigidbody2D>();
		rb2d.angularVelocity = 0;
		rb2d.velocity = Vector2.zero;
		rb2d.bodyType = RigidbodyType2D.Kinematic;
	}

}
