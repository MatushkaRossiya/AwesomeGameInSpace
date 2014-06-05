using UnityEngine;
using System.Collections.Generic;

public class ExplosionMarkManager : MonoSingleton<ExplosionMarkManager>
{
    private Queue<GameObject> marks;
    public int maxMarkCount = 16;
    
    // Use this for initialization
    ExplosionMarkManager()
    {
        marks = new Queue<GameObject>(maxMarkCount);
    }
    
    public void AddExplosionMark(GameObject mark)
    {
        if (marks.Count >= maxMarkCount)
        {
            Destroy(marks.Dequeue());
        }
        marks.Enqueue(mark);
    }
}
