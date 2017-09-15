using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCenter : MonoBehaviour
{

    private AudioSource mAudioSource;
    private List<AudioSource> mCommonAudioSourcePool;
    private List<AudioSource> mReservedAudioSourcePool;
    private List<AudioClip> mSounds;

    void Start()
    {
        mSounds = ObjectInscriptionCenter.GetInstance().Sounds;
        mAudioSource = ObjectInscriptionCenter.GetInstance().AudioPlayer;

        mCommonAudioSourcePool = new List<AudioSource>();
        mCommonAudioSourcePool.Add(mAudioSource);

        mAudioSource.loop = false;
    }

    internal void SwitchMotionSituation()
    {
        mAudioSource.loop = !mAudioSource.loop;
        if (mAudioSource.loop == true)
        {
            mAudioSource.Play();
        }
        else
        {
            mAudioSource.Stop();
        }
    }

    internal void PlayOpeningSound(AudioSource audioSourceMemo)
    {
        PlayReservedSound(mSounds[4], audioSourceMemo);
    }

    internal void PlayWinMusic()
    {
        PlayCommonSound(mSounds[7]);
    }

    internal void PlayOpeningSound()
    {
        PlayCommonSound(mSounds[4]);
    }
    internal void PlayClosingSound(AudioSource audioSourceMemo)
    {
        PlayReservedSound(mSounds[5], audioSourceMemo);
    }
    internal void PlayClosingSound()
    {
        PlayCommonSound(mSounds[5]);
    }
    internal void PlayRecyclingSound(int clipNum)
    {
        PlayCommonSound(mSounds[clipNum]);
    }
    private void PlayReservedSound(AudioClip ac, AudioSource audioSourceMemo)
    {
        audioSourceMemo.Stop();
        audioSourceMemo.clip = ac;
        audioSourceMemo.Play();
    }
    private void PlayCommonSound(AudioClip ac)
    {
        AudioSource theAudio = mCommonAudioSourcePool[0];
        bool isFound = false;
        //Trying to find a free AudioSource
        for (int i = 0; i < mCommonAudioSourcePool.Count; i++)
        {
            if (mCommonAudioSourcePool[i].isPlaying == false)
            {
                theAudio = mCommonAudioSourcePool[i];
                isFound = true;
                break;
            }
        }
        //Create a new AudioSource if all are playing
        if (isFound == false)
        {
            theAudio = CreatSound();
            mCommonAudioSourcePool.Add(theAudio);
        }
        //Play
        theAudio.clip = ac;
        theAudio.Play();
    }

    internal void PlayPickUpSound()
    {
        PlayCommonSound(mSounds[6]);
    }

    internal AudioSource CreatSound()
    {
        AudioSource theAudio = gameObject.AddComponent<AudioSource>();
        theAudio.playOnAwake = false;
        return theAudio;
    }
    private AudioSource CreatReservedSound()
    {
        AudioSource theAudio = CreatSound();
        mReservedAudioSourcePool.Add(theAudio);
        return theAudio;
    }
}
