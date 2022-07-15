using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVA.Component
{
    /// <summary>
    /// For auto blink, but also can be use to animate any blendshapes, like mouth and face expression
    /// </summary>
    [ExecuteInEditMode]
    public class AutoBlink : MonoBehaviour
    {

        [System.Serializable]
        public class BlendShapeInfo
        {
            public SkinnedMeshRenderer skinnedMeshRenderer;
            public int index;
            public int minBlend = 0;
            public int maxBlend = 100;
        }

        public List<BlendShapeInfo> blendShapes;
        public float blendTime = 0.2f;
        public float interval = 5.0f;

        private float m_elapsedTime = 0.0f;
        private float m_deltaTime = 0.0f;
        private bool isRunning = false;

        void Start()
        {
            if (blendShapes.Count == 0)
            {
                Destroy(this);
            }
        }

        void Update()
        {
            m_elapsedTime += Time.deltaTime;

            if (m_elapsedTime > interval)
            {
                Blink(blendTime);
                m_elapsedTime = 0;
            }

        }

        private void Blink(float time = 2.0f)
        {
            StartCoroutine(Blinking(time));
        }


        private IEnumerator Blinking(float time)
        {
            if (isRunning)
            {
                yield break;
            }

            isRunning = true;
            m_deltaTime = 0;

            float halfTime = time / 2;

            while (m_deltaTime <= halfTime)
            {
                m_deltaTime += Time.deltaTime;

                foreach (BlendShapeInfo bs in blendShapes)
                {
                    float value = Mathf.Lerp(bs.minBlend, bs.maxBlend, m_deltaTime / halfTime);
                    bs.skinnedMeshRenderer.SetBlendShapeWeight(bs.index, value);
                }

                yield return 0;
            }

            m_deltaTime = 0;

            while (m_deltaTime <= halfTime)
            {
                m_deltaTime += Time.deltaTime;

                foreach (BlendShapeInfo bs in blendShapes)
                {
                    float value = Mathf.Lerp(bs.maxBlend, bs.minBlend, m_deltaTime / halfTime);
                    bs.skinnedMeshRenderer.SetBlendShapeWeight(bs.index, value);
                }

                yield return 0;
            }

            isRunning = false;
        }
    }
}