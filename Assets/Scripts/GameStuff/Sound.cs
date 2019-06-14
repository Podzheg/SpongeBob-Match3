using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    AudioClip patrick, spongeTrap;

    [SerializeField]
    GameObject easterHerry;

    AudioSource music;

    void Start() { music = gameObject.GetComponent<AudioSource>(); }

    public void Matched(List<GameObject> matchedFood, bool vertical)
    {       
        if (matchedFood.Count < 5)
        {
            music.PlayOneShot(patrick);
        }
        else
        {
            easterHerry.SetActive(true);
            music.clip = spongeTrap;
            music.Play();
            
        }
    }
}
