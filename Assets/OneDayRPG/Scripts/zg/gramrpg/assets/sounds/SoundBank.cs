using System;
using System.Collections.Generic;
using UnityEngine;
using zg.gramrpg.data;

namespace zg.gramrpg.assets.sounds
{
    [CreateAssetMenu(fileName = "SoundBank", menuName = "GramRPG/SoundBank", order = 3)]
    public class SoundBank : ScriptableObject
    {
        // Sound list
        public List<AudioType> clips;

        // Audio lookup
        private Dictionary<SoundType, AudioClip> _audioLookup;

        public AudioClip GetAudioClip(SoundType type)
        {
            if (_audioLookup.TryGetValue(type, out AudioClip audioClip))
                return audioClip;

            return null;
        }

        public void CreateAudioTypeLookUp()
        {
            // Convert list to dictionary for easy look up
            _audioLookup = new Dictionary<SoundType, AudioClip>();
            int i, len = clips.Count;
            for (i = 0; i < len; i++)
            {
                var audioType = clips[i];
                _audioLookup[audioType.type] = audioType.clip;
            }
        }

        [Serializable]
        public struct AudioType
        {
            // Used to add sounds in editor
            public SoundType type;
            public AudioClip clip;
        }
    }
}