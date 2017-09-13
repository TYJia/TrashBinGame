using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinGroupController : MonoBehaviour {

    private SoundCenter mSoundCenter;
    private AudioSource mASMemo;

    void Awake ()
    {
        mSoundCenter = FindObjectOfType<SoundCenter>();
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
