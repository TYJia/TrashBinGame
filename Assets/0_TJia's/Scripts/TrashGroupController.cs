using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGroupController : MonoBehaviour
{

    private List<Transform> mTrashes;

    void Start()
    {
        mTrashes = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            mTrashes.Add(transform.GetChild(i));
        }

        InvokeRepeating("PosCorrection", 0, 0.1f);
    }

    private void PosCorrection()
    {
        for (int i = 0; i < mTrashes.Count; i++)
        {
            Vector3 position = mTrashes[i].position;
            position.x = Mathf.Clamp(position.x, 1.2f, 4.0f);
            position.y = Mathf.Clamp(position.y, 2.2f, 3.7f);
            position.z = Mathf.Clamp(position.z, -1.8f, 1.5f);
            mTrashes[i].position = position;
        }
    }
}
