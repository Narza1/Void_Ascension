using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class RandomOST : MonoBehaviour
{
        [SerializeField]
        private string audioFolder = "OST/Dungeon/Normal";

        private void Start()
        {
            AudioClip[] audioClips = Resources.LoadAll<AudioClip>(audioFolder);
  
                AudioClip randomAudioClip = audioClips[Random.Range(0, audioClips.Length)];
                AudioSource audioSource = GetComponent<AudioSource>();

                audioSource.clip = randomAudioClip;
        audioSource.Play();
        }
    }