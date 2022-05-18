using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
using Photon.Pun;
using FirstGearGames.SmoothCameraShaker;
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
    bool canDamageStuff = false;
    
   
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
            if (!canDamageStuff) return;
            canDamageStuff = false;
            _animator.SetTrigger("Interrupt");
            other.gameObject.GetComponent<LootableItem>().TakeDamage(damagePower);
            other.gameObject.GetComponent<LootableItem>().LootableItems();
            _animator.SetFloat("ChopSpeed", 0f);
            Invoke(nameof(PauseAnimation), .5f);
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
        {
            canDamageStuff = true;
            _animator.SetTrigger(_triggerName);
        }
    }
    void PauseAnimation()
    {

        _animator.SetFloat("ChopSpeed", 1f);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
    }
}
