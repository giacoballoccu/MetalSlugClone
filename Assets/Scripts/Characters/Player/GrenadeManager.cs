using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    [SerializeField] public GameObject _grenade;
    [SerializeField] public int _poolCount = 10;

    static protected GrenadePool _pool;
    static GrenadeManager _instance;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        if (_grenade)
            _pool = new GrenadePool(_grenade);
    }

    public static int GetPoolCount()
    {
        if (_instance)
            return _instance._poolCount;
        return 0;
    }

    public static GrenadePool GetPool()
    {
        if (_instance)
            return _pool;
        return null;
    }
    
    public class GrenadePool
    {
        GameObject _prefab;
        List<GameObject> _actives = new List<GameObject>();
        List<GameObject> _inactives = new List<GameObject>();

        public GrenadePool(GameObject prefab)
        {
            _prefab = prefab;
            for (int i = 0; i < GrenadeManager.GetPoolCount(); i++)
            {
                GameObject grenade = GrenadeNew();
                GrenadeReset(grenade);
                _inactives.Add(grenade);
            }
        }

        public void Spawn(Vector2 position, Quaternion rotation)
        {
            GameObject grenade = null;
            if (_inactives.Count <= 0)
                grenade = GrenadeNew();
            else
                grenade = PopFromInactives();
            PushToActives(position, rotation, grenade);
        }

        private void PushToActives(Vector2 position, Quaternion rotation, GameObject grenade)
        {
            GrenadeSetVisible(grenade, position, rotation);
            _actives.Add(grenade);
        }

        private GameObject PopFromInactives()
        {
            GameObject grenade;
            int lastId = _inactives.Count - 1;
            grenade = _inactives[lastId];
            _inactives.RemoveAt(lastId);
            return grenade;
        }

        public void Despawn(GameObject grenade)
        {
            GrenadeReset(grenade);
            int id = _actives.IndexOf(grenade);
            if (id == -1)
                return;
            _inactives.Add(_actives[id]);
            _actives.RemoveAt(id);
        }

        private GameObject GrenadeNew()
        {
            GameObject grenade = Instantiate(_prefab, _instance.transform);
            GrenadeReset(grenade);
            return grenade;
        }

        private void GrenadeSetVisible(GameObject grenade, Vector2 position, Quaternion rotation)
        {
            grenade.transform.position = position;
            grenade.transform.rotation = rotation;
            grenade.SetActive(true);
        }

        private void GrenadeReset(GameObject grenade)
        {
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            grenade.SetActive(false);
        }
    }
}
