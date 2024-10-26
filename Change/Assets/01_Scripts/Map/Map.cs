using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private GameObject _box;
    [SerializeField][Range(0, 100)] private int _boxCount = 10;

    [SerializeField] private int _boxDistance = 3;
    [SerializeField] private int _boxDistanceWeight = 5;
    [SerializeField] private float _boxHeight = 5;
    
    private List<GameObject> _boxList;
    private List<GameObject> _oldBoxList;
    private Transform _createTrm;
    private bool _isCreate = false;

    public Transform nextHook;

    private void Initialized()
    {
        _boxList = new List<GameObject>();
        _oldBoxList = new List<GameObject>();
    }

    private void Awake()
    {
        Initialized();
    }

    private void Start()
    {
        CreateMapObject();
    }

    private void Update()
    {
        // Create Map
        if (_createTrm is null == false && _isCreate == false)
        {
            if (_playerTrm.position.x > _createTrm.position.x)
                CreateMapObject();
        }

        // NextHook
        /*if (_isCreate == false)
        {
            foreach (var box in _oldBoxList)
            {
                if (_playerTrm.position.x < box.transform.position.x) 
                    _player.SetNextHook(box.transform);
                else
                    _oldBoxList.Remove(box);
            }
        }*/
    }

    private void CreateMapObject()
    {
        _isCreate = true;

        Vector2 newPos = Vector2.zero;
        // TODO : 제발 이름 좀 바꿔 제발 변수명좀
        int yPos = Random.Range(0, _boxDistanceWeight);

        if (_boxList.Count > 0)
            newPos = _boxList[_boxList.Count - 1].transform.position;

        _boxList.Clear();

        for (int i = 1; i <= _boxCount; ++i)
        {
            Vector2 genPos = new Vector2(
                newPos.x + Random.Range(_boxDistance, _boxDistance + _boxDistanceWeight),
                transform.position.y + yPos + Random.Range(0, _boxHeight));
            newPos = genPos;

            GameObject box = Instantiate(_box, newPos, Quaternion.identity);
            _boxList.Add(box);
        }

        foreach (var box in _boxList)
        {
            _oldBoxList.Add(box);
        }
        _createTrm = _boxList[_boxCount / 2].transform;
        _isCreate = false;
    }
}
