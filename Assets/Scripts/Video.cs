using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))] 
public class Video : MonoBehaviour
{
    [SerializeField] private VideoClip defaultClip;
    [SerializeField, Range(0, 5)] private float fadeInDuration;
    [SerializeField, Range(0, 5)] private float fadeOutDuration;
    
    private VideoPlayer videoPlayer;
    private float? fadeInStartTime;
    private float? fadeOutStartTime;
    private double fadeOutStartTimeInClip;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetCameraAlpha = 0;

        Controls.VideoTriggered += Controls_VideoTriggered;
    }

    private void Controls_VideoTriggered(object sender, VideoTriggeredEventHandler e)
    {
        if (string.IsNullOrEmpty(e.ClipName))
        {
            PlayClip(defaultClip);
        }        
    }

    private void PlayClip(VideoClip clip)
    {
        if (clip == null)
        {
            return;
        }

        fadeOutStartTimeInClip = clip.length - fadeOutDuration;
        videoPlayer.clip = clip;
        videoPlayer.Play();
        fadeInStartTime = Time.time;
    }

    private void Update()
    {
        // Handle fade in if necessary.
        if (fadeInStartTime.HasValue)
        {
            var alphaValue = Mathf.InverseLerp(fadeInStartTime.Value, fadeInStartTime.Value + fadeInDuration, Time.time);
            videoPlayer.targetCameraAlpha = alphaValue;
            
            if (alphaValue >= 1.0f)
            {
                fadeInStartTime = null;
            }
        }        

        // If the video is visible and it is time for the fade out to start, 
        // record the current time to start it.
        if (videoPlayer.targetCameraAlpha > 0 && 
            !fadeOutStartTime.HasValue && 
            videoPlayer.time >= fadeOutStartTimeInClip)
        {
            fadeOutStartTime = Time.time;
        }

        // Handle the fade out if necessary.
        if (fadeOutStartTime.HasValue)
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
}
