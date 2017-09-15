using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    private int mTotalTrashNumber;
    [SerializeField]
    internal int mRecycledTrashNumber = 0;
    private ParticleSystem mWinParticle;
    private SoundCenter mSoundCenter;

    // Use this for initialization
    void Start () {
        mSoundCenter = FindObjectOfType<SoundCenter>();
        mTotalTrashNumber = ObjectInscriptionCenter.GetInstance().TrashGroup.childCount;
        mWinParticle = ObjectInscriptionCenter.GetInstance().WinParticle;

    }
	
    internal void AddOnePoint()
    {
        mRecycledTrashNumber++;
        IfWin();
    }

    private void IfWin()
    {
        if (mRecycledTrashNumber == mTotalTrashNumber)
        {
            mWinParticle.Play();
            mSoundCenter.PlayWinMusic();
            Invoke("StopParticle", 3f);
        }
    }

    private void StopParticle()
    {        
        mWinParticle.Stop();
    }
}
