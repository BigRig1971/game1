using Lowscope.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Lowscope.Saving.Examples
{
    public class ExamplePoolBall : MonoBehaviour, ISaveable
    {
        public UnityEvent OnDestroyed;
        public UnityEvent OnDisabled;

        [SerializeField] private float destroyAfterTime = 10;
        [SerializeField] private Rigidbody rigidBody;

        private float destroyTime = 0;

        private void Update()
        {
            if (destroyTime > destroyAfterTime)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                destroyTime += Time.deltaTime;
            }
        }

        public void ResetValues()
        {
            destroyTime = 0;
            rigidBody.velocity = new Vector3();
        }

        private void OnDisable()
        {
            OnDisabled.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyed.Invoke();
        }

        [System.Serializable]
        private struct SaveData
        {
            public float DestroyTime;
            public Vector3 Velocity;
        }
        
        public void OnLoad(string data)
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(data);

            destroyTime = saveData.DestroyTime;
            rigidBody.velocity = saveData.Velocity;
        }

        public string OnSave()
        {
            return JsonUtility.ToJson(new SaveData()
            {
                DestroyTime = destroyTime,
                Velocity = rigidBody.velocity
            });
        }

        public bool OnSaveCondition()
        {
            return true;
        }
    }
}