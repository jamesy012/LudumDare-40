﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

	public GameObject m_DoorRotHolder;
	public SpriteRenderer m_RequiredObjHolder;
	public Transform m_CrossHolder;

	private ObjList m_ObjList;
	private DoorController m_DoorController;

	private float m_LastTime;
	public float m_TimeBetweenRequests = 5.0f;
	public float m_RandomOffset = 3.0f;

	private bool m_HasObject = false;

	public float m_TimeForTurn = 10.0f;
	private float m_TurnTimeStart = 0;
	public Color m_Col1;
	public Color m_Col2;
	public SpriteMask m_TimerMask;
	private SpriteRenderer m_TimerSpriteRenderer;

	private string m_StartingText;
	public TextMesh m_NameText;

	/// <summary>
	/// counter for the amount of errors the player has done this turn
	/// </summary>
	private int m_NumOfCrosses = 0;

	private void Awake() {
		m_ObjList = GetComponent<ObjList>();
		m_DoorController = FindObjectOfType<DoorController>();

		if (m_CrossHolder == null) {
			Debug.LogWarning("m_CrossHolder is null");
		}
		if (m_RequiredObjHolder == null) {
			Debug.LogWarning("m_RequiredObjHolder is null");
		}
		if (m_DoorRotHolder == null) {
			Debug.LogWarning("m_DoorRotHolder is null");
		}
		if (m_TimerMask == null) {
			Debug.LogWarning("m_TimerMask is null");
		}
		if (m_NameText == null) {
			Debug.LogWarning("m_NameText is null");
		}

		m_TimerSpriteRenderer = m_TimerMask.GetComponent<SpriteRenderer>();
		m_TimerMask.alphaCutoff = 1;

		m_StartingText = m_NameText.text;
		m_NameText.text = "Requests appear here:";

		setupNextTurn();
		m_LastTime = Time.time - m_TimeBetweenRequests/2;
	}

	private void Update() {
		if (!m_HasObject) {
			if(Time.time - m_LastTime > m_TimeBetweenRequests) {
				m_HasObject = true;
				getNextObject();
				m_DoorController.runDoorAnimation(true);
				m_TurnTimeStart = Time.time;
			}
		}else {
			float percentage = (Time.time - m_TurnTimeStart) / m_TimeForTurn;

			if(percentage > 1) {
				percentage = 1;
				setupNextTurn();
			}

			m_TimerMask.alphaCutoff = percentage;
			m_TimerSpriteRenderer.color = Color.Lerp(m_Col1, m_Col2, percentage);
		}
	}

	private void getNextObject() {
		m_RequiredObjHolder.sprite = m_ObjList.getRandomObject().GetComponent<SpriteRenderer>().sprite;

		//there is a chance they could have put another object in while the door is closing
		m_NumOfCrosses = 0;
		updateCrossUI();

		m_NameText.text = m_StartingText.Replace("_NAME_", ListOfNames.getRandomName());
	}

	private void setupNextTurn() {
		m_RequiredObjHolder.sprite = null;

		m_LastTime = Time.time + UnityEngine.Random.Range(-m_RandomOffset, m_RandomOffset);
		m_HasObject = false;
		m_NumOfCrosses = 0;
		updateCrossUI();
		m_DoorController.runDoorAnimation(false);

	}

	private void updateCrossUI() {
		for(int i = 0; i < m_CrossHolder.childCount; i++) {
			GameObject cross = m_CrossHolder.GetChild(i).GetChild(0).gameObject;
			if (i >= m_NumOfCrosses) {
				cross.SetActive(false);
			}else {
				cross.SetActive(true);
			}
		}
	}

	private void runLoseCheck() {
		if(m_NumOfCrosses >= m_CrossHolder.childCount) {
			setupNextTurn();
		}
	}

	public void objectSent(GameObject a_Object) {
		if(a_Object.GetComponent<SpriteRenderer>().sprite == m_RequiredObjHolder.sprite) {
			setupNextTurn();
		}else {
			m_NumOfCrosses++;
			updateCrossUI();
			runLoseCheck();
		}
	}



}
