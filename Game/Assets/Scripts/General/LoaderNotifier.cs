using UnityEngine;
using System.Collections;

public class LoaderNotifier : MonoBehaviour {
	void Update()
	{
		if(!(Application.isLoadingLevel))
		{
			if(Loader.instance.saveToLoad != null)
			Loader.instance.continueLoading();
			this.StopAllCoroutines();
			Destroy(GameObject.FindObjectOfType<LoaderNotifier>());
		}
	}
}

