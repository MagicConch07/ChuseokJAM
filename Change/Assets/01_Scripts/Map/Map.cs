using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private GameObject _box;
    [SerializeField] [Range(0, 100)] private int _boxCount = 10;

    [SerializeField] private int _boxDistance = 3;
    [SerializeField] private int _boxDistanceWeight = 5;
    [SerializeField] private float _boxHeight = 5;

    private List<GameObject> _boxList;
    
    private void Initialized()
    {
        _boxList = new List<GameObject>();
    }

    private void Awake()
    {
        Initialized();
    }
    
    private void Start()
    {
        StartCoroutine(TestCoroutine());
    }

    private IEnumerator TestCoroutine()
    {
        while (true)
        {
            CreateMapObject();
            yield return new WaitForSeconds(5f);
        }
    }

    private void CreateMapObject()
    {
        Vector2 newPos = Vector2.zero;
        
        if (_boxList.Count > 0)
            newPos = _boxList[_boxList.Count - 1].transform.position;
        
        _boxList.Clear();
        
        for (int i = 1; i <= _boxCount; ++i)
        {
            Vector2 genPos = new Vector2(
                newPos.x + Random.Range(_boxDistance, _boxDistance + _boxDistanceWeight), 
                transform.position.y + Random.Range(-_boxHeight, _boxHeight));
            newPos = genPos;
            
            GameObject box = Instantiate(_box, newPos, Quaternion.identity);
            _boxList.Add(box);
        }
    }

    
    //[SerializeField] private float 

    private IEnumerator UpDistance()
    {
        // todo : Change method name
        
        //_boxDistanceWeight += 
        yield return null;
    }
}
