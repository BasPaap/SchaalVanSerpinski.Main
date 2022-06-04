using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class Video : MonoBehaviour
{
    [SerializeField] private VideoClip defaultClip;
    [SerializeField] private Controls controls;
    [SerializeField] private RawImage rawImage;
    [SerializeField, Range(0, 5)] private float fadeInDuration;
    [SerializeField, Range(0, 5)] private float fadeOutDuration;
    [SerializeField, Range(0, 5)] private float pentagramStartOffset;

    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;
    private float? fadeInStartTime;
    private float? fadeOutStartTime;
    private double fadeOutStartTimeInClip;

    private void Awake()
    {
        renderTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
        rawImage.color = Color.clear;
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        Controls.VideoTriggered += Controls_VideoTriggered;
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        StartPlayer();
    }

    private void StartPlayer()
    {
        var settings = Settings.Load();
        if (settings.RandomizeVideoRotation)
        {
            rawImage.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
        }

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
            if (videoPlayer.isPrepared)
            {
                StartPlayer();
            }
            else
            {
                videoPlayer.Prepare();
            }
        }
    }

    private void PlayClip()
    {
        fadeOutStartTimeInClip = videoPlayer.length - fadeOutDuration;
        videoPlayer.Play();

        if (fadeInDuration == 0)
        {
            rawImage.color = Color.white;
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

        videoPlayer.SetDirectAudioVolume(0, rawImage.color.a);
    }

    private bool isFadeOutDue => fadeOutDuration > 0 &&
                                 rawImage.color.a > 0 &&
                                 !fadeOutStartTime.HasValue &&
                                 videoPlayer.time >= fadeOutStartTimeInClip;

    private void UpdateFadeIn()
    {
        var alphaValue = Mathf.InverseLerp(fadeInStartTime.Value, fadeInStartTime.Value + fadeInDuration, Time.time);
        rawImage.color = new Color(1, 1, 1, alphaValue);

        if (alphaValue >= 1.0f)
        {
            fadeInStartTime = null;
        }
    }

    private void UpdateFadeOut()
    {
        var alphaValue = 1 - Mathf.InverseLerp(fadeOutStartTime.Value, fadeOutStartTime.Value + fadeOutDuration, Time.time);
        rawImage.color = new Color(1, 1, 1, alphaValue);

        if (alphaValue <= 0.0f)
        {
            fadeOutStartTime = null;
            videoPlayer.Stop();
            videoPlayer.time = 0;
        }
    }
}
