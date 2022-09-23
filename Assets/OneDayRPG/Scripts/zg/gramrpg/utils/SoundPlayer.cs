using System.Collections;
using UnityEngine;

namespace zg.gramrpg.utils
{

    public class SoundPlayer : MonoBehaviour
    {

        public AudioSource audioSource;

        public IEnumerator PlaySound(AudioClip sfx, TypePool<SoundPlayer> soundPool)
        {
            // Set parent and play
            this.transform.SetParent(Camera.main.transform, false);
            if(audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = sfx;
            audioSource.Play();
            
            // Wait to finish
            yield return new WaitForSecondsRealtime(sfx.length);

            // Store
            soundPool.StoreItem(this);
        }
    }
}