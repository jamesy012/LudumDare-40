using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	private string m_StartingScoreText;
	public UnityEngine.UI.Text m_ScoreText;

	private int m_Score = 0;
	private int m_NumOfRequests = 0;

	public int m_RemoveForWrongItem = 10;
	public int m_RemoveForTimeUp = 20;
	public int m_AddForCorrectItem = 30;

	// Use this for initialization
	void Awake() {
		m_StartingScoreText = m_ScoreText.text;
		updateScore();
	}

	private void updateScore() {
		m_ScoreText.text = m_StartingScoreText.Replace("_SCORE_", m_Score.ToString()).Replace("_REQUESTS_", m_NumOfRequests.ToString());
	}

	public void addedWrongItem() {
		addScore(-m_RemoveForWrongItem);
	}

	public void addedRightItem() {
		addScore(m_AddForCorrectItem);
	}

	public void timerRunOut() {
		addScore(-m_RemoveForTimeUp);
	}

	public void addNewRequest() {
		m_NumOfRequests++;
		updateScore();
	}

	private void addScore(int a_Amount) {
		m_Score = Mathf.Max(m_Score + a_Amount, 0);
		updateScore();

		GameObject textObject = new GameObject();
		textObject.transform.parent = m_ScoreText.transform;
		textObject.transform.position = m_ScoreText.transform.position;
		UnityEngine.UI.Text text = textObject.AddComponent<UnityEngine.UI.Text>();
		text.font = m_ScoreText.font;
		text.fontSize = a_Amount > 0 ? 48 : 32;
		text.text = a_Amount.ToString();
		text.color = a_Amount > 0 ? Color.green : Color.red;
		TextMoveUp tmu = textObject.AddComponent<TextMoveUp>();
		tmu.m_MoveUpSpeed = 20.0f;
		tmu.m_SinXMovement = 20.0f;
		tmu.m_SinXTimeScale = 2.0f;
		tmu.m_TimeAlive = 2.0f;
	}
}
