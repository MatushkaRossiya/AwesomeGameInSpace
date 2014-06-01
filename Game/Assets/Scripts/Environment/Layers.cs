using UnityEngine;
using System.Collections;

public class Layers
{
    public static int playerAttack
    {
        get;
        private set;
    }

    public static int damage
    {
        get;
        private set;
    }

    public static int enemyAttack
    {
        get;
        private set;
    }

    public static int enemy
    {
        get;
        private set;
    }

	public static int environment {
		get;
		private set;
	}

    static Layers()
    {
        playerAttack = 
            1 << LayerMask.NameToLayer("Enemy") |
            1 << LayerMask.NameToLayer("Environment");
        damage =
            1 << LayerMask.NameToLayer("Player") |
            1 << LayerMask.NameToLayer("Enemy") |
            1 << LayerMask.NameToLayer("Environment");
        enemyAttack = 
            1 << LayerMask.NameToLayer("Player") |
            1 << LayerMask.NameToLayer("Environment");
        enemy =
			1 << LayerMask.NameToLayer("Enemy");
		environment =
			1 << LayerMask.NameToLayer("Environment") |
			1 << LayerMask.NameToLayer("Interactive");
    }
}
