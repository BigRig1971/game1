using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectSpawner : MonoBehaviour
{

    [Header("SPAWN ON GROUND ONLY PROPERTIES")]
    Quaternion targetRot;
    [SerializeField] bool spawnOnGround = true;
    [SerializeField] bool alignWithGround = true;
    [SerializeField, Range(0f, 200)] float minTerrainHeight = 0f;
    [SerializeField, Range(0f, 200)] float maxTerrainHeight = 200f;
    [SerializeField, Range(0f, 60)] float slopeLimit = 5f;
    [Header("")]
    public float scale = 1f;
    public LayerMask avoidableObjects;
    public LayerMask ground;
    public GameObject[] prefab;
    public float delay = .1f;
    public int maximum = 15;
    public float maxRange = 3f;
    public float minSeparation = 3f;
    GameObject go;

    float slopeAngle;


    List<GameObject> list = new List<GameObject>();
    private float m_internalTimer = 5f;

#if UNITY_EDITOR

#endif
   
    void Start()
    {
        m_internalTimer = delay;
    }
    public void OnRemoveObject()
    {
        list.Remove(go);
    }
    void Update()
    {
       
        spawnObjects();
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
            Quaternion q = Quaternion.identity;
            foreach (GameObject _prefab in prefab)
            {
                float rndScale = Random.Range(.8f, 1.2f);
                RaycastHit hit;
                Vector3 _transform = transform.position + GetOffset();
                if (Physics.Raycast(new Vector3(_transform.x, _transform.y + transform.up.y * 2, _transform.z),
            -transform.up, out hit, 50, ground))
                {
                    slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                }
                if (spawnOnGround)
                {
                    _transform = OnGround(_transform);
                    if (alignWithGround) q = AlignToGround(_transform, q);
                    if (slopeAngle >= slopeLimit) return;
                    if (hit.point.y <= minTerrainHeight || hit.point.y >= maxTerrainHeight) return;
                }
                else
                {
                    if (_transform.y <= OnGround(_transform).y + 3f) return;
                }
                                                         
                if (Physics.CheckSphere(_transform, minSeparation, avoidableObjects)) return;
                q = Quaternion.Euler(new Vector3(0, Random.Range(q.x, 360f), q.z));
                _prefab.transform.localScale = new Vector3(scale*rndScale, scale*rndScale, scale*rndScale);
                GameObject obj = Instantiate(_prefab, _transform, q) as GameObject;
                
                list.Add(obj);
                go = obj;
                m_internalTimer = delay;

            }
        }
    }
  

    void LateUpdate()
    {
        //remove all destroyed objects
        list.RemoveAll(o => (o == null || o.Equals(null)));
    }
    Vector3 GetOffset()
    {
        Vector3 offset = new Vector3(Random.Range(-maxRange, maxRange), 0, Random.Range(-maxRange, maxRange));
        return offset;
    }
    Quaternion AlignToGround(Vector3 position, Quaternion rotation)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));

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