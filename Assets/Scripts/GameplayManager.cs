using System;
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
	public float m_RandomOffsetSub = 3.0f;
	public float m_RandomOffsetPos = 3.0f;

	private bool m_HasObject = false;

	public float m_TimeForTurn = 10.0f;
	private float m_TurnTimeStart = 0;
	public Color m_Col1;
	public Color m_Col2;
	public SpriteMask m_TimerMask;
	private SpriteRenderer m_TimerSpriteRenderer;

	private string m_StartingRequestText;	
	[UnityEngine.Serialization.FormerlySerializedAs("m_NameText")]
	public TextMesh m_RequestingText;

	private CameraShake m_CameraShake;
	private FeedbackController m_Feedback;

	private ScoreManager m_Sm;

	/// <summary>
	/// counter for the amount of errors the player has done this turn
	/// </summary>
	private int m_NumOfCrosses = 0;

	private void Awake() {
		m_ObjList = GetComponent<ObjList>();
		m_Sm = GetComponent<ScoreManager>();
		m_DoorController = FindObjectOfType<DoorController>();
		m_CameraShake = FindObjectOfType<CameraShake>();
		m_Feedback = FindObjectOfType<FeedbackController>();

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
		if (m_RequestingText == null) {
			Debug.LogWarning("m_NameText is null");
		}
		if (m_CameraShake == null) {
			Debug.LogWarning("m_CameraShake is null");
		}
		if (m_Feedback == null) {
			Debug.LogWarning("m_Feedback is null");
		}

		m_TimerSpriteRenderer = m_TimerMask.GetComponent<SpriteRenderer>();
		m_TimerMask.alphaCutoff = 1;

		m_StartingRequestText = m_RequestingText.text;
		m_RequestingText.text = "Requests appear here:";

		setupNextTurn();
		m_LastTime = Time.time - m_TimeBetweenRequests/2;
	}

	private void Update() {
		if (!m_HasObject) {
			if(Time.time - m_LastTime > m_TimeBetweenRequests) {
				m_HasObject = true;
				getNextObject();

			}
		}else {
			float percentage = (Time.time - m_TurnTimeStart) / m_TimeForTurn;

			if(percentage > 1) {
				percentage = 1;
				setupNextTurn();
				m_Sm.timerRunOut();
			}

			m_TimerMask.alphaCutoff = percentage;
			m_TimerSpriteRenderer.color = Color.Lerp(m_Col1, m_Col2, percentage);
		}
	}

	private Sprite getRandomObjectInLevel() {
		Pickupable[] objects = FindObjectsOfType<Pickupable>();
		if(objects.Length <= 4) {
			return null;
		}
		return objects[UnityEngine.Random.Range(0, objects.Length)].GetComponent<SpriteRenderer>().sprite;
	}

	private void getNextObject() {
		m_RequiredObjHolder.sprite = getRandomObjectInLevel();

		if (m_RequiredObjHolder.sprite == null) {
			setupNextTurn();
			return;
		}

		m_Sm.addNewRequest();

		//there is a chance they could have put another object in while the door is closing
		m_NumOfCrosses = 0;
		updateCrossUI();

		m_TurnTimeStart = Time.time;

		m_DoorController.runDoorAnimation(true);
		m_RequestingText.text = m_StartingRequestText.Replace("_NAME_", ListOfNames.getRandomName());
	}

	private void setupNextTurn() {
		m_RequiredObjHolder.sprite = null;

		m_LastTime = Time.time + UnityEngine.Random.Range(-m_RandomOffsetSub, m_RandomOffsetPos);
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
		bool correct = a_Object.GetComponent<SpriteRenderer>().sprite == m_RequiredObjHolder.sprite;
		m_Feedback.addFeedback(a_Object.GetComponent<SpriteRenderer>().sprite, correct);
		if (correct) {
			setupNextTurn();
			m_Sm.addedRightItem();
		} else {
			m_Sm.addedWrongItem();
			m_CameraShake.startShake();
			m_NumOfCrosses++;
			updateCrossUI();
			runLoseCheck();
		}
	}



}
