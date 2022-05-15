using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
using Photon.Pun;
public class AnimTriggerAndDamageDealer : MonoBehaviour
{
    Rigidbody rb;
    public LayerMask lootableLayer;
    public Vector3 colliderSize = Vector3.one;
    public Vector3 colliderCenter = Vector3.zero;
    public KeyCode _keyCode = KeyCode.Mouse0;
    public int damagePower = 5;
    public string _triggerName;
    public string _animationName;  
    public float delayImpact = .3f;
    [SerializeField] Animator _animator;
    LootableItem damagableGo;
    
   
    


    private void Start()
    {
     
        if (TryGetComponent<Rigidbody>(out rb))
        {
            rb = GetComponent<Rigidbody>();
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
            rb.isKinematic = true;
        }
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = colliderSize;
        boxCollider.center = (Vector3.zero + colliderCenter);
        boxCollider.isTrigger = true;
    }
   
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Lootable"))///
		{
            _animator.SetTrigger("Interrupt");
            damagableGo = other.gameObject.GetComponent<LootableItem>();
            damagableGo.TakeDamage(damagePower);
           
            if (damagableGo.lootable)
            {
                damagableGo.LootableItems();
            }
        }
    }

    void Update()
    {
      
        if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
        {
            _animator.SetTrigger(_triggerName);
        }
    }
   
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
    }
}
