using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Inscription should be done in edit mode, by disable and enable this script
[ExecuteInEditMode]
public class ObjectInscriptionCenter : MonoBehaviour
{

    public bool AutoSet = false;

    #region Objects
    [Tooltip("BinGroup")]
    public GameObject BinGroup;
    [Tooltip("AudioSource in GameController")]
    public AudioSource AudioPlayer;
    [Tooltip("Backgrounds")]
    public SpriteRenderer[] Backgrounds;
    [Tooltip("Animations")]
    public List<Animation> TopAnimations;
    [Tooltip("Sounds During Game (Manual initialization)")]
    public List<AudioClip> Sounds;
    public enum Bins:int
    {
        Food = 0,
        Glass = 1,
        Paper = 2,
        Plastic = 3
    }
    #endregion

    [HideInInspector]
    public static ObjectInscriptionCenter mSelf;

    public static ObjectInscriptionCenter GetInstance()
    {
        return mSelf;
    }

    void OnEnable()
    {
        mSelf = this;

        if (AutoSet)
        {
            AutoSetMethods();
            AutoSet = false;
        }

        CheckNull();
    }

    private void CheckNull()
    {
        if (BinGroup == null)
        {
            Debug.LogError("Null Object Found, please check the tooltip and set it again");
        }
        if (AudioPlayer == null)
        {
            Debug.LogError("Null Object Found, please check the tooltip and set it again");
        }
        if (Backgrounds.Length == 0)
        {
            Debug.LogError("Null Object Found, please check the tooltip and set it again");
        }
        if (TopAnimations == null || TopAnimations[0] == null)
        {
            Debug.LogError("Null Object Found, please check the tooltip and set it again");
        }
        if (Sounds == null || Sounds[0] == null)
        {
            Debug.LogError("Null Object Found, please check the tooltip and set it again");
        }
    }

    private void AutoSetMethods()
    {
        if (BinGroup == null)
        {
            BinGroup = GameObject.Find("BinGroup");
        }
        if (AudioPlayer == null)
        {
            AudioPlayer = GetComponent<AudioSource>();
        }
        if (Backgrounds.Length == 0 || Backgrounds[0] == null)
        {
            Backgrounds = GameObject.Find("Backgrounds").transform.GetComponentsInChildren<SpriteRenderer>();
        }
        if (TopAnimations == null || TopAnimations[0] == null)
        {
            TopAnimations[(int)Bins.Food] = GameObject.Find("FoodBin").GetComponentInChildren<Animation>();
            TopAnimations[(int)Bins.Glass] = GameObject.Find("GlasBin").GetComponentInChildren<Animation>();
            TopAnimations[(int)Bins.Paper] = GameObject.Find("PaperBin").GetComponentInChildren<Animation>();
            TopAnimations[(int)Bins.Plastic] = GameObject.Find("PlasticBin").GetComponentInChildren<Animation>();
        }
    }

    // Use this for initialization
    void Awake()
    {
        mSelf = this;
        //mSelf.enabled = false;
    }
}
