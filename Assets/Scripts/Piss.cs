using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Piss : MonoBehaviour
{
    public PlayerCharacterController PlayerController;
    public AudioSource PissAudioSource;
    public AudioClip Unzip;
    public AudioClip ToiletPissStart;
    public AudioClip ToiletPissMiddle;
    public AudioClip ToiletPissEnd;
    public AudioClip BottlePissStart;
    public AudioClip BottlePissMiddle;
    public AudioClip BottlePissEnd;
    public AudioClip Flush;
    public AudioClip Zip;

    public float MaxPiss = 100f;
    public float PissGainPerSecond = 1f;
    public float PissGraceTimer = 5f;
    public float PissDuration = 10f;

    public Cutscene pissCutscene;

    public float CurrentPiss { get; private set; }
    public float PissOverloadTimer { get; private set; }

    private bool _pissing;
    public bool Pissing
    {
        get { return _pissing; }
        set
        {
            _pissing = value;
            if(_pissing) PissOverloadTimer = 0;

            PlayerController.MovementFrozen = _pissing;
        }
    }

    IEnumerator PlayBottlePissAudio()
    {
         PissAudioSource.PlayOneShot(Unzip);
        yield return new WaitForSeconds(Unzip.length);

        PissAudioSource.PlayOneShot(BottlePissStart);
        yield return new WaitForSeconds(ToiletPissStart.length);

        PissAudioSource.clip = BottlePissMiddle;
        PissAudioSource.loop = true;
        PissAudioSource.Play();
        while(Pissing)
        {
            yield return null;
        }

        PissAudioSource.Stop();
        PissAudioSource.loop = false;

        PissAudioSource.PlayOneShot(BottlePissEnd);
        yield return new WaitForSeconds(ToiletPissEnd.length);

        PissAudioSource.PlayOneShot(Zip);
        yield return new WaitForSeconds(Zip.length);
    }

    IEnumerator PlayToiletPissAudio(float duration)
    {
        float loopDuration = duration
            - Unzip.length
            - ToiletPissStart.length
            - ToiletPissEnd.length
            - Flush.length
            - Zip.length;

        PissAudioSource.PlayOneShot(Unzip);
        yield return new WaitForSeconds(Unzip.length);

        PissAudioSource.PlayOneShot(ToiletPissStart);
        yield return new WaitForSeconds(ToiletPissStart.length);

        PissAudioSource.clip = ToiletPissMiddle;
        PissAudioSource.loop = true;
        PissAudioSource.Play();
        yield return new WaitForSeconds(loopDuration);

        PissAudioSource.Stop();
        PissAudioSource.loop = false;

        PissAudioSource.PlayOneShot(ToiletPissEnd);
        yield return new WaitForSeconds(ToiletPissEnd.length);

        PissAudioSource.PlayOneShot(Flush);
        yield return new WaitForSeconds(Flush.length);

        PissAudioSource.PlayOneShot(Zip);
        yield return new WaitForSeconds(Zip.length);
    }        

    public void PissInBathroom()
    {
        Pissing = true;
        pissCutscene.Play();
        StartCoroutine(PlayToiletPissAudio(pissCutscene.GetDuration()));
    }

    public void PissInBottle()
    {
        Pissing = true;
        StartCoroutine(PlayBottlePissAudio());
    }

    public void Reset()
    {
        CurrentPiss = 0;
        PissOverloadTimer = 0;
    }

    void Awake()
    {
        Reset();

        EventManager.Instance.OnRoundComplete += Reset;
        EventManager.Instance.OnPissInBottle += PissInBottle;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.Instance.CurrentState == GameState.Paused)
        {
            return;
        }

        if(Pissing)
        {
            CurrentPiss -= (MaxPiss / PissDuration) * Time.deltaTime;
            if(CurrentPiss <= 0)
            {
                if (!pissCutscene.shouldRun)
                {
                    Pissing = false;
                    CurrentPiss = 0;
                }
            }

            return;
        }

        CurrentPiss += PissGainPerSecond * Time.deltaTime;

        if(CurrentPiss >= MaxPiss)
        {
            CurrentPiss = MaxPiss;
            PissOverloadTimer += Time.deltaTime;
        }

        if(PissOverloadTimer > PissGraceTimer)
        {
            // You pissed yourself!
            Debug.Log("You pissed yourself!");
            SceneTransitionHelper.Instance.TransitionReason = TransitionReason.GameOverPissedYourself;
            SceneTransitionHelper.Instance.FromScene = FromScene.Warehouse;
            SceneManager.LoadScene("GameOver");
        }
    }
}
