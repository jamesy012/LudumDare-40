using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMoveUp : MonoBehaviour {

	private Vector3 m_StartPos;
	private float m_StartTime;
	public float m_SinXMovement = 10;
	public float m_SinXTimeScale = 0.5f;
	public float m_MoveUpSpeed = 1.0f;

	public float m_TimeAlive = 5.0f;

	private UnityEngine.UI.Text m_TextObject;
	private Color m_StartingColor;
	private Color m_Desiredolor;

	// Use this for initialization
	void Start () {
		m_StartTime = Time.time;
		m_StartPos = transform.position;
		m_TextObject = GetComponent<UnityEngine.UI.Text>();
		m_StartingColor = m_TextObject.color;

		m_Desiredolor = m_StartingColor;
		m_Desiredolor.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float timeSinceStart = Time.time - m_StartTime;

		transform.position = m_StartPos + new Vector3(
			Mathf.Sin(timeSinceStart * m_SinXTimeScale) * m_SinXMovement,
			m_MoveUpSpeed * timeSinceStart,
			0);

		float percentage = timeSinceStart / m_TimeAlive;

		if(percentage > 1.0f) {
			Destroy(gameObject);
		}

		m_TextObject.color = Color.Lerp(m_StartingColor, m_Desiredolor, percentage);
	}
}
