using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // 감정적으로 하지마라
    // 제발 현실적으로 봐
    // 지금 공부시간을 파악해
    // 냉정해져라
    // 냉철해져라
    // 직시하라
    
    // 구상 빨리 끝내라
    // 저 콜라이더 처럼 영역을 만들다
    // 영역 안에 랜덤하게 오브젝트를 생성한다
    // 오브젝트에게 콜라이더를 늘려서 퍼지게 만든다
    // 그리고 주변 오브젝트 삭제 -> 위에서 영역 안 랜덤 오브젝트를 생성할꺼면 굳이 콜라이더를 펄져서 앵커로 만들 이유가 없는데
    
    // 다시 게임을 구상
    
    // TODO : 영역 만들기
    // TODO : 그냥 하기 그냥 만들기 Just Do it

    [SerializeField] private Transform _playerTrm;

    [SerializeField] private float _weigthX = 30f;
    private float _mapX;
    
    private void Update()
    {
        if (_playerTrm.position.x > _mapX)
        {
            GenerateMap();
            _mapX = _playerTrm.position.x + _weigthX;
        }
    }

    private void GenerateMap()
    {
        // TODO : OBJ 생성
        
        
    }
}
