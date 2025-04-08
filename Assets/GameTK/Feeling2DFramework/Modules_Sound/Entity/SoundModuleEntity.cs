using System;
using UnityEngine;

namespace GameTK.Modules_Sound {

    public class SoundModuleEntity {

        public int id;
        public int typeGroup;
        public int typeID;
        public UniqueSignature belong;
        public SoundLayerType layer;

        public AudioSource player;

        public bool isLoop; // destroy when belong destroy

        public bool isFollowBelong;
        public Vector2 happenPos;

        public AudioClip clip;
        public float volumePercent;

        public bool isEffectByDistance;

        // - Fadeout
        public bool isFadingOut;
        public float fadeOutDuration;
        public float fadeOutTimer;
        float fadeOutStartVolume;

        public bool isPause;

        public SoundModuleEntity() { }

        public void Release() {
            player.Stop();
            player.clip = null;
            player.volume = 1;
            player.loop = false;
            clip = null;

            isFadingOut = false;
            fadeOutDuration = 0;
            fadeOutTimer = 0;

            isPause = false;
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
            isPause = false;
        }

        public void FadeOut_Tick(float dt) {
            if (!isFadingOut) {
                return;
            }

            fadeOutTimer -= dt;
            if (fadeOutTimer <= 0) {
                player.Stop();
                isFadingOut = false;
                fadeOutTimer = 0;
            } else {
                player.volume = fadeOutStartVolume * (fadeOutTimer / fadeOutDuration);
            }
        }

        public void FadeOut_Begin(float duration) {
            isFadingOut = true;
            fadeOutDuration = duration;
            fadeOutTimer = duration;
            fadeOutStartVolume = player.volume;
        }

        public void Pause() {
            player.Pause();
            isPause = true;
        }

        public bool IsPlaying() {
            return player.isPlaying;
        }

        public void Volume_Set(float settingVolume) {
            player.volume = volumePercent * settingVolume;
        }

    }

}