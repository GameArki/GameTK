using System;
using System.Collections.Generic;
using UnityEngine;

namespace NJM.UIApplication {

    public static class WidgetUtil {

        // ==== Events ====
        public static Func<float> onVolumeGetterHandle;
        public static Func<AudioSource> onAudioSourceGetterHandle;

        static Dictionary<WidgetDefaultSoundType, AudioClip> defaultSoundDict;
        static Dictionary<int, AudioClip> customSoundDict;

        public static void Setup() {
            defaultSoundDict = new Dictionary<WidgetDefaultSoundType, AudioClip>();
            customSoundDict = new Dictionary<int, AudioClip>();
        }

        public static void RegisterDefaultSound(WidgetDefaultSoundType type, AudioClip clip) {
            if (defaultSoundDict.ContainsKey(type)) {
                defaultSoundDict[type] = clip;
            } else {
                defaultSoundDict.Add(type, clip);
            }
        }

        public static void RegisterCustomSound(int typeID, AudioClip clip) {
            if (customSoundDict.ContainsKey(typeID)) {
                customSoundDict[typeID] = clip;
            } else {
                customSoundDict.Add(typeID, clip);
            }
        }

        #region Default
        internal static void Sound_PlaySelect() {
            bool has = defaultSoundDict.TryGetValue(WidgetDefaultSoundType.Select, out AudioClip clip);
            if (has) {
                Sound_Play(clip, true);
            }
        }

        internal static void Sound_PlayConfirm() {
            bool has = defaultSoundDict.TryGetValue(WidgetDefaultSoundType.Confirm, out AudioClip clip);
            if (has) {
                Sound_Play(clip, true);
            }
        }

        internal static void Sound_PlayValueChange() {
            if (defaultSoundDict.TryGetValue(WidgetDefaultSoundType.ValueChange, out AudioClip clip)) {
                Sound_Play(clip, false);
            }
        }

        internal static void Sound_PlayPopup() {
            if (defaultSoundDict.TryGetValue(WidgetDefaultSoundType.Popup, out AudioClip clip)) {
                Sound_Play(clip, true);
            }
        }

        internal static void Sound_PlayCancel() {
            if (defaultSoundDict.TryGetValue(WidgetDefaultSoundType.Cancel, out AudioClip clip)) {
                Sound_Play(clip, true);
            }
        }

        internal static void Sound_PlayDialogue() {
            if (defaultSoundDict.TryGetValue(WidgetDefaultSoundType.Dialogue, out AudioClip clip)) {
                Sound_Play(clip, true);
            }
        }
        #endregion

        #region Custom
        public static void Sound_Play_Custom(int typeID, bool isOneShot) {
            if (customSoundDict.TryGetValue(typeID, out AudioClip clip)) {
                Sound_Play(clip, isOneShot);
            }
        }
        #endregion

        static void Sound_Play(AudioClip clip, bool isOneShot) {
            if (clip == null) {
                return;
            }
            if (onAudioSourceGetterHandle == null) {
                return;
            }
            var audioSource = onAudioSourceGetterHandle.Invoke();
            float volume = onVolumeGetterHandle.Invoke();
            audioSource.volume = volume;
            if (isOneShot) {
                audioSource.PlayOneShot(clip);
            } else {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

    }

}