using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

	public GameObject m_DoorRotHolder;
	public Transform m_CrossHolder;

	public Transform m_RequiredItemsHolder;

	private SpriteRenderer[] m_SpriteHolders;
	private bool m_MultiChoice = false;
	private bool[] m_HasGotten = new bool[2] { false, false};

	private ObjList m_ObjList;
	private DoorController m_DoorController;

	private float m_LastTime;
	public float m_TimeBetweenRequests = 5.0f;
	public float m_RandomOffsetSub = 3.0f;
	public float m_RandomOffsetPos = 3.0f;

	private bool m_HasObject = false;

	public float m_TimeForTurn = 10.0f;
	public float m_TimeForMultiTurn = 20.0f;
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
		if (m_RequiredItemsHolder == null) {
			Debug.LogWarning("m_RequiredObjHolder is null");
		}
		if(m_RequiredItemsHolder.childCount != 2) {
			Debug.LogWarning("m_RequiredObjHolder should have 2 children");
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

		m_SpriteHolders = new SpriteRenderer[3];
		m_SpriteHolders[0] = m_RequiredItemsHolder.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
		m_SpriteHolders[1] = m_RequiredItemsHolder.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
		m_SpriteHolders[2] = m_RequiredItemsHolder.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>();

		removeAllMarks();
	}

	private void Update() {
		if (!m_HasObject) {
			if(Time.time - m_LastTime > m_TimeBetweenRequests) {
				getNextObject();

			}
		}else {
			float timerForTurn = m_MultiChoice ? m_TimeForMultiTurn : m_TimeForTurn;
			float percentage = (Time.time - m_TurnTimeStart) / timerForTurn;

			if(percentage > 1) {
				percentage = 1;
				setupNextTurn();
				m_Sm.timerRunOut();
				crossUnWonMarks();
			}

			m_TimerSpriteRenderer.color = Color.Lerp(m_Col1, m_Col2, percentage);
			if(percentage == 1) {
				percentage = 1 - percentage;
			}
			m_TimerMask.alphaCutoff = percentage;
		}
	}

	private bool getRandomObjectInLevel() {
		Pickupable[] objects = FindObjectsOfType<Pickupable>();
		if(objects.Length < 2) {
			m_SpriteHolders[0].sprite = m_SpriteHolders[1].sprite = m_SpriteHolders[2].sprite = null;
			return false;
		}
		if(objects.Length > 10) {
			m_SpriteHolders[1].sprite = objects[UnityEngine.Random.Range(0, objects.Length)].GetComponent<SpriteRenderer>().sprite;
			m_SpriteHolders[2].sprite = objects[UnityEngine.Random.Range(0, objects.Length)].GetComponent<SpriteRenderer>().sprite;

			if (m_SpriteHolders[1].sprite != m_SpriteHolders[2].sprite) {
				m_MultiChoice = true;
				m_HasGotten[0] = m_HasGotten[1] = false;
				return true;
			}

			m_SpriteHolders[1].sprite = m_SpriteHolders[2].sprite = null;
		}
		m_SpriteHolders[0].sprite = objects[UnityEngine.Random.Range(0, objects.Length)].GetComponent<SpriteRenderer>().sprite;
		m_MultiChoice = false;
		return true;
	}

	private void getNextObject() {
		removeAllMarks();
		m_SpriteHolders[0].sprite = m_SpriteHolders[1].sprite = m_SpriteHolders[2].sprite = null;

		bool didSetNewSprite = getRandomObjectInLevel();

		if (!didSetNewSprite) {
			setupNextTurn();
			return;
		}

		m_HasObject = true;

		m_Sm.addNewRequest();

		//there is a chance they could have put another object in while the door is closing
		m_NumOfCrosses = 0;
		updateCrossUI();

		m_TurnTimeStart = Time.time;

		m_DoorController.runDoorAnimation(true);
		m_RequestingText.text = m_StartingRequestText.Replace("_NAME_", ListOfNames.getRandomName());
	}

	private void setupNextTurn() {

		m_LastTime = Time.time + UnityEngine.Random.Range(-m_RandomOffsetSub, m_RandomOffsetPos);
		m_HasObject = false;
		//m_NumOfCrosses = 0;
		//updateCrossUI();
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

	private void removeAllMarks() {
		for (int i = 0; i < m_SpriteHolders.Length; i++) {
			removeMarks(m_SpriteHolders[i]);
		}
	}

	private void crossUnWonMarks() {
		if (m_MultiChoice) {
			updateMarks(m_SpriteHolders[1], m_HasGotten[0]);
			updateMarks(m_SpriteHolders[2], m_HasGotten[1]);
		}else {
			updateMarks(m_SpriteHolders[0], false);
		}
	}

	private void removeMarks(SpriteRenderer a_RequiredObj) {
		a_RequiredObj.transform.GetChild(0).gameObject.SetActive(false);
		a_RequiredObj.transform.GetChild(1).gameObject.SetActive(false);
	}

	private void updateMarks(SpriteRenderer a_RequiredObj, bool a_Correct) {
		a_RequiredObj.transform.GetChild(0).gameObject.SetActive(a_Correct);
		a_RequiredObj.transform.GetChild(1).gameObject.SetActive(!a_Correct);
	}

	private void runLoseCheck() {
		if(m_NumOfCrosses >= m_CrossHolder.childCount) {
			if (m_HasObject) {
				crossUnWonMarks();
			}
			setupNextTurn();
		}
	}

	public void objectSent(GameObject a_Object) {
		bool correct = false;
		if (m_HasObject) {
			if (m_MultiChoice) {
				Sprite spr = a_Object.GetComponent<SpriteRenderer>().sprite;
				if (!m_HasGotten[0]) {
					if (spr == m_SpriteHolders[1].sprite) {
						correct |= true;
						m_HasGotten[0] = true;
						updateMarks(m_SpriteHolders[1], true);
					}
				}
				if (!m_HasGotten[1]) {
					if (spr == m_SpriteHolders[2].sprite) {
						correct |= true;
						m_HasGotten[1] = true;
						updateMarks(m_SpriteHolders[2], true);
					}
				}
			} else {
				correct = a_Object.GetComponent<SpriteRenderer>().sprite == m_SpriteHolders[0].sprite;
				if (correct) {
					updateMarks(m_SpriteHolders[0], true);
				}
			}
		}
		m_Feedback.addFeedback(a_Object.GetComponent<SpriteRenderer>().sprite, correct);
		if (correct) {
			if (m_MultiChoice) {
				if(m_HasGotten[0] && m_HasGotten[1]) {
					setupNextTurn();
				}
			} else {
				setupNextTurn();
			}
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
