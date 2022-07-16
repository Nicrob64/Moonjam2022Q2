using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announcement : MonoBehaviour
{
    public List<AudioSource> audioSources;
    public List<AudioClip> clips;
    public AudioClip startAnnouncement;
    public AudioClip stopAnnouncement;

    protected bool makingAnnouncement = false;
    protected float musicLoopTime;
    protected AudioClip originalJam;

    public void MakeAnnouncement()
    {
        if (makingAnnouncement) { return; }
        makingAnnouncement = true;
        musicLoopTime = audioSources[0].time;
        foreach(AudioSource a in audioSources)
        {
            a.loop = false;
        }
        originalJam = audioSources[0].clip;

        StartCoroutine(PlayRandomClip());
    }

    IEnumerator PlayRandomClip()
    {
        foreach (AudioSource a in audioSources)
        {
            a.Pause();
            a.clip = startAnnouncement;
            a.time = 0;
            a.Play();
        }
        yield return new WaitForSeconds(startAnnouncement.length);
        AudioClip clip = clips[Random.Range(0, clips.Count)];
        foreach (AudioSource a in audioSources)
        {
            a.Pause();
            a.clip = clip;
            a.time = 0;
            a.Play();
        }
        yield return new WaitForSeconds(clip.length);
        foreach (AudioSource a in audioSources)
        {
            a.Pause();
            a.clip = stopAnnouncement;
            a.time = 0;
            a.Play();
        }
        yield return new WaitForSeconds(stopAnnouncement.length);
        foreach (AudioSource a in audioSources)
        {
            a.Pause();
            a.clip = originalJam;
            a.loop = true;
            a.Play();
            a.time = musicLoopTime;
        }
        makingAnnouncement = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) && !makingAnnouncement)
        {
            MakeAnnouncement();
        }
    }


}
