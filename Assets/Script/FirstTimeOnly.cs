using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeOnly : MonoBehaviour
{
    public GameObject objectToShowOnlyOnce;

    void Start()
    {
        if (!PlayerPrefs.HasKey("HasShownObject"))
        {
            objectToShowOnlyOnce.SetActive(true);
            PlayerPrefs.SetInt("HasShownObject", 1);
        }
        else
        {
            objectToShowOnlyOnce.SetActive(false);
        }
    }
}