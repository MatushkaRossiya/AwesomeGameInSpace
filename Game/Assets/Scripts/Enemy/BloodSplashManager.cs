using UnityEngine;
using System.Collections.Generic;

public class BloodSplashManager : MonoSingleton<BloodSplashManager>
{
    private Queue<GameObject> splashes;
    public int maxSplashCount = 32;
    
    // Use this for initialization
	BloodSplashManager()
    {
		splashes = new Queue<GameObject>(maxSplashCount);
    }

    public void AddSplash(GameObject splash)
    {
		if (splashes.Count >= maxSplashCount)
        {
			Destroy(splashes.Dequeue());
        }
		splashes.Enqueue(splash);
    }
}
