using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;

public class herocreater_camera : MonoBehaviour {
	

	public GameObject messageBox;



	// Use this for initialization
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void init()
	{
		Global_XML_IO tempIO = Global_XML_IO.getInstance ();

	/*	XmlNodeList xmlNodeList =Global_XML_IO.getInstance().xml_herolist.SelectSingleNode("herolist").ChildNodes;

		foreach (XmlElement xl1 in xmlNodeList) {

			string 


		}
	*/
	/*	XmlNode xNode =tempIO.xml_herolist.SelectSingleNode("//herolist");
		XmlElement nowhero_Elem = (XmlElement)xNode.SelectSingleNode("//hero[@id='" + tempIO.now_IO_hero_ID + "']");
		if (nowhero_Elem != null)
		{
			string firstname = nowhero_Elem.GetAttribute("firstname");
			firstname_text.text = firstname;
		}
		else
		{
		}
	*/
	}

	public void SaveHeroList()
	{
        StartCoroutine(Global_XML_IO.getInstance().Save_hero_Xml());
        messageBox.GetComponent<messageboxscript> ().move ();
	}

	
	public void BackMain()
	{

		SceneManager.LoadScene("MainUI");

	}
}
