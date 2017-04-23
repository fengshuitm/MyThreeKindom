using UnityEngine;
using System.Collections;


public class Global_const {

	public int MAXMATS=10;//配偶最大数
	public int MAXFEIENDS=300;//好友人数
	public int MAXRIGHTEOUSBROTHERS=10;//义兄弟最大数

	public int MAXHEROS=2000;//英雄总数
	public int MAXBACKS=1000;//英雄总数

    public const int MAXFRIENDLIKE = 100;//英雄总数

    public const int MAXARMYHERONO = 5;//部队英雄总数


    public const int MAXIMPORTANCE=200;//据点总数

	public int MAXFAMILY=100;//family总数
	
	public int MAXSKILLTYPE_COUNT=50;//总技能类型数
	public int MAXSKILL_COUNT=6;//单人技能最大数
	public int MAX_S_SKILL_COUNT=2;//单人S技能最大数

	public int BGINSILL_COUNT=5;
	
	public int MAXKINDOM_COUNT=50;//势力总数

    public const int MAXFACERESOURCE = 50;//资源数

    public int GrownUp=15;
	public const int NONE_ID=-1;//无效ID

	public const int BIGNO=100000;//BIG
	public const int SMALLNO =-100000;//BIG

	public int BATTLE_WORKS_NUM=4;

	//public const float CellSizeX =0.16f;
    //public const float CellSizeY = 0.16f;

    enum family_enum {FAMILY_BROTHER,FAMILY_SWORN_BROTHER,FAMILY_MATE,FAMILY_FATHER,FAMILY_MOTHER,FAMILY_FATHER_INLAW,FAMILY_MOTHER_INLAW,FAMILY_OTHERS};
	
	private static  Global_const instance = new Global_const(); 
	private Global_const(){} 
	
	  public static Global_const  getInstance() 
	  { 
		  
	   return instance; 
	  }


}
