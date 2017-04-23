using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainUI : UI_OBJ {

	// Use this for initialization
	void Start () {
        this.move();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GotoScene(string _Scene)
	{

		SceneManager.LoadScene(_Scene);

	}

}
