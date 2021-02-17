using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string audioName;
    public AudioClip audio;
}
public class AudioManager : Singleton<AudioManager>
{
    static Dictionary<string, AudioClip> soundsDict = new Dictionary<string, AudioClip>();
    public Sounds[] miscellaneousSounds;
    static AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (soundsDict.Count == 0)
        {
            for (int i = 0; i < miscellaneousSounds.Length; i++)
            {
                soundsDict.Add(miscellaneousSounds[i].audioName, miscellaneousSounds[i].audio);
            }
        }

    }
    private void OnEnable()
    {
        EventManager.StartListening("Level Won", PlayWinSound);
        EventManager.StartListening("Level Lost", PlayLoseSound);
    }



    private void OnDisable()
    {
        EventManager.StopListening("Level Won", PlayWinSound);
        EventManager.StopListening("Level Lost", PlayLoseSound);
    }

    private void PlayWinSound()
    {
    //    PlaySound("Win");
    }

    private void PlayLoseSound()
    {
     //   PlaySound("Lose");
    }

    public void PlaySound(string whatToPlay)
    {
        source.PlayOneShot(soundsDict[whatToPlay]);
    }


}
