using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
using Photon.Pun;
public class AnimTriggerAndDamageDealer : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 colliderSize = Vector3.one;
    public Vector3 colliderCenter = Vector3.zero;
    public KeyCode _keyCode = KeyCode.Mouse0;
    public int damagePower = 5;
    public string _triggerName;
    public string _animationName;
    public string _floatName;
    public string _boolName;
    public float delayImpact = .3f;
    [SerializeField] Animator _animator;
    LootableItem damagableGo;
    bool canCauseDamage = false;
    bool canClickAttackButton = true;

    private void OnEnable()
    {
        if (_boolName == "") return;

        if (_animator) _animator.SetBool(_boolName, true);
    }
    private void OnDisable()
    {
        if (_boolName == "") return;

        if (_animator) _animator.SetBool(_boolName, false);

    }
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
        if (other.CompareTag("Lootable") && canCauseDamage)///
		{
            canCauseDamage = false;
            damagableGo = other.gameObject.GetComponent<LootableItem>();
            StartCoroutine(CauseSomeDamage());
        }
    }
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_animationName) || _animator.GetCurrentAnimatorStateInfo(1).IsName(_animationName) || _animator.GetCurrentAnimatorStateInfo(2).IsName(_animationName))
        {
            if (!_animator.GetBool("Attack"))
                _animator.SetBool("Attack", true);
        }
        else
        {
            if (_animator.GetBool("Attack"))
                _animator.SetBool("Attack", false);
        }
        if (Input.GetKey(_keyCode) && !InventoryManager.IsOpen() && canClickAttackButton)
        {
            StartCoroutine(CanClicAttackButton());
        }
    }
    private IEnumerator CauseSomeDamage()
    {
        canClickAttackButton = true;
        damagableGo.TakeDamage(damagePower);
        _animator.SetTrigger("Interrupt");
        _animator.speed = 0f;
        yield return new WaitForSeconds(.2f);
        _animator.speed = 1;
        if (damagableGo.health <= 0)
        {
            damagableGo.LootableItems();
        }
    }
    private IEnumerator CanClicAttackButton()
    {
        //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_triggerName))

        _animator.SetTrigger(_triggerName);
        canCauseDamage = true;

        while ((_animator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
        {
            canClickAttackButton = false;
            yield return null;
            canClickAttackButton = true;
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
    }
}
