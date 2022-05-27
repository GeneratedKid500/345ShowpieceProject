using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBattleSwitcheer : MonoBehaviour
{
    [SerializeField] AudioSource musicSourceA;
    [SerializeField] AudioSource musicSourceB;

    [SerializeField] AudioClip[] normalClips;
    [SerializeField] AudioClip[] battleClips;

    int clipA;
    int clipB;

    bool fadeA;

    List<Collider> inBubble;

    private void Start()
    {
        inBubble = new List<Collider>();

        clipA = Random.Range(0, normalClips.Length);

        clipB = Random.Range(0, battleClips.Length);

        musicSourceA.clip = normalClips[clipA];
        musicSourceB.clip = battleClips[clipB];
        musicSourceA.loop = true;
        musicSourceB.loop = true;
        musicSourceA.volume = 1;
        musicSourceB.volume = 0;
        musicSourceA.Play();
    }

    private void Update()
    {
        if (fadeA)
        {
            if (musicSourceA.volume > 0)
            {
                musicSourceA.volume = Mathf.Lerp(musicSourceA.volume, 0, Time.deltaTime * 4);
            }

            if (musicSourceB.volume < 1)
            {
                musicSourceB.volume = Mathf.Lerp(musicSourceB.volume, 1, Time.deltaTime*2);
            }
            if (!musicSourceB.isPlaying) musicSourceB.Play();
        }
        else
        {
            musicSourceB.volume = 0;
            musicSourceB.time = 0;
            musicSourceB.Stop();
            if (musicSourceA.volume < 1)
            {
                musicSourceA.volume = Mathf.Lerp(musicSourceA.volume, 1, Time.deltaTime * 2);
            }
            if (!musicSourceA.isPlaying) musicSourceA.Play();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < inBubble.Count; i++)
        {
            if (inBubble[i] == null || !inBubble[i].gameObject.activeSelf)
            {
                inBubble.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Duel":
            case "Swarm":
                fadeA = true;
                if (!inBubble.Contains(other))
                {
                    inBubble.Add(other);
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Duel":
            case "Swarm":
                inBubble.Remove(other);
                break;

            default:
                break;
        }

        if (inBubble.Count == 0)
        {
            fadeA = false;
        }
    }

}
