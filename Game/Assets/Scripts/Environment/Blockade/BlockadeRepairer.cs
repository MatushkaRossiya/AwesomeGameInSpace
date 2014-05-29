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
                    return "Insufficient syf (" + buildCost + " needed)";
                }
            }
            else if (blockade.hitPointsPercentage < 100)
            {
                if (PlayerStats.instance.syf >= repairCost)
                {
                    return "Repair blockade for " + repairCost + " syf";
                }
                else
                {
                    return "Insufficient syf (" + repairCost + " needed)";
                }
            }
            else 
            {
                return "";
            }
        }
    }

    public override void Action()
    {
        if (blockade.hitPointsPercentage <= 0 && PlayerStats.instance.syf >= buildCost)
        {
            blockade.Repair(1);
            PlayerStats.instance.syf -= buildCost;
        }
        else if (PlayerStats.instance.syf >= repairCost)
        {
            blockade.Repair(repairAmount);
            PlayerStats.instance.syf -= repairCost;
        }
    }
}
