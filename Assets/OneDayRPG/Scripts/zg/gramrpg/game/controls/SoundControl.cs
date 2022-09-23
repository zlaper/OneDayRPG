using UnityEngine;
using zg.gramrpg.assets.sounds;
using zg.gramrpg.data;
using zg.gramrpg.utils;

namespace zg.gramrpg.game.controls
{
    public class SoundControl : Singleton<SoundControl>
    {
        public GameObject soundPlayerPrefab;
        public SoundBank soundBank;

        private TypePool<SoundPlayer> soundPool;
        private AudioSource _playerMusic;

        private void Start()
        {
            // Create audio lookup
            soundBank.CreateAudioTypeLookUp();
            //Create pool
            CreateSoundPool();
            // Create music audio source
            CreateMusic();
        }

        public void PlayMusic(SoundType soundType)
        {
            _playerMusic.clip = soundBank.GetAudioClip(soundType);
            _playerMusic.Play();
        }

        public void PlaySound(SoundType soundType)
        {
            // Play sfx by type
            AudioClip soundClip = soundBank.GetAudioClip(soundType);
            if (soundClip != null)
            {
                // Create player for sfx and play
                SoundPlayer player = soundPool.CreateItem(transform);
                StartCoroutine(player.PlaySound(soundClip, soundPool));
            }
        }

        private void CreateSoundPool()
        {
            // Create pool storage 
            GameObject poolStorage = new GameObject("SoundPool");
            poolStorage.transform.SetParent(this.transform);
            poolStorage.SetActive(false);
            // Create pool
            soundPool = new TypePool<SoundPlayer>();
            soundPool.Setup(soundPlayerPrefab, poolStorage.transform);
        }

        private void CreateMusic()
        {
            _playerMusic = soundPool.CreateItem(transform).audioSource;
            _playerMusic.loop = true;
            _playerMusic.volume = 0.3f;
        }
    }
}
