using System;


public class Global_Skilldata
{
	private static  Global_Skilldata instance = new Global_Skilldata(); 

	private static string Global_SkilldataNamelist =
		"万人敌," +
		"无双," +
		"火神," +
		"飞将," +
		"捆绑," +
		"望梅止渴," +
		"霹雳车," +
		"一骑当千," +
		"遁逃," +
		"劫营," +
		"八阵图," +
		"十面埋伏," +
		"水军," +
		"山岳行军," +
		"威风," +
		"连锁," +
		"徽兵," +
		"巨贾," +
		"雄辩," +
		"仁德," +
		"奸雄," +
		"英主," +
		"商业," +
		"农业," +
		"能工," +
		"巧匠," +
		"隆中卧," +
		"识人," +
		"三姓家奴," +
		"反骨," +
		"霉运," +
		"庸碌," +
		"妄人," +
		"贪财";

	public string [] Global_SkilldataName = Global_SkilldataNamelist.Split(new char[] { ',' });

	private Global_Skilldata ()
	{


	}


	public static Global_Skilldata  getInstance() 
	{ 

		return instance; 
	}


	public void Update()
	{


	}
}


