using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewObjects : MonoBehaviour {

	public Transform m_ListOfSpots;

	private Draggable[] m_SpwanedObjects;
	private ObjList m_List;

	// Use this for initialization
	void Awake() {
		m_List = GetComponent<ObjList>();
		m_SpwanedObjects = new Draggable[m_ListOfSpots.childCount];
		for (int i = 0; i < m_List.m_ListOfObjects.Count; i++) {
			if(m_List.m_ListOfObjects[i] == null) {
				Debug.LogError("One of the ListOfObjects items are null");
			}
		}
	}

	private void Update() {
		for (int i = 0; i < m_SpwanedObjects.Length; i++) {
			if(m_SpwanedObjects[i] == null) {
				spwanObject(i);
			}
		}
	}

	private void spwanObject(int a_Slot) {
		GameObject go = new GameObject();

		go.transform.position = m_ListOfSpots.GetChild(a_Slot).position;

		go.AddComponent<SpriteRenderer>().sprite = m_List.getRandomObject();

		go.AddComponent<BoxCollider2D>();

		Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Kinematic;

		m_SpwanedObjects[a_Slot] = go.AddComponent<Draggable>();
	}

}
