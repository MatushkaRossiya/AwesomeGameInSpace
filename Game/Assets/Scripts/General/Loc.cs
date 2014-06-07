using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Loc : MonoSingleton<Loc>
{
    public enum Language {
        ENGLISH,
        POLISH
    }
    
    static TextAsset _locFile;
    static Dictionary<string, string> _texts;
    static Language _currentLanguage;
    
    public override void Init()
    {
        if (PlayerPrefs.HasKey("language")) {
            Language language = (Language)PlayerPrefs.GetInt("language");
            setLanguage(language);
        }
        else {
            SystemLanguage language = Application.systemLanguage;
            switch(language) {
                case SystemLanguage.English:
                    setLanguage(Language.ENGLISH);
                    break;
                case SystemLanguage.Polish:
                    setLanguage(Language.POLISH);
                    break;
                default:
                    setLanguage(Language.ENGLISH);
                    break;
            }
        }
    }
    
    public string getText(string id) 
    {
        if (_texts != null)
        {
            return _texts [id];
        }
        else
        {
            return "Loc error";
        }
    }
    
    public void setLanguage(Language language) {
        string fileName = "English";
        
        switch(language) {
            case Language.ENGLISH:
                fileName = "English";
                _currentLanguage = Language.ENGLISH;
                break;
            case Language.POLISH:
                fileName = "Polish";
                _currentLanguage = Language.POLISH;
                break;
            default:
            {
                fileName = "English";
                _currentLanguage = Language.ENGLISH;
                language = Language.ENGLISH;
                break;
            }
        }
        
        _locFile = Resources.Load(fileName) as TextAsset;
        if (_locFile == null) return;
        
        StringReader textReader = new StringReader(_locFile.text);
        
        _texts = new Dictionary<string, string>();
        string line;
        while ((line = textReader.ReadLine()) != null) {
            string[] pair = line.Split(' ');
            string text = line.Substring(line.IndexOf(' ') + 1);
            _texts.Add(pair[0], text);
        }
        
        PlayerPrefs.SetInt("language", (int)language);
    }
    
    public Language getLanguage() {
        return _currentLanguage;
    }
}