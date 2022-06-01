using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class Video : MonoBehaviour
{
    [SerializeField] private VideoClip defaultClip;
    [SerializeField] private Controls controls;
    [SerializeField, Range(0, 5)] private float fadeInDuration;
    [SerializeField, Range(0, 5)] private float fadeOutDuration;
    [SerializeField, Range(0, 5)] private float pentagramStartOffset;

    private VideoPlayer videoPlayer;
    private float? fadeInStartTime;
    private float? fadeOutStartTime;
    private double fadeOutStartTimeInClip;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetCameraAlpha = 0;
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        Controls.VideoTriggered += Controls_VideoTriggered;
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        controls.TriggerPentagramStart();
        this.Wait(pentagramStartOffset, () => PlayClip());
    }

    private void Controls_VideoTriggered(object sender, VideoTriggeredEventHandler e)
    {
        bool isVideoAvailable = false;

        if (string.IsNullOrEmpty(e.ClipName))
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = defaultClip;
            isVideoAvailable = true;
        }
        else
        {
            var fileName = $"{e.ClipName}.mp4";
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Serpinski", fileName);

            if (File.Exists(filePath))
            {
                videoPlayer.source = VideoSource.Url;
                videoPlayer.url = $"file://{filePath}";
                isVideoAvailable = true;
            }
        }

        if (isVideoAvailable)
        {
            videoPlayer.Prepare();
        }
    }

    private void PlayClip()
    {
        fadeOutStartTimeInClip = videoPlayer.length - fadeOutDuration;
        videoPlayer.Play();

        if (fadeInDuration == 0)
        {
            videoPlayer.targetCameraAlpha = 1;
        }
        else
        {
            fadeInStartTime = Time.time;
        }
    }

    private void Update()
    {
        // Handle fade in if necessary.
        if (fadeInStartTime.HasValue)
        {
            UpdateFadeIn();
        }

        // If the video is visible and it is time for the fade out to start, 
        // record the current time to start it.
        if (isFadeOutDue)
        {
            controls.TriggerPentagramEnd();
            fadeOutStartTime = Time.time;
        }

        // Handle the fade out if necessary.
        if (fadeOutStartTime.HasValue)
        {
            UpdateFadeOut();
        }

        videoPlayer.SetDirectAudioVolume(0, videoPlayer.targetCameraAlpha);
    }

    private bool isFadeOutDue => fadeOutDuration > 0 &&
                                 videoPlayer.targetCameraAlpha > 0 &&
                                 !fadeOutStartTime.HasValue &&
                                 videoPlayer.time >= fadeOutStartTimeInClip;

    private void UpdateFadeIn()
    {
        var alphaValue = Mathf.InverseLerp(fadeInStartTime.Value, fadeInStartTime.Value + fadeInDuration, Time.time);
        videoPlayer.targetCameraAlpha = alphaValue;

        if (alphaValue >= 1.0f)
        {
            fadeInStartTime = null;
        }
    }

    private void UpdateFadeOut()
    {
        var alphaValue = 1 - Mathf.InverseLerp(fadeOutStartTime.Value, fadeOutStartTime.Value + fadeOutDuration, Time.time);
        videoPlayer.targetCameraAlpha = alphaValue;

        if (alphaValue <= 0.0f)
        {
            fadeOutStartTime = null;
            videoPlayer.time = 0;
        }
    }
}
