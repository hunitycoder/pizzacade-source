using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Systems.UI;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameLogicBase : SerializedMonoBehaviour
{
    [SerializeField] protected Button[] _buttons;
    [SerializeField] protected int[] _gameState;
    [SerializeField] protected IntVariable _playerSelectedTile;
    [SerializeField] protected IntVariable _enemySelectedTile;
    [SerializeField] protected BoolVariable _canSelectTile;
    [SerializeField] protected SimpleEvent _newGame;
    [SerializeField] protected StringVariable _gameStateDisplay;
    [SerializeField] protected UIState _endState;
    [SerializeField] protected GameObject _tileButton;
    [SerializeField] protected Vector2Int _gridSize;
    [SerializeField] protected Vector2 _cellSize;
    [SerializeField] protected Vector3 _gridOffset;
    [SerializeField] protected Color _gridColor;

    protected PhotonView photonView;

    protected virtual void OnEnable()
    {
        _playerSelectedTile.onValueChanged += PlayerSelectedTile;
        _enemySelectedTile.onValueChanged += EnemySelectedTile;
        _newGame.Subscribe(ResetBoard);

        //setup game
        GameObject grid = GameObject.FindGameObjectWithTag("GameGrid");
        RectTransform rectTransform = grid.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(_gridSize.x * _cellSize.x, _gridSize.y * _cellSize.y);
        rectTransform.anchoredPosition = _gridOffset;
        GridLayoutGroup gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();

        gridLayoutGroup.cellSize = _cellSize;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = _gridSize.x;
        
        int gridSize = Mathf.RoundToInt(_gridSize.x * _gridSize.y);

        _buttons = new Button[gridSize];
        _gameState = new int[gridSize];

        for (int i = 0; i < grid.transform.childCount; i++)
        {
            Destroy(grid.transform.GetChild(i).gameObject);
        }
        
        for(int i=0; i< gridSize; i++)
        {
            _buttons[i] = Instantiate(_tileButton, grid.transform).GetComponent<Button>();
            _buttons[i].GetComponent<TileHelperBase>().Setup(i);
        }
    }
    
    private void OnDisable()
    {
        _playerSelectedTile.onValueChanged -= PlayerSelectedTile;
        _enemySelectedTile.onValueChanged -= EnemySelectedTile;
        _newGame.Unsubscribe(ResetBoard);
    }

    public virtual void Start()
    {
        ResetBoard();
        photonView = PhotonView.Get(this);
        photonView.ViewID = 2;
    }

    protected virtual void PlayerSelectedTile()
    {
        Debug.Log($"State at selected position: {_gameState[_playerSelectedTile.Value]} ------ Can Select? = {_canSelectTile.Value}");
        if (!_canSelectTile.Value)
            return;
        int state = PhotonNetwork.IsMasterClient ? 1 : 2;
        //ActivateTile(_playerSelectedTile.Value, state);

        photonView.RPC("ActivateTile", RpcTarget.All, _playerSelectedTile.Value, state);

    }

    protected virtual void EnemySelectedTile()
    {
        //ActivateTile(_enemySelectedTile.Value);
        // foreach (Button button in _buttons)
        //     Destroy(button);
    }
    
    protected virtual void ResetBoard()
    {
        for(int i=0; i<_gameState.Length; i++)
        {
            _gameState[i] = 0;
            _buttons[i].GetComponent<TileHelperBase>().UpdateTile(0);
        }

        _gameStateDisplay.Value = "";
    }

    protected virtual void GameEnded(int winner)
    {
        _canSelectTile.Value = false;

        PhotonNetwork.LeaveRoom();

        int state = PhotonNetwork.IsMasterClient ? 1 : 2;

        if (winner == 0)
            _gameStateDisplay.Value = "DRAW";
        
        _gameStateDisplay.Value = winner == state ? "Win" : "Loss";
    }

    public Color GetGridColor() => _gridColor;

    [PunRPC]
    public virtual void ActivateTile(int position, int state)
    {
        _gameState[position] = state;
        _buttons[position].GetComponent<TileHelperBase>().UpdateTile(state);
        _canSelectTile.Value = !_canSelectTile.Value;

        CheckCompletion();
    }

    [Button]
    protected virtual void CheckCompletion() { }

    [Button]
    protected virtual void Test() { }

    // void OnDestroy()
    // {
    //     int gridSize = Mathf.RoundToInt(_gridSize.x * _gridSize.y);
    //
    //     for (int i = 0; i < gridSize; i++)
    //     {
    //         Destroy(_buttons[i]);
    //     }
    // }
    
}
