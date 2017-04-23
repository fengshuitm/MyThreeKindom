using UnityEngine;
using System.Collections;

public class Global_fighter_Sprites  {

	public int fightstatue=0;//0 正在进行战斗 1 玩家胜（左） 2 电脑胜（右） 3 平局

	/*public int lefthero_might=89;//玩家武力
	public int righthero_might=91;//电脑武力
	*/
	public herodata lefthero;
	public herodata righthero;

	public int lefthero_hp=100;//玩家hp
	public int righthero_hp=100;//玩家hp


	private static  Global_fighter_Sprites instance = new Global_fighter_Sprites(); 

	private Global_fighter_Sprites(){
		lefthero = Global_HeroData.getInstance ().List_hero [100];
		righthero = Global_HeroData.getInstance ().List_hero [50];


	}

	public static Global_fighter_Sprites getInstance()
	{

		return instance;
	}

	public void Init(int might_left_id,int might_right_id)
	{


		lefthero =Global_HeroData.getInstance().List_hero[might_left_id];
		righthero = Global_HeroData.getInstance().List_hero[might_right_id];

		lefthero_hp = 100;
		righthero_hp = 100;

		fightstatue = 0;

	}
}
