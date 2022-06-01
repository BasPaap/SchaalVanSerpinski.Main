using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gong : MonoBehaviour
{
    [SerializeField] private AudioClip gongClip;

    private AudioSource audioSource;
    private bool isPlaying;
    private float? lastGongTime = null;
    private float gongInterval;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var settings = Settings.Load();
        gongInterval = settings.GongInterval;
    }

    private void OnEnable()
    {
        Controls.GongTriggered += Controls_GongTriggered;
        Controls.PentagramTriggered += Controls_PentagramTriggered;
        Controls.VideoTriggered += Controls_VideoTriggered;
    }

    private void OnDisable()
    {
        Controls.GongTriggered -= Controls_GongTriggered;
        Controls.PentagramTriggered -= Controls_PentagramTriggered;
        Controls.VideoTriggered -= Controls_VideoTriggered;
    }

    private void Controls_VideoTriggered(object sender, VideoTriggeredEventHandler e) => Stop();
    private void Controls_PentagramTriggered(object sender, System.EventArgs e) => Stop();

    private void Controls_GongTriggered(object sender, System.EventArgs e)
    {
        isPlaying = !isPlaying;
    }

    private void Stop()
    {
        isPlaying = false;
    }

    private void Update()
    {
        if (isPlaying && gongClip != null)
        {
            if (lastGongTime == null ||
                (lastGongTime.HasValue && Time.time - lastGongTime.Value > gongInterval))
            {
                audioSource.PlayOneShot(gongClip);
                lastGongTime = Time.time;
            }
        }
    }
}
