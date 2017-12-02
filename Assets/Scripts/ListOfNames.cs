using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfNames {

	public static string getRandomName() {
		return m_Names[Random.Range(0, m_Names.Length)];
	}

	//names from http://listofrandomnames.com/
	public static string[] m_Names = {
		"Tandra",
		"Tomeka",
		"Rachelle",
		"Charlyn",
		"Matthew",
		"Reena",
		"Dayle",
		"Jamila",
		"Darrin",
		"Nella",
		"Lavonne",
		"Roseanna",
		"Josef",
		"Anderson",
		"Earnest",
		"Frankie",
		"Pattie",
		"Elliott",
		"Effie",
		"Monique",
		"Aracely",
		"Graig",
		"Zoe",
		"Tanya",
		"Emmie",
		"Micheline",
		"Angla",
		"Sondra",
		"Brandon",
		"Rigoberto",
		"Nathaniel",
		"Tommy",
		"Edris",
		"Verline",
		"Teresia",
		"Gus",
		"Eldora",
		"Larry",
		"Addie",
		"Hester",
		"Ernestine",
		"Amal",
		"Elina",
		"Angie",
		"Lekisha",
		"Nakesha",
		"Caprice",
		"Vanesa",
		"Rickey",
		"Burl"};
}
