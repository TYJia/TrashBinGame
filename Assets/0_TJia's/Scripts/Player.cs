using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mMainCam;
    private RaycastHit mHitInfo;
    private Transform mSelectedObj;
    private BinGroupController mBinGroupController;
    private Ray mMouseRay;
    private Trash mCurrentTrash;
    private LevelController mLevelController;
    private SoundCenter mSoundCenter;

    void Start()
    {
        mSoundCenter = FindObjectOfType<SoundCenter>();
        mMainCam = ObjectInscriptionCenter.GetInstance().MainCamera;
        mBinGroupController = ObjectInscriptionCenter.GetInstance().BinGroup.GetComponent<BinGroupController>();
        mLevelController = FindObjectOfType<LevelController>();
    }

    void Update()
    {
        mMouseRay = mMainCam.ScreenPointToRay(Input.mousePosition);
        Select();
        Unselect();
        PosFollower();
    }

    private void PosFollower()
    {
        if (mSelectedObj && Physics.Raycast(mMouseRay, out mHitInfo, 20))
        {
            Vector3 target = mHitInfo.point;
            //Limitation
            target.x = Mathf.Clamp(target.x, 1.2f, 4.0f);
            target.z = Mathf.Clamp(target.z, -1.8f, 1.5f);

            Vector3 distance = target - mSelectedObj.position;
            if (distance.magnitude > 0.1f)
            {
                mSelectedObj.position = Vector3.Lerp(mSelectedObj.position, target, 0.1f);
            }
        }
    }

    //Pickup trashes
    private void Select()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mMouseRay, out mHitInfo, 20))
            {
                if (mHitInfo.transform.name.Contains("3D_recycle_trash_"))
                {
                    ReleaseObj();
                    mSelectedObj = mHitInfo.transform;
                    mSelectedObj.gameObject.layer = 2;
                    mBinGroupController.OpenRelatedBin(mSelectedObj.name);
                    mCurrentTrash = mSelectedObj.GetComponent<Trash>();
                    mSoundCenter.PlayPickUpSound();
                }
            }
        }
    }

    private void Unselect()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (mSelectedObj && mCurrentTrash.mIsOnCorrectBin)
            {
                Recycle();
            }
            else
            {
                ReleaseObj();
            }
        }
    }

    private void Recycle()
    {        
        mSelectedObj.GetComponent<Trash>().JumpIntoPoint();
        mBinGroupController.Recycle(mSelectedObj.name);
        mSelectedObj = null;
        mLevelController.AddOnePoint();
    }

    private void ReleaseObj()
    {
        if (mSelectedObj)
        {
            mSelectedObj.gameObject.layer = 0;
            mBinGroupController.CloseRelatedBin(mSelectedObj.name);
            mSelectedObj = null;
        }
    }
}
