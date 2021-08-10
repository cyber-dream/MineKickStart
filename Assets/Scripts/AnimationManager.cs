using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private PlayableAsset wallPostiveAnim;
    [SerializeField] private PlayableAsset wallNegativeAnim;
    private bool wallState;

    [SerializeField] private GameObject membrana;
    [SerializeField] private PlayableAsset membaranaPostiveAnim;
    [SerializeField] private PlayableAsset membaranaNegativeAnim;
    private bool membranaState;

    void Start()
    {
        
    }

    public void wallButton()
    {
        print(wallState);
        if (!wallState)
        {
            wall.GetComponent<PlayableDirector>().playableAsset = wallPostiveAnim;
            wall.GetComponent<PlayableDirector>().Play();
            wallState = true;

        }
        else
        {
            wall.GetComponent<PlayableDirector>().playableAsset = wallNegativeAnim;
            wall.GetComponent<PlayableDirector>().Play();
            wallState = false;
        }
    }
    public void membranaButton()
    {
        print(membranaState);
        if (!membranaState)
        {
            membrana.GetComponent<PlayableDirector>().playableAsset = membaranaPostiveAnim;
            membrana.GetComponent<PlayableDirector>().Play();
            membranaState = true;

        }
        else
        {
            membrana.GetComponent<PlayableDirector>().playableAsset = membaranaNegativeAnim;
            membrana.GetComponent<PlayableDirector>().Play();
            membranaState = false;
        }
    }
}
