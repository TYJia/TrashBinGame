using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{

    [SerializeField]
    private string mType;
    public bool mIsOnCorrectBin = false;
    private Vector3 EnterPoint;
    private float Timer = 0;
    private Vector3 OriginalPos;

    void Start()
    {
        if (transform.name.Contains("food"))
        {
            mType = "food";
        }
        else if (transform.name.Contains("glass"))
        {
            mType = "glass";
        }
        else if (transform.name.Contains("paper"))
        {
            mType = "paper";
        }
        else if (transform.name.Contains("plastic"))
        {
            mType = "plastic";
        }
        EnterPoint = GameObject.Find(mType + "Point").transform.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Contains(mType + "Collider"))
        {
            mIsOnCorrectBin = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name.Contains(mType + "Collider"))
        {
            mIsOnCorrectBin = false;
        }
    }
    internal void JumpIntoPoint()
    {
        OriginalPos = transform.position;
        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        InvokeRepeating("Motion", 0, 0.02f);
    }
    private void Motion()
    {
        Timer = Timer + 0.02f;
        float height = Mathf.Clamp(Mathf.Sin(Timer * 2 * 3.14f), 0, 2) * 1;
        transform.position = Vector3.Lerp(OriginalPos, EnterPoint, Timer * 2) + Vector3.up * height;
        if (Vector3.Distance(transform.position, EnterPoint) < 0.01)
        {
            gameObject.SetActive(false);
        }
    }
}
