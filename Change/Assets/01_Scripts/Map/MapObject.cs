using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    private Rigidbody2D _myRigid;
    private CircleCollider2D _myCollider;
    
    private void Awake()
    {
        _myRigid = GetComponent<Rigidbody2D>();
        _myCollider = GetComponent<CircleCollider2D>();
    }

    public void EnableCollider()
    {
        int cnt = 0;
        while (cnt != 1)
        {
            Collider2D[] result = new Collider2D[1];
            cnt = Physics2D.OverlapCircleNonAlloc(transform.position, _myCollider.radius, result, _targetLayer);
            if (cnt == 1) 
            {   
                //Destroy(GetComponent<MapObject>());
            }
        }
        _myCollider.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _myCollider.radius);
    }   
}
