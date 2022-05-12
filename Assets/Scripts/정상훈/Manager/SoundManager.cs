using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public static SoundManager singleTon;

    [SerializeField]
    AudioSource screamSource;
    [SerializeField]
    AudioSource fireWork;
    [SerializeField]
    AudioSource crashSound;

    [SerializeField]
    AudioSource conquestSource;
    
    public AudioMixer master;
    private float volume = 0;
    private void Awake()
    {
        singleTon = this;
    }

    void Update()
    {
        volume_changer();
        
    }
    void volume_changer()
    {
        if(Input.GetKeyDown(KeyCode.Plus)||Input.GetKeyDown(KeyCode.KeypadPlus))
            volume += 10f;
        else if(Input.GetKeyDown(KeyCode.Minus)||Input.GetKeyDown(KeyCode.KeypadMinus))
            volume -= 10f;
        volume = Mathf.Clamp(volume,-40f,0f);
        Debug.Log(volume);
        master.SetFloat("Master",volume);
    }

    public void FireWorkSoundPlay()
    {
        fireWork.Play();
    }

    public void FireWorkSoundStop()
    {
        fireWork.Stop();
    }

    public void CrashSoundPlay()
    {
        crashSound.Play();
    }

    public void ScreamSoundPlay()
    {
        if(screamSource.isPlaying==false)
            screamSource.Play();
    }

    public void ConquestSoundPlay()
    {
        if (conquestSource.isPlaying == false)
            conquestSource.Play();
    }
}
