using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCommon : MonoBehaviour {

	public void restartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
