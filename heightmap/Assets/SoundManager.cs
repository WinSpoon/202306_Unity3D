using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AusioInfo
{
    public AudioClip clip;
    public string key;
}
 


//[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance = null;

    public static SoundManager GetInstance
    {
        get
        {
            if (!Instance)
            {
                GameObject obj = new GameObject("SoundManager");
                Instance = obj.AddComponent(typeof(SoundManager)) as SoundManager;
            }
            return Instance;
        }
    }

    //private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    public Dictionary<string, List<AusioInfo>> SoundList = new Dictionary<string, List<AusioInfo>>();
    //public List<AudioClip> SoundList = new List<AudioClip>();
    public string[] filePath = { "" };

    private void Start()
    {
        foreach (string path in filePath)
        {
            object[] Objects = Resources.LoadAll("" + path);

            for (int i = 0; i < Objects.Length; ++i)
            {
                AusioInfo obj = new AusioInfo();
                obj.clip = Objects[i] as AudioClip;
                obj.key = Objects[i].ToString();


                List<AusioInfo> temp = new List<AusioInfo>();
                temp.Add(obj);
                SoundList.Add(path, temp);
            }
        }
    }

    /*
    void PlaySound()
    {
        AudioSource source = new AudioSource();
        source.clip = SoundList[0];

        source.Play();
        source.Stop();
    }
     */
}
