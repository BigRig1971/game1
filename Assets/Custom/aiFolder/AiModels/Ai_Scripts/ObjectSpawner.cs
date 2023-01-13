using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


namespace StupidHumanGames
{

    public class ObjectSpawner : MonoBehaviour
    {
        [Header("SPAWN ON GROUND ONLY PROPERTIES")]
        Quaternion targetRot;
        Quaternion q;
        bool save = true;
        [SerializeField] GameObject _prefab;
        [SerializeField] bool spawnOnGround = true;
        [SerializeField] bool alignWithGround = true;
        [SerializeField] float minYSpace = 10f;
        [SerializeField, Range(0f, 200)] float minTerrainHeight = 0f;
        [SerializeField, Range(0f, 200)] float maxTerrainHeight = 200f;
        [SerializeField, Range(0f, 60)] float slopeLimit = 5f;
        [Header("")]
        public LayerMask avoidableObjects;
        public LayerMask ground;
        public float delay = .1f;
        public int maximum = 15;
        public float maxRange = 3f;
        public float minSeparation = 3f;
        GameObject _object;
        //[SerializeField] SaveGame saveGame;
        bool canSpawn = false;
        float slopeAngle;
        GameObject delete;
        bool delayStarted = false;
        [SerializeField] List<GameObject> list = new List<GameObject>();
        [SerializeField] List<GameObject> deleteThese = new List<GameObject>();
        private float m_internalTimer = 5f;
        private void Awake()
        {
        }
        void Start()
        {
            StartCoroutine(GetAlreadySpawned());
        }
        
        public void OnRemoveObject()
        {
            list.Remove(_object);
        }
        public IEnumerator GetAlreadySpawned()
        {
            yield return new WaitForSeconds(1f);

            SaveableObject[] myItems = FindObjectsOfType(typeof(SaveableObject)) as SaveableObject[];
            foreach (SaveableObject obj in myItems)
            {
                GameObject go = obj.gameObject;
                if (_prefab == null) yield break;
                if (go != null && go.name == _prefab.name)
                {
                    list.Add(go);
                }
                canSpawn = true;
            }
        }
        public void Remove(GameObject go)
        {
            for (int t = list.Count - 1; t >= 0; t--)
            {
                if (go.name != list[t].name)
                {
                    list.RemoveAt(t);
                }
            }
        }
        void Update()
        {
            if (canSpawn)
            {
                spawnObjects();
            }
        }
        public void spawnObjects()
        {
            if (list.Count >= maximum)
            {
               
                return;
            }
            m_internalTimer -= Time.deltaTime;
            m_internalTimer = Mathf.Max(m_internalTimer, 0f);
            if (m_internalTimer == 0f)
            {
                q = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(q.x, 360f), q.z));
                RaycastHit hit;
                Vector3 _position = transform.position + GetOffset();
                if (Physics.Raycast(new Vector3(_position.x, _position.y + transform.up.y * 10, _position.z),
            -transform.up, out hit, 500, ground))
                {
                    slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                }
                if (spawnOnGround)
                {
                    _position = OnGround(_position);
                    if (alignWithGround) q = AlignToGround(_position, q);
                    if (slopeAngle >= slopeLimit) return;
                    if (hit.point.y <= minTerrainHeight || hit.point.y >= maxTerrainHeight) return;
                }
                else
                {
                    if (maxTerrainHeight - hit.point.y < minYSpace) return;
                    if (hit.point.y <= minTerrainHeight || hit.point.y >= maxTerrainHeight) return;
                    _position.y = (hit.point.y + maxTerrainHeight) / 2;
                   // Debug.Log(hit.point.y);
                }

                if (Physics.CheckSphere(_position, minSeparation, avoidableObjects)) return;
               if(save)
                {
                    if (!canSpawn) return;
                    GameObject obj = _prefab.gameObject;
                    SaveGame.SpawnPrefab(obj, _position, q);
                    list.Add(obj);
                }
                else
                {
                    GameObject obj = Instantiate(_prefab, _position, q) as GameObject;
                    list.Add(obj);
                }

                m_internalTimer = delay;
            }
        }
        void LateUpdate()
        {
            //remove all destroyed objects
            list.RemoveAll(o => (o == null || o.Equals(null)));
        }
        Vector3 GetOffset()
        {
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-maxRange, maxRange), 0, UnityEngine.Random.Range(-maxRange, maxRange));
            return offset;
        }
        Quaternion AlignToGround(Vector3 position, Quaternion rotation)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0));

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(position.x, position.y + transform.up.y, position.z),
                -transform.up, out hit, 20, ground))
            {
                targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * rotation;
            }
            return targetRot;
        }
        Vector3 OnGround(Vector3 position)
        {
            Vector3 _pos = position;
            _pos.y = Terrain.activeTerrain.SampleHeight(_pos);
            return _pos;
        }
    }
}
