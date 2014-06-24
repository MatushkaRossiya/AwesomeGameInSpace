using UnityEngine;
using System.Collections;

public class TrapInteractive : Interactive {

    public Trap trap;

    private int TrapActivationPrice
    {
        get
        {
            return trap.price;
        }
    }


    public override string message
    {
        get
        {
            if (trap.used)
            {
                if (PlayerStats.instance.syf >= TrapActivationPrice) return "Hold E to activate trap for " + TrapActivationPrice.ToString() + " Syf";
                else return "Insufficient Syf";

            }
            else if (!trap.used && !trap.triggered) return "Trap is ready";
            else return null;
        }
    }


    //public override void MomentaryAction()
    //{
    //    if(PlayerStats.instance.syf >= TrapActivationPrice)
    //    {
    //        PlayerStats.instance.syf -= TrapActivationPrice;
    //        trap.used = false;
    //        StartCoroutine(trap.activate());
    //    }
    //}


    public override void HoldAction()
    {
        if (PlayerStats.instance.syf >= TrapActivationPrice)
        {
            PlayerStats.instance.syf -= TrapActivationPrice;
            trap.used = false;
            StartCoroutine(trap.activate());
        }
    }

}
