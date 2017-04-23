using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Global_source_loader
{
	public Sprite [] hero_L_face=new Sprite[Global_const.getInstance().MAXHEROS];
	public Sprite [] Event_Sprite=new Sprite[500];

	public Sprite [] Back_List=new Sprite[Global_const.getInstance().MAXBACKS];


	public Sprite[] Flag_Sprite = new Sprite[Global_const.getInstance ().MAXKINDOM_COUNT];

    public Sprite[] ImportanceList = new Sprite[20];

    private static  Global_source_loader instance = new Global_source_loader(); 

	//public SceneManager.GetSceneByName("fight");
	public Scene [] SceneList = new Scene[10];

    /// <summary>
    /// facecreator
    /// </summary>
    public Sprite[] Add_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Armor_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Back_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Beard_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Eye_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Eyebrow_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Hair_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Head_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Mouse_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Moustache_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Nose_Sprite = new Sprite[Global_const.MAXFACERESOURCE];
    public Sprite[] Weapon_Sprite = new Sprite[Global_const.MAXFACERESOURCE];

    public Sprite None_Sprite = new Sprite();

    private Global_source_loader ()
	{
		//SceneManager.GetSceneByName("fight");
		for (int i = 0; i < Global_const.getInstance ().MAXHEROS; i++) {

			string tempstr = "faceS/" + i + "-1";
			Texture2D tex=Resources.Load(tempstr) as Texture2D;
			if (tex == null) {

				continue;
			}

			hero_L_face[i]=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值

			//Sprite sp = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值
			//spr.sprite = sp;
			//Image temp=this.GetComponent<Image>;
			//Image image = this.GetComponent<Image>();
			//image.sprite = sp;
			//image.overrideSprite = sp;

		}

		for (int j = 0; j < 500; j++) {
			int numtemp = j;
			string tempstr = "Events/" + numtemp + "-1";
			Texture2D tex=Resources.Load(tempstr) as Texture2D;
			if (tex == null) {

				continue;
			}

			Event_Sprite[j]=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值


        }

		for (int i = 0; i < Global_const.getInstance ().MAXKINDOM_COUNT; i++) {

			Texture2D tex=Resources.Load("spirite/flags/flag_"+i) as Texture2D;
			if (tex == null) {

				continue;
			}
			float flagwidth = tex.width / 4;
			float flagheight = tex.height / 8;

			Flag_Sprite[i]=Sprite.Create(tex,new Rect(0,7*flagheight+5,flagwidth,flagheight-5),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值
			
		}

		for (int i = 0; i < Global_const.getInstance ().MAXBACKS; i++) {

			Texture2D tex=Resources.Load("backgrounds/GrpCity_"+i+"-1") as Texture2D;
			if (tex == null) {

				continue;
			}

			Back_List[i]=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值


		}

        for (int i = 0; i < 20; i++)
        {

            Texture2D tex = Resources.Load("importance/importance" + i) as Texture2D;
            if (tex == null)
            {
                continue;
            }

            ImportanceList[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值


        }
        /*Texture2D tex=Resources.Load("faceL/1-1") as Texture2D;

		//Image spr =this.GetComponent<Image>();

		Sprite sp = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值
		//spr.sprite = sp;
		//Image temp=this.GetComponent<Image>;
		Image image = this.GetComponent<Image>();
		//image.sprite = sp;
		image.overrideSprite = sp;
		*/

        for(int i=0;i<Global_const.MAXFACERESOURCE;i++)
        {
            Texture2D tex = Resources.Load("facecreator/add/add_" + i) as Texture2D;
            if (tex != null)
            {
                Add_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/armor/armor_" + i) as Texture2D;
            if (tex != null)
            {
                Armor_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/back/back_" + i) as Texture2D;

            //tex.LoadImage(getim "facecreator/back/back_" + i,true);

            if (tex != null)
            {
                Back_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/beard/beard_" + i) as Texture2D;
            if (tex != null)
            {
                Beard_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/eye/eye_" + i) as Texture2D;
            if (tex != null)
            {
                Eye_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/eyebrow/eyebrow_" + i) as Texture2D;
            if (tex != null)
            {
               Eyebrow_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/hair/hair_" + i) as Texture2D;
            if (tex != null)
            {
                Hair_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/head/head_" + i) as Texture2D;
            if (tex != null)
            {
                Head_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/mouse/mouse_" + i) as Texture2D;
            if (tex != null)
            {
                Mouse_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/moustache/moustache_" + i) as Texture2D;
            if (tex != null)
            {
                Moustache_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/nose/nose_" + i) as Texture2D;
            if (tex != null)
            {
               Nose_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

            tex = Resources.Load("facecreator/weapon/weapon_" + i) as Texture2D;
            if (tex != null)
            {
                Weapon_Sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }

        }

        {
            Texture2D tex = Resources.Load("facecreator/none/none") as Texture2D;
            if (tex != null)
            {
                None_Sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
            }
            

        }

    }

    public static Global_source_loader  getInstance() 
	{

		return instance;
	}

    private static byte[] getImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);

        byte[] imageByte = new byte[files.Length];
        files.Read(imageByte, 0, imageByte.Length);
        files.Close();

        return imageByte;
    }
}


