using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	private Rigidbody2D m_Rb;
	private SpriteRenderer m_Sr;
	private int m_PlayerLayerMask = 0;

	public float m_JumpScale = 12.0f;
	public float m_HorizontalMovementScale = 5.0f;

	private bool m_IsOnGround = false;
	private bool m_IsOnWall = false;
	private int m_NumJumpsUsed = 0;
	public int m_MaxNumOfJumps = 2;

	private Animator m_Animator;

	private void Awake() {
		m_Rb = GetComponent<Rigidbody2D>();
		m_Sr = GetComponent<SpriteRenderer>();
		m_Animator = GetComponent<Animator>();

		m_PlayerLayerMask = ~ (1<<LayerMask.NameToLayer("Player"));

		m_Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	// Update is called once per frame
	void Update () {
		if (PauseHandler.m_IsPaused) {
			return;
		}
		groundCheck();

		//todo add wall jumps
		float horizontalMovment = 0;
		float verticalMovment = 0;
		bool didMove = false;
		if (Input.GetKey(KeyCode.D)) {
			horizontalMovment += Vector2.right.x * m_HorizontalMovementScale;
			m_Sr.flipX = true;
			didMove = true;
		}
		if (Input.GetKey(KeyCode.A)) {
			horizontalMovment += Vector2.left.x * m_HorizontalMovementScale;
			m_Sr.flipX = false;
			didMove = true;
		}
		m_Animator.SetBool("Run", didMove);

		if (Input.GetKeyDown(KeyCode.W) && m_NumJumpsUsed != m_MaxNumOfJumps) {
			m_NumJumpsUsed++;
			//wall jump
			if (m_IsOnWall) {
				RaycastHit2D jumpRh2d = Physics2D.Raycast(transform.position, new Vector3(horizontalMovment * Time.deltaTime, 0, 0), 0.4f, m_PlayerLayerMask);
				verticalMovment += 2.0f;
				if (jumpRh2d) {
					horizontalMovment = Mathf.Sign(horizontalMovment) * -20;					
				}else {
					horizontalMovment *= 5;
				}
			}

			m_Rb.AddForce(Vector2.up * m_JumpScale, ForceMode2D.Impulse);
		}

		Vector2 vel = m_Rb.velocity;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(horizontalMovment * Time.deltaTime,0,0), 0.4f, m_PlayerLayerMask);
		m_Animator.SetBool("OnWall", hit);
		if (hit) {
			horizontalMovment *= 0.5f;
			verticalMovment += -0.2f;
		}
		vel.x += horizontalMovment;
		vel.x = Mathf.Clamp(vel.x, -5, 5);
		vel.y += verticalMovment;
		vel.y = Mathf.Clamp(vel.y, -15, 15);
		m_Rb.velocity = vel;

	}

	void groundCheck() {
		float YOffset = 0.6f;
		Debug.DrawRay(transform.position + new Vector3(0.4f,-YOffset, 0), (Vector3.down + new Vector3(-2,0,0)).normalized,Color.red);
		Debug.DrawRay(transform.position + new Vector3(-0.4f,-YOffset, 0), (Vector3.down + new Vector3(2,0,0)).normalized,Color.red);
		RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.4f, -YOffset, 0), (Vector3.down + new Vector3(-2, 0, 0)).normalized, 1, m_PlayerLayerMask);
		RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(-0.4f, -YOffset, 0), (Vector3.down + new Vector3(2, 0, 0)).normalized, 1, m_PlayerLayerMask);
		//print(hit.transform);
		m_IsOnGround = hit.transform != null || hit2.transform != null;
		m_IsOnWall = (hit.transform != null ^ hit2.transform != null) || hit.transform == hit2.transform;

		m_Animator.SetBool("Jump", !m_IsOnGround);

		RaycastHit2D simpleCheck = Physics2D.Raycast(transform.position + new Vector3(0, -YOffset,0), m_Rb.velocity, 0.5f, m_PlayerLayerMask);
		if (simpleCheck.transform != null) {
			m_NumJumpsUsed = 0;
		}
	}
}
