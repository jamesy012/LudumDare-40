﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewObjects : MonoBehaviour {

	public Transform m_ListOfSpots;

	private Draggable[] m_SpwanedObjects;
	private ObjList m_List;

	public float m_TimeBetweenNewItems = 5.0f;
	private float m_LastSpwanTime = 0;

	public bool m_IsMainMenu = false;

	// Use this for initialization
	void Awake() {
		m_List = GetComponent<ObjList>();
		m_SpwanedObjects = new Draggable[m_ListOfSpots.childCount];
		for (int i = 0; i < m_List.m_ListOfObjects.Count; i++) {
			if(m_List.m_ListOfObjects[i] == null) {
				Debug.LogError("One of the ListOfObjects items are null");
			}
		}
		m_LastSpwanTime = -m_TimeBetweenNewItems * 2;
	}

	private void Update() {
		if (Time.time - m_LastSpwanTime > m_TimeBetweenNewItems) {
			for (int i = 0; i < m_SpwanedObjects.Length; i++) {
				if (m_SpwanedObjects[i] == null || m_IsMainMenu) {
					spwanObject(i);
				}
			}
		}
	}

	private void spwanObject(int a_Slot) {
		m_LastSpwanTime = Time.time;
		GameObject go = new GameObject();

		//give the object a name, cause it's funny and so it's easy to tell the difference between them
		go.name = "OBJ " + ListOfNames.getRandomName();
		go.layer = LayerMask.NameToLayer("NewObject");

		go.transform.position = m_ListOfSpots.GetChild(a_Slot).position;

		go.AddComponent<SpriteRenderer>().sprite = m_List.getRandomObject();

		go.AddComponent<PolygonCollider2D>();

		Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
		rb.angularVelocity = Random.Range(-500, 500);
		//rb.bodyType = RigidbodyType2D.Kinematic;

		m_SpwanedObjects[a_Slot] = go.AddComponent<Draggable>();

	}

}
