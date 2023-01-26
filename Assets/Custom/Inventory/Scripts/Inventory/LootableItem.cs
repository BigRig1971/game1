using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StupidHumanGames
{
    public class LootableItem : MonoBehaviour, DamageInterface
    {
        [SerializeField] List<Item> randomItems = new List<Item>();
        [SerializeField] bool randomSize = false;
        [SerializeField] AudioSource _audioSource;
        Quaternion targetRot;
        [SerializeField] float droppedLootScale = 1f;
        [SerializeField] float damageDropScale = 1f;    
        [SerializeField] bool spawnLoot = false;
        [SerializeField] float spawnLootOffset = 10f;
        private bool lootable;
        [System.Serializable]
        public class ImpactSound
        {
            public AudioClip impactSound;
            public float impactVolume;
        }
        [SerializeField] List<ImpactSound> _listOfImpactSounds = new List<ImpactSound>();
        AudioClip _impactSound;
        float _impactVolume;
        [SerializeField] AudioClip _deathSound;
        [SerializeField] float _deathVolume;
        public bool isDamagable = false;
        [SerializeField] int health = 30;
        [SerializeField] float deathDelay = 5f;
        [System.Serializable]
        public class Item
        {
            public ItemSO _item;
            public int _itemAmount = 1;
        }
        public List<Item> _listOfItems = new List<Item>();  
        [System.Serializable]
        public class DamageItem
        {
            public ItemSO _item;
            public int _itemAmount = 1;
        }
        public DamageItem[] _listOfDamageItems;

        private void Awake()
        {
            if (isDamagable) lootable = false; else lootable = true;
        }
        private void Start()
        {
			RandomLootableItems();
            OnRandomScale(); 
        }
#if UNITY_EDITOR


#endif
        public void LootableItems()
        {
            if (!lootable && isDamagable) return;
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
                        if (transform.parent != null)
                        {
                            Destroy(transform.parent.gameObject);
                            return;
                        }

                        Destroy(transform.gameObject);
                    }
                }
            }
        }
        public void RandomLootableItems()
        {
            if (randomItems.Count > 0)
            {
                var index = Random.Range(0, randomItems.Count);
                _listOfItems.Add(randomItems[index]);
            }
        }
        public void RandomImpactSounds()
        {
            if(_listOfImpactSounds.Count > 0)
            {
                var index = Random.Range(0, _listOfImpactSounds.Count);
                _impactSound = _listOfImpactSounds[index].impactSound;
                _impactVolume = _listOfImpactSounds[index].impactVolume;
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
            if (spawnLoot) StartCoroutine(OnDropItems()); else LootableItems();
        }
        IEnumerator OnDropItems()
        {
            transform.localScale = Vector3.zero;
            foreach (Item i in _listOfItems)
            {
                for (int o = 0; o < i._itemAmount; o++)
                {
                    Vector3 force = new Vector3(Random.Range(-3f, 3f), spawnLootOffset, Random.Range(-3f, 3f));
                    var drop = (Instantiate(i._item.spawnPrefab, transform.position + (force / 4f), Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)))) as GameObject);
                    drop.transform.localScale = new Vector3(drop.transform.localScale.x * droppedLootScale * Random.Range(1f, 1.5f), drop.transform.localScale.y * droppedLootScale * Random.Range(1f, 1.5f), drop.transform.localScale.z * droppedLootScale * Random.Range(1f, 1.5f));
                    if (!drop.GetComponent<MeshCollider>()) drop.AddComponent<MeshCollider>();
                    drop.GetComponent<MeshCollider>().convex = true;
                    drop.AddComponent<Rigidbody>();
                    drop.GetComponent<Rigidbody>().drag = 1f;
                    Destroy(drop.gameObject, 60f);
                    yield return new WaitForSeconds(.05f);
                }
            }
            if (transform.parent != null) Destroy(transform.parent.gameObject); else Destroy(transform.gameObject);
        }
        void OnDropOnDamage()
        {
            if (_listOfDamageItems.Length > 0)
            {
                var index = Random.Range(0, _listOfDamageItems.Length);
                Vector3 force = new Vector3(Random.Range(-3f, 3f), spawnLootOffset, Random.Range(-3f, 3f));
                var drop = (Instantiate(_listOfDamageItems[index]._item.spawnPrefab, transform.position + (force / 4f), Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)))) as GameObject);
                drop.transform.localScale = new Vector3(drop.transform.localScale.x * damageDropScale * Random.Range(1f, 1.5f), drop.transform.localScale.y * damageDropScale * Random.Range(1f, 1.5f), drop.transform.localScale.z * damageDropScale * Random.Range(1f, 1.5f));
                if (!drop.GetComponent<MeshCollider>()) drop.AddComponent<MeshCollider>();
                drop.GetComponent<MeshCollider>().convex = true;
                drop.AddComponent<Rigidbody>();
                drop.GetComponent<Rigidbody>().drag = 1f;
                Destroy(drop.gameObject, 120f);
            }
        }
        void OnRandomScale()
        {
            if (randomSize)
            {
                transform.localScale = new Vector3(transform.localScale.x * Random.Range(1f, 2f), transform.localScale.y * Random.Range(1f, 2f), transform.localScale.z * Random.Range(1f, 2f));
            }
        }
        public void Damage(int damage)
        {
            if(damage <=0) return;  
            RandomImpactSounds();
			if (!isDamagable) return;
			lootable = false;
			if (_impactSound != null) _audioSource?.PlayOneShot(_impactSound, _impactVolume);
			if (spawnLoot) OnDropOnDamage();
            health -= damage;
			if (health <= 0)
			{
				isDamagable = false;
				if (_deathSound != null) _audioSource?.PlayOneShot(_deathSound, _deathVolume);
				Invoke(nameof(LootItem), deathDelay);
			}
		}
    }
}
