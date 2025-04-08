using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameTK {

    public class WidgetVideo : MonoBehaviour {
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] GameObject videoGroup;
        [SerializeField] RawImage rawImage;
        RenderTexture rt;

        public void SetVideoClip(VideoClip videoClip) {
            if (videoClip == null) {
                videoGroup.SetActive(false);
                if (rt != null) {
                    videoPlayer.Stop();
                    videoPlayer.clip = null;
                    videoPlayer.targetTexture = null;
                    rawImage.texture = null;
                    rt.Release();
                    rt = null;
                }
            } else {
                if (rt == null) {
                    rt = new RenderTexture((int)videoClip.width, (int)videoClip.height, 0);
                    rt.filterMode = FilterMode.Point;
                    rt.format = RenderTextureFormat.ARGB32;
                    videoPlayer.targetTexture = rt;
                    rawImage.texture = rt;
                }
                videoGroup.SetActive(true);
                videoPlayer.clip = videoClip;
                videoPlayer.Play();
            }
        }
    }

}