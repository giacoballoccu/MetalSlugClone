﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] public GameObject _normalBullet;
    [SerializeField] public GameObject _heavyMachineBullet;
    [SerializeField] public GameObject _grenade;
    [SerializeField] public int _poolCount = 10;

    static protected Dictionary<GameObject, BulletPool> _pools = new Dictionary<GameObject, BulletPool>();
    static BulletManager _instance;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        if (_normalBullet)
            _pools[_normalBullet] = new BulletPool(_normalBullet);
        if (_heavyMachineBullet)
            _pools[_heavyMachineBullet] = new BulletPool(_heavyMachineBullet);
        if (_grenade)
            _pools[_grenade] = new BulletPool(_grenade);
    }
    
    public static int GetPoolCount()
    {
        if (_instance)
            return _instance._poolCount;
        return 0;
    }

    public static BulletPool GetNormalBulletPool()
    {
        if (_instance)
           return _pools[_instance._normalBullet];
        return null;
    }

    public static BulletPool GetHeavyMachineBulletPool()
    {
        if (_instance)
            return _pools[_instance._heavyMachineBullet];
        return null;
    }

    public static BulletPool GetGrenadePool()
    {
        if (_instance)
            return _pools[_instance._grenade];
        return null;
    }

    public class BulletPool
    {
        GameObject _prefab;
        List<GameObject> _actives = new List<GameObject>();
        List<GameObject> _inactives = new List<GameObject>();

        public BulletPool(GameObject prefab)
        {
            _prefab = prefab;
            for (int i = 0; i < BulletManager.GetPoolCount(); i++)
            {
                GameObject bullet = BulletNew();
                BulletReset(bullet);
                _inactives.Add(bullet);
            }
        }

        public void Spawn(Vector2 position, Quaternion rotation)
        {
            GameObject bullet = null;
            if (_inactives.Count <= 0)
                bullet = BulletNew();
            else
                bullet = PopFromInactives();
            PushToActives(position, rotation, bullet);
        }

        private void PushToActives(Vector2 position, Quaternion rotation, GameObject bullet)
        {
            BulletSetVisible(bullet, position, rotation);
            _actives.Add(bullet);
        }

        private GameObject PopFromInactives()
        {
            GameObject bullet;
            int lastId = _inactives.Count - 1;
            bullet = _inactives[lastId];
            _inactives.RemoveAt(lastId);
            return bullet;
        }

        public void Despawn(GameObject bullet)
        {
            BulletReset(bullet);
            int id = _actives.IndexOf(bullet);
            if (id == -1)
                return;
            _inactives.Add(_actives[id]);
            _actives.RemoveAt(id);
        }

        private GameObject BulletNew()
        {
            GameObject bullet = Instantiate(_prefab, _instance.transform);
            BulletReset(bullet);
            return bullet;
        }

        private void BulletSetVisible(GameObject bullet, Vector2 position, Quaternion rotation)
        {
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true);
        }

        private void BulletReset(GameObject bullet)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            bullet.SetActive(false);
        }
    }
}
