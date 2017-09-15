using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinGroupController : MonoBehaviour
{

    private SoundCenter mSoundCenter;
    private List<Animation> mBinAnims;
    private int AnimNumber = -1;
    private AudioSource[] AudioSourceMemo;
    //To handle multi-recycle in the same time
    private Dictionary<int, float> mTimer;

    void Start()
    {
        Initialization();
        //Disable player interaction
        FindObjectOfType<Player>().enabled = false;
    }

    private void Initialization()
    {
        mSoundCenter = FindObjectOfType<SoundCenter>();
        mBinAnims = new List<Animation>();
        mBinAnims.Add(transform.FindChild("FoodBin").GetComponentInChildren<Animation>());
        mBinAnims.Add(transform.FindChild("GlasBin").GetComponentInChildren<Animation>());
        mBinAnims.Add(transform.FindChild("PaperBin").GetComponentInChildren<Animation>());
        mBinAnims.Add(transform.FindChild("PlasticBin").GetComponentInChildren<Animation>());
        AudioSourceMemo = new AudioSource[mBinAnims.Count];
        mTimer = new Dictionary<int, float>();
        for (int i = 0; i < AudioSourceMemo.Length; i++)
        {
            mTimer.Add(i, -1);
            AudioSourceMemo[i] = mSoundCenter.CreatSound();
        }
    }

    void ToggleRollInSound()
    {
        //Enable and disable AudioSource 
        mSoundCenter.SwitchMotionSituation();
    }

    void OpenBin()
    {
        OptimizeBins();
        OpenAndCloseBin();
    }

    private void OpenAndCloseBin()
    {
        BinsAnim("BinOpen");
        mSoundCenter.PlayOpeningSound();
        Invoke("CloseBin", 0.75f);
    }

    internal void OpenRelatedBin(string trashName)
    {
        SelectAnimNumber(trashName);
        mBinAnims[AnimNumber].Play("BinOpen");
        mSoundCenter.PlayOpeningSound(AudioSourceMemo[AnimNumber]);
    }

    internal void CloseRelatedBin(string trashName)
    {
        SelectAnimNumber(trashName);
        mBinAnims[AnimNumber].Play("BinClose");
        mSoundCenter.PlayClosingSound(AudioSourceMemo[AnimNumber]);
    }

    internal void CloseRelatedBin(int ainmNumber)
    {
        mBinAnims[ainmNumber].Play("BinClose");
        mSoundCenter.PlayClosingSound(AudioSourceMemo[ainmNumber]);
    }

    private void SelectAnimNumber(string trashName)
    {
        if (trashName.Contains("_food"))
        {
            AnimNumber = (int)ObjectInscriptionCenter.Bins.Food;
        }
        else if (trashName.Contains("_glass"))
        {
            AnimNumber = (int)ObjectInscriptionCenter.Bins.Glass;
        }
        else if (trashName.Contains("_paper"))
        {
            AnimNumber = (int)ObjectInscriptionCenter.Bins.Paper;
        }
        else if (trashName.Contains("_plastic"))
        {
            AnimNumber = (int)ObjectInscriptionCenter.Bins.Plastic;
        }
    }

    internal void Recycle(string trashName)
    {
        SelectAnimNumber(trashName);
        Invoke("PlayingRecyclingSound", 0.4f);        
        mTimer[AnimNumber] = Time.time + 0.5f;
    }
    private void PlayingRecyclingSound()
    {
        mSoundCenter.PlayRecyclingSound(AnimNumber);
    }
    void Update()
    {
        for (int i = 0; i < mTimer.Count; i++)
        {
            if (mTimer[i] > 0 && mTimer[i] < Time.time)
            {
                CloseRelatedBin(i);
                mTimer[i] = -1;
            }
        }
    }

    private static void BinsAnim(string clipName)
    {
        foreach (Animation anim in ObjectInscriptionCenter.GetInstance().TopAnimations)
        {
            anim.Play(clipName);
        }
    }

    private void CloseBin()
    {
        BinsAnim("BinClose");
        mSoundCenter.PlayClosingSound();
        Invoke("ActivePlayerInteraction", 0.75f);
    }
    private void ActivePlayerInteraction()
    {
        FindObjectOfType<Player>().enabled = true;
    }
    private void OptimizeBins()
    {
        //Optimize static meshes (bin bodies) with a fixed sprite
        foreach (SpriteRenderer sr in ObjectInscriptionCenter.GetInstance().Backgrounds)
        {
            sr.enabled = !sr.enabled;
        }
        //Disable bin bodies 
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            if (mr.name == "3D_housework_dustbin")
            {
                mr.enabled = false;
            }
        }
    }
}
