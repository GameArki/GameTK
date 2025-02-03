using System;
using UnityEngine;

namespace NJM.Modules_Sound {

    public class SoundModuleEntity {

        public int id;
        public int typeID;
        public UniqueSignature belong;
        public SoundLayerType layer;

        public bool isLoop; // destroy when belong destroy

        public bool isFollowBelong;
        public Vector2 happenPos;

        public AudioClip clip;
        public float volumePercent;

        public bool isEffectByDistance;

        public AudioSource player;

        public SoundModuleEntity() { }

        public void Release() {
            player.Stop();
            player.clip = null;
            player.volume = 1;
            player.loop = false;
            player = null;
            clip = null;
        }

        public void Play(float settingVolume) {
            player.clip = clip;
            player.volume = volumePercent * settingVolume;
            player.loop = isLoop;
            if (!player.isPlaying) {
                player.Play();
            } else {
                player.UnPause();
            }
        }

        public void Pause() {
            player.Pause();
        }

        public bool IsPlaying() {
            return player.isPlaying;
        }

        public void Volume_Set(float settingVolume) {
            player.volume = volumePercent * settingVolume;
        }

    }

}