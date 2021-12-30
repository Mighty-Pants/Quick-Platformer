using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip jump, coin, hurt, up ,victory, lose, back1, back2, back3;

    [SerializeField] GameObject soundObject;

    void Awake()
    {
        instance = this;
    }

    public void playMusic(string name)
    {
        switch (name)
        {
            case "jump":
                soundCreate(jump);
                break;
            case "coin":
                soundCreate(coin);
                break;
            case "hurt":
                soundCreate(hurt);
                break;
            case "victory":
                soundCreate(victory);
                break;
            case "lose":
                soundCreate(lose);
                break;
            case "up":
                soundCreate(up);
                break;
        }
    }

    void soundCreate(AudioClip clip)
    {
        GameObject newObject = Instantiate(soundObject, transform);
        newObject.GetComponent<AudioSource>().clip = clip;
        newObject.GetComponent<AudioSource>().Play();
    }

    public void playBack(int name)
    {
        switch(name){
            case 1:
                backCreate(back1);
                break;
            case 2:
                backCreate(back2);
                break;
            case 3:
                backCreate(back3);
                break;
        }
    }

    public void backCreate(AudioClip clip)
    {
        GameObject newObject = Instantiate(soundObject, transform);
        newObject.GetComponent<AudioSource>().clip = clip;
        newObject.GetComponent<AudioSource>().loop = true;
        newObject.GetComponent<AudioSource>().Play();
    }
}
