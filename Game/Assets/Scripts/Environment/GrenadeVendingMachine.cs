using UnityEngine;
using System.Collections;

public class GrenadeVendingMachine : Interactive
{
	public int grenadePrice;
	public int grenadeAmount;

	public override string message
	{
		get
		{
			string ret;
			CanBuy(out ret);
			return ret;
		}
	}
	
	public override void MomentaryAction()
	{
		string str;
		if (CanBuy(out str))
		{
			LaserRifle.instance.grenades += grenadeAmount;
			PlayerStats.instance.syf -= grenadePrice;
		}
	}
	
	private bool CanBuy(out string msg)
	{
		if (!LaserRifle.instance.hasGrenadeLaucherUpgrade)
		{
			msg = "You don't have grenade launcher";
			return false;
		}
		if (LaserRifle.instance.grenades >= LaserRifle.instance.maxGrenades)
		{
			msg = "Grenades full";
			return false;
		}
		else if (PlayerStats.instance.syf < grenadePrice)
		{
			msg = "Insufficient Syf";
			return false;
		}
		else
		{
			msg = string.Format("Buy {0} grenade for {1} Syf", grenadeAmount, grenadePrice);
			return true;
		}
	}
}
