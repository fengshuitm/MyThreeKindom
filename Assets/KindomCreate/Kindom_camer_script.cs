using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;


public class Kindom_camer_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackMain()
	{

		SceneManager.LoadScene("MainUI");

	}
}
