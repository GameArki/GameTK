using System;
using System.Collections;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace NJM {

    public class GhostTrailEntity : MonoBehaviour {

        Pool<SpriteRenderer> pool;

        const float interval = 0.1f;
        WaitForSeconds waitSec = new WaitForSeconds(interval);

        public void Ctor() {
            pool = new Pool<SpriteRenderer>(64, () => CreateSpriteRenderer());
        }

        SpriteRenderer CreateSpriteRenderer() {
            var go = new GameObject("GhostTrail");
            go.transform.SetParent(transform);
            var sr = go.AddComponent<SpriteRenderer>();
            return sr;
        }

        public void SetFrame(Vector2 pos, Quaternion rot, Vector3 scale, Sprite sprite, float maintainSec, float alpha) {
            StartCoroutine(IE_SetFrame(pos, rot, scale, sprite, maintainSec, alpha));
        }

        IEnumerator IE_SetFrame(Vector2 pos, Quaternion rot, Vector3 scale, Sprite sprite, float maintainSec, float alpha) {
            SpriteRenderer sr = pool.Get();

            sr.sprite = sprite;
            sr.color = new Color(1, 1, 1, alpha);
            sr.enabled = true;
            sr.transform.position = pos;
            sr.transform.rotation = rot;
            sr.transform.localScale = scale;

            float time = maintainSec;
            while (time > 0) {
                time -= interval;
                yield return waitSec;
            }

            sr.enabled = false;
            pool.Return(sr);

        }

    }

}