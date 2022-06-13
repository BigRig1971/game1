using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


namespace StupidHumanGames
{
    public class LootableItem : MonoBehaviour
    {
        Quaternion targetRot;
        [SerializeField] Vector3 scale = Vector3.one;
        [SerializeField] Vector3 playerPosition;
        
        [SerializeField] bool spawnLoot = false;
        [SerializeField] float spawnLootOffset = 10f;
        private bool lootable;
        [SerializeField] AudioClip _impactSound;
        [SerializeField, Range(0f, 1f)] float _impactVolume;
        [SerializeField] AudioClip _deathSound;
        [SerializeField, Range(0f, 1f)] float _deathVolume;

        public bool isDamagable = false;
        [SerializeField] int health = 30;
        [SerializeField] float deathDelay = 5f;
        public UnityEvent death;
        public UnityEvent takeHit;
        [System.Serializable]
        public class Item
        {
            public ItemSO _item;
            public int _itemAmount = 1;
        }
        public Item[] _listOfItems;
        
        private void Awake()
        {
            if (isDamagable) lootable = false; else lootable = true;
        }
        private void Start()
        {
            transform.localScale = scale;
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position; 
            if (death == null)
                death = new UnityEvent();
            if (takeHit == null)
                takeHit = new UnityEvent();
           // SitOnGround();
           // AlignToGround();
        }
#if UNITY_EDITOR


#endif

        public void LootableItems()
        {
            if (!lootable) return;

            foreach (Item loi in _listOfItems)
            {
                int remaining = InventoryManager.AddItemToInventory(loi._item, loi._itemAmount);

                if (remaining > 0)
                {
                    loi._itemAmount = remaining;
                }
                else
                {
                    if (!isDamagable)
                    {
                        if (transform.parent != null) Destroy(transform.parent.gameObject, .3f);
                        Destroy(transform.gameObject, .3f);
                    }
                }
            }
        }

        public void TakeDamage(int amount)
        {
            if (!isDamagable) return;
            takeHit.Invoke();
            lootable = false;
            if (_impactSound != null) AudioSource.PlayClipAtPoint(_impactSound, transform.position, _impactVolume);

            health -= amount;
            if (health <= 0)
            {
                death.Invoke();
                isDamagable = false;
                if (_deathSound != null) AudioSource.PlayClipAtPoint(_deathSound, transform.position, _deathVolume);                
                Invoke(nameof(LootItem), deathDelay);
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            //Gizmos.(Vector3.zero + colliderCenter, colliderSize);
        }
        void LootItem()
        {

            lootable = true;
            isDamagable = false;
            if(spawnLoot) DropItems(); else LootableItems();



        }
      
        void DropItems()
        {
            int amount = _listOfItems[0]._itemAmount;
            ItemSO itemDrop = _listOfItems[0]._item;

            for (int i = 0; i < amount; i++)
            {
              
              
                Vector3 force = new Vector3(Random.Range(-3f, 3f), spawnLootOffset + Random.Range(0f, 5f), Random.Range(-3f, 3f));
                var drop = (Instantiate(itemDrop.spawnPrefab, transform.position + (force / 4f), Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0))) as GameObject);

                if (!drop.GetComponent<BoxCollider>()) drop.AddComponent<BoxCollider>();
                drop.AddComponent<Rigidbody>();                
            }
       
            if (transform.parent != null) Destroy(transform.parent.gameObject, .3f); else Destroy(transform.gameObject, .3f);
        }
    }
}
