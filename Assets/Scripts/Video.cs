using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))] 
public class Video : MonoBehaviour
{
    [SerializeField] private Material rippleMaterial;
    [SerializeField] private VideoClip defaultClip;

    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

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

        videoPlayer.clip = clip;
        videoPlayer.Play();
    }
}
