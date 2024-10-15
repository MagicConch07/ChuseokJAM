using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private LayerMask _targetLayer;
    private DistanceJoint2D _distanceJoint2D;
    private Rigidbody2D _myRigid;
    private LineRenderer _line;
    private Transform _targetColTrm;

    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _originPower = 5f;
    [SerializeField] private float _resistance = 15f;
    private float _currentPower;

    private bool _isHook;
    private bool _isGrappling;

    [SerializeField] private float _distance;
    
    // Compo
    private CameraZoom _cameraZoom;
    
    private void Awake()
    {
        _inputReader.MovementEvent += HandleMovement;
        _inputReader.HookEvent     += HandleHook;
        _inputReader.MousePosEvent += HandleMousePos;

        _myRigid = GetComponent<Rigidbody2D>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        _line = GetComponent<LineRenderer>();
        _cameraZoom = GetComponent<CameraZoom>();
    }

    private void Start()
    {
        // Line Settings
        _currentPower = _originPower;
        _line.positionCount = 2;
        _line.endWidth = _line.startWidth = 0.05f;
        _line.SetPosition(0, transform.position);
        _line.useWorldSpace = true;
        
        // todo : lerp
    }

    private void HandleMousePos(Vector2 pos)
    {
        //MousePos = pos;
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
        Grappling();
    
        if (_isGrappling && _targetColTrm is null == false)
        {
            Vector2 rotVec = _targetColTrm.position - transform.position;
            float rotZ = Mathf.Atan2(rotVec.y, rotVec.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            if (_currentPower > 0)
            {
                // 이거 그냥 velocity로 가보자
                Vector2 localVec = transform.InverseTransformDirection(_myRigid.velocity);
                localVec.x = _currentPower;
                localVec.y = -_currentPower;
                _myRigid.velocity = transform.TransformDirection(localVec);
            }
            _currentPower -= _resistance * Time.fixedDeltaTime;
        }
    }

    private void Grappling()
    {
        // 새 그래플 오브젝트 탐색
        if (_isHook)
        {
            if (_targetColTrm is null)
            {
                Collider2D[] _targetColliderArr = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);

                // 그래플링 성공
                if (_targetColliderArr.Length > 0)
                {
                    _distanceJoint2D.enabled = true;
                    _line.enabled = true;
                    // TODO : 더욱 가까이 있는 오브젝트를 찾아야 함
                    // 거리 조절을 바꾸면 될 듯
                    Transform targetPos = _targetColliderArr[0].transform;
                    float targetDistance = Vector2.Distance(targetPos.position, transform.position);
                    foreach (Collider2D col in _targetColliderArr)
                    {
                        float distance = Vector2.Distance(col.transform.position, transform.position);
                        if (distance < targetDistance)
                        {
                            targetDistance = distance;
                            targetPos = col.transform;
                        }
                    }

                    _distanceJoint2D.connectedAnchor = targetPos.position;
                    _distanceJoint2D.distance = _distance;
                    _targetColTrm = targetPos;
                    targetPos.GetComponent<HookBlock>().Hooking();
                    // todo : 오브젝트 없어지면 끊어지게 

                    // cam
                    _cameraZoom.ZoomAndFollow(ZoomMode.In, 0.5f, _targetColTrm);

                    _isGrappling = true;
                }
                // 그래플 실패
                else
                {
                    _distanceJoint2D.enabled = false;
                    _targetColTrm = null;
                }
            }
        }
        else
        {
            // 그래플 줄을 끊었을 때
            _currentPower = _originPower;
            _distanceJoint2D.enabled = false;
            _line.enabled = false;
            _isGrappling = false;
            _targetColTrm = null;
            
            _cameraZoom.FollowTarget(this.transform);
            _cameraZoom.Zoom(ZoomMode.Out); 
        }
    }

    private void LateUpdate()
    {
        if (_targetColTrm is null == false)
        {
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, _targetColTrm.position);
        }
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