using System;
using UnityEngine;

[Serializable]
public struct SpriteSeqModel {

    [NonSerialized] public int index;
    [NonSerialized] public bool isPause;
    [NonSerialized] public float time;
    public float frameSec;
    public bool isLoop;
    public int loopTimes;
    public int loopTimesMax;
    public Sprite[] sprites;

    public sbyte Tick(float dt, out Sprite sprite) {
        // -1: Failed, 0: Running, 1: Done
        if (sprites == null || sprites.Length == 0) {
            sprite = null;
            return -1;
        }

        if (isPause) {
            sprite = sprites[index];
            return 0;
        }

        sbyte status = 0;
        time += dt;
        if (time >= frameSec) {
            time -= frameSec;
            index++;
            if (index >= sprites.Length) {
                if (isLoop) {
                    if (loopTimesMax > 0) {
                        loopTimes++;
                        if (loopTimes >= loopTimesMax) {
                            isPause = true;
                            status = 1;
                        }
                    }
                    index = 0;
                } else {
                    index = sprites.Length - 1;
                    isPause = true;
                    status = 1;
                }
            }
        }
        sprite = sprites[index];
        return status;
    }

    public void Reset() {
        index = 0;
        isPause = false;
        time = 0;
    }
}