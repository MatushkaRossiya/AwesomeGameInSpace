using UnityEngine;

public class BlockadeRepairer : Interactive
{
    Blockade blockade;
    public int buildCost;
    public int repairCost;
    public float repairAmount;
    
    void Start()
    {
        blockade = GetComponent<Blockade>();
    }

    public override string message
    {
        get
        {
            if (blockade.hitPointsPercentage <= 0)
            {
                if (PlayerStats.instance.syf >= buildCost)
                {
                    return "Build blockade for " + buildCost + " syf";
                }
                else
                {
                    return "Insufficient syf (Need " + buildCost + ")";
                }
            }
            else
            {
                if (PlayerStats.instance.syf >= repairCost)
                {
                    return "Repair blockade for " + repairCost + " syf";
                }
                else
                {
                    return "Insufficient syf (Need " + repairCost + ")";
                }
            }
        }
    }

    public override void Action()
    {
        if (blockade.hitPointsPercentage <= 0 && PlayerStats.instance.syf > buildCost)
        {
            blockade.Repair(1);
            PlayerStats.instance.syf -= buildCost;
        }
        else if (PlayerStats.instance.syf > repairCost)
        {
            blockade.Repair(repairAmount);
            PlayerStats.instance.syf -= repairCost;
        }
    }
}
