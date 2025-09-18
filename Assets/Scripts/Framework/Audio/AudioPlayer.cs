using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private Dictionary<AudioClip, AudioSource> _audioSources = new Dictionary<AudioClip, AudioSource>();
        
        public void PlayAudio(AudioClip clip)
        {
            PlayAudioInternal(clip, false, false);
        }
        
        public void PlayAudioOverlapping(AudioClip clip)
        {
            PlayAudioInternal(clip, false, true, true);
        }

        public void PlayAudioLooped(AudioClip clip)
        {
            PlayAudioInternal(clip, true, false);
        }

        private void PlayAudioInternal(AudioClip clip, bool loop, bool overlap, bool pitchChange = false)
        {
            if (!clip) return;
    
            if (!_audioSources.ContainsKey(clip))
                _audioSources[clip] = gameObject.AddComponent<AudioSource>();
    
            var audioSource = _audioSources[clip];

            if(!overlap)
                if (audioSource.isPlaying) return;
            
            if (pitchChange)
                audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            else
                audioSource.pitch = 1f;
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        public void StopAudio(AudioClip clip)
        {
            if (!clip) return;

            if (!_audioSources.ContainsKey(clip)) return;
            var audioSource = _audioSources[clip];
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
