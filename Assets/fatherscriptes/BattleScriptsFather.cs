using UnityEngine;
using System.Collections;

public class BattleScriptsFather : MonoBehaviour {

	protected int old_flagID=Global_const.NONE_ID;
	public GameObject flagspirit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void ChangeFlag(int _flagID)
	{
		if (_flagID == Global_const.NONE_ID) {
			return;
		}

        flagspirit.GetComponent<SpriteRenderer>().sprite = Global_source_loader.getInstance().Flag_Sprite[_flagID];// Sprite.Create(tex,new Rect(0,6*Flagsprite.rect.height,Flagsprite.rect.width,Flagsprite.rect.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值


	}

}
