using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private LayerMask _targetLayer;
    private SpringJoint2D _springJoint2D;
    private Rigidbody2D _myRigid;
    private LineRenderer _line;
    private Vector2 MousePos;
    private Vector3 _targetColPosition = Vector3.zero;

    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _power = 5f;

    private bool _isHook;
    
    private void Awake()
    {
        _inputReader.MovementEvent += HandleMovement;
        _inputReader.HookEvent += HandleHook;
        _inputReader.MousePosEvent += HandleMousePos;

        _myRigid = GetComponent<Rigidbody2D>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        _line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        // Line Settings
        _line.positionCount = 2;
        _line.endWidth = _line.startWidth = 0.05f;
        _line.SetPosition(0, transform.position);
        _line.useWorldSpace = true;
    }

    private void HandleMousePos(Vector2 pos)
    {
        MousePos = pos;
    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= HandleMovement;
        _inputReader.HookEvent     -= HandleHook;
        _inputReader.MousePosEvent -= HandleMousePos;
    }
    
    private void HandleHook(bool isClick)
    {
        _isHook = isClick;
    }

    private void FixedUpdate()
    {
        // 새 그래플 오브젝트 탐색
        if (_isHook)
        {
            if (_targetColPosition.Equals(Vector3.zero))
            {
                _springJoint2D.enabled = true;
                _line.enabled = true;
                
                Collider2D[] _targetColliderArr = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);
                if (_targetColliderArr.Length > 0)
                {
                    // TODO : 더욱 가까이 있는 오브젝트를 찾아야 함
                    // 거리 조절을 바꾸면 될 듯
                    Vector3 targetPos = _targetColliderArr[0].transform.position;
                    float targetDistance = Vector2.Distance(targetPos, transform.position);
                    foreach (Collider2D col in _targetColliderArr)
                    {
                        float distance = Vector2.Distance(col.transform.position, transform.position);
                        if (distance < targetDistance)
                        {
                            targetDistance = distance;
                            targetPos = col.transform.position;
                        }
                    }
                    
                    _springJoint2D.connectedAnchor = targetPos;
                    _targetColPosition = targetPos;
                    // TODO : 움직임 수정 요청
                    //_myRigid.velocity = (_targetCollider.transform.position - transform.position).normalized * _power;
                    _myRigid.AddForce((targetPos - transform.position).normalized * _power, ForceMode2D.Impulse);
                }
                else
                {
                    _springJoint2D.enabled = false;
                    _targetColPosition = Vector3.zero;
                }
            }
        }
        else
        {
            // 그래플 줄을 끊었을 때
            _targetColPosition = Vector3.zero;
            _springJoint2D.enabled = false;
            _line.enabled = false;
        }
    }
    
    private void LateUpdate()
    {
        _line.SetPosition(0, transform.position);
        if(_targetColPosition.Equals(Vector3.zero) == false)
            _line.SetPosition(1, _targetColPosition);
    }

    private void HandleMovement(Vector2 movement)
    {
        // TODO : 아직은 움직임이 필요하지 않음
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
