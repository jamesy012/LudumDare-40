using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjList : MonoBehaviour {

	public List<Sprite> m_ListOfObjects;

	public Sprite getRandomObject() {
		return m_ListOfObjects[Random.Range(0, m_ListOfObjects.Count)];
	}

}
