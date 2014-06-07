using UnityEngine;
using System.Collections;

public class LoaderNotifier : MonoBehaviour
{
    void FixedUpdate()
    {
        if (!Application.isLoadingLevel)
        {
            if (Loader.isLoading)
                Loader.instance.continueLoading();
            this.StopAllCoroutines();
            //Destroy(GameObject.FindObjectOfType<LoaderNotifier>());
            Destroy(this);
        }
    }
}

