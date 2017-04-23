using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class heroface_image_script : MonoBehaviour {

	Image image;// = this.GetComponent<Image>();
	// Use this for initialization
	void Start () {

		/*Texture2D tex=Resources.Load("faceL/1-1") as Texture2D;

		//Image spr =this.GetComponent<Image>();

		Sprite sp = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值
		//spr.sprite = sp;
		//Image temp=this.GetComponent<Image>;
		*/
		//Image image = this.GetComponent<Image>();
		//image.sprite = sp;
		//image.overrideSprite = Global_source_loader.getInstance().hero_L_face[GlobalData.getInstance().nowheroid];
		//this.GetComponent<"Image">().overrideSprite = sprite;

		image = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		image.overrideSprite = Global_source_loader.getInstance().hero_L_face[GlobalData.getInstance().nowheroid];

	}
}
