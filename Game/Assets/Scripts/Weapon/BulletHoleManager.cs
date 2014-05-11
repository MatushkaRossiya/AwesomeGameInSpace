using UnityEngine;
using System.Collections.Generic;

public class BulletHoleManager : MonoSingleton<BulletHoleManager>
{
    private Queue<GameObject> holes;
    public int maxHoleCount = 100;
    
    // Use this for initialization
    BulletHoleManager()
    {
        holes = new Queue<GameObject>(maxHoleCount);
    }

    public void AddBulletHole(GameObject hole)
    {
        if (holes.Count >= maxHoleCount)
        {
            Destroy(holes.Dequeue());
        }
        holes.Enqueue(hole);
    }
}
