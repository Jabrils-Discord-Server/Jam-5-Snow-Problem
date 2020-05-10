using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{

    private static Transform t;

    void Awake() {
        t = transform;
    }

    public static AudioSource GetSound(string soundName) {
        return t.Find(soundName).gameObject.GetComponent<AudioSource>();
    }
}
