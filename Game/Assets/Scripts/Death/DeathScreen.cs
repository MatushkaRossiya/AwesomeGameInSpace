using UnityEngine;
using System.Collections;

public class DeathScreen : MonoBehaviour
{
    //if you're gonna die, die with your boots on

    public Texture[] textures;
    private bool showButtons = false;
    private GUITexture gui;

    void Awake()
    {
        gui = GetComponent<GUITexture>();
        int what = Random.Range(0, textures.Length);
        gui.texture = textures [what];
        gui.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void Start()
    {
        StartCoroutine(fadeIn());
        StartCoroutine(buttonWait());
    }

    void OnGUI()
    {
        if (showButtons)
        {
            if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.33f, 200, 100), "Play again (if you dare)"))
                Application.LoadLevel(1);
            if (GUI.Button(new Rect(3 * Screen.width * 0.2f, Screen.height * 0.33f, 200, 100), "Go to Menu"))
                Application.LoadLevel(0);
        }
    }

    IEnumerator buttonWait()
    {
        yield return new WaitForSeconds(1.5f);
        Screen.lockCursor = false;
        //Screen.showCursor = true;
        showButtons = true;
        yield return null;
    }

    IEnumerator fadeIn()
    {
        for (int i = 0; i < 151; ++i)
        {
            Color c = gui.color;
            c.a = (i / 150.0f);
            gui.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}
