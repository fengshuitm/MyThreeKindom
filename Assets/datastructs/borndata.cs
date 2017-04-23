using System;

public class borndata
{
	private static string born_list =
		"皇室,"+
		"豪族,"+
		"寒族,"+
		"教徒,"+
		"农民,"+
		"工人,"+
		"商人,"+
		"流民";

	public string [] born_listName = born_list.Split(new char[] { ',' });

	public borndata ()
	{
	}

}

