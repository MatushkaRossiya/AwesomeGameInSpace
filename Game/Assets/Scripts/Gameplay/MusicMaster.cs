using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicMaster : MonoSingleton<MusicMaster>
{
    public AudioClip[] music;

    private float tipOffCountdown = 0.0f;
    private bool musicChange = false;
    private bool musicSet = false;
    private float volume1 = 0.0f;
    private float volume2 = 0.0f;
    private int musicId = -1;
    private float dayMusicTime = 0.0f;
    private bool resetAudioTime = false;

    // Use this for initialization
    void Start()
    {
        audio.loop = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (musicChange)
        {
            fadeOut();
            
            if (volume1 < 0.1f) 
            {
                if(musicSet == false)
                {
                    musicSet = true;
                    audio.Stop();
                    audio.clip = music[musicId];
                    if (resetAudioTime)
                    {
                        audio.time = 0.0f;
                    }
                    if (musicId == 0)
                    {
                        audio.time = dayMusicTime;
                        //Debug.Log(">>>>>>>>>> RESTORE DAY MUSIC TIME <<<<<<<<<<");
                        //Debug.Log(audio.time);
                    }
                    audio.Play();
                }

                if (volume2 < 1.0f) 
                {
                    fadeIn();
                }
                else
                {
                    musicChange = false;
                    musicSet = false;
                    volume1 = 1.0f;
                    volume2 = 0.0f;
                }
            }
        }

        if (musicId == 2)
        {
            if (tipOffCountdown <= 0.0f)
            {
                startExplorationMusic();
            }
            
            tipOffCountdown -= Time.deltaTime;
            if (tipOffCountdown < 0.0f)
                tipOffCountdown = 0.0f;
        }
    }

    public void startDayMusic()
    {
        if (musicId == 0)
            return;
        musicChange = true;
        musicSet = false;
        musicId = 0;
    }

    public void startExplorationMusic()
    {
        if (musicId == 1)
            return;
        if (musicId == 0)
        {
            dayMusicTime = audio.time;
            //Debug.Log(">>>>>>>>>> SET DAY MUSIC TIME <<<<<<<<<<");
            //Debug.Log(dayMusicTime);
            resetAudioTime = true;
        }
        musicChange = true;
        musicSet = false;
        musicId = 1;
    }

    public void startFightMusic()
    {
        if (musicId == 2)
        {
            tipOffCountdown += 5.0f;
            if (tipOffCountdown > 15.0f)
                tipOffCountdown = 15.0f;
            return;
        }
        if (musicId != 1)
        {
            //Podczas dnia nie odgrywamy
            return;
        }
        if (musicChange == true)
        {
            //Za wcześnie podczas zmiany
            return;
        }
        musicChange = false;
        musicSet = true;
        musicId = 2;
        tipOffCountdown += 15.0f;
        audio.Stop();
        audio.clip = music[musicId];
        audio.time = 40.0f;
        audio.Play();
        audio.volume = 1.0f;
    }

    private void fadeIn() {
        if (volume2 < 1.0f) {
            volume2 += 0.2f * Time.deltaTime;
            audio.volume = volume2;
        }
    }
    
    private void fadeOut() {
        if (volume1 > 0.0f)
        {
            volume1 -= 0.2f * Time.deltaTime;
            audio.volume = volume1;
        }
    }
}
