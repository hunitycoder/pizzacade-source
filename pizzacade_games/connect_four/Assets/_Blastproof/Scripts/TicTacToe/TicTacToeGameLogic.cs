using Photon.Pun;
using UnityEngine;

public class TicTacToeGameLogic : GameLogicBase
{
    protected override void PlayerSelectedTile()
    {
        if (_gameState[_playerSelectedTile.Value] != 0)
            return;
        
        base.PlayerSelectedTile();
    }

    int CheckRows()
    {
        int pos1 = 0;
        int pos2 = 0;
        int pos3 = 0;
        int result = 0;
        for (int i = 0; i < _gameState.Length; i += 3)
        {
            pos1 = _gameState[i];
            pos2 = _gameState[i + 1];
            pos3 = _gameState[i + 2];

            if (pos1 == pos2 && pos2 == pos3 && pos1 != 0)
            {
                result = pos1;
                break;
            }
        }
        return result;
    }

    int CheckColumns()
    {
        int pos1 = 0;
        int pos2 = 0;
        int pos3 = 0;
        int result = 0;

        for (int i = 0; i < _gameState.Length / 3; i++)
        {
            pos1 = _gameState[i];
            pos2 = _gameState[i + 3];
            pos3 = _gameState[i + 6];

            if (pos1 == pos2 && pos2 == pos3 && pos1 != 0)
            {
                result = pos1;
                break;
            }
        }
        return result;
    }

    int CheckDiagonals()
    {
        int pos1 = _gameState[0];
        int pos2 = _gameState[4];
        int pos3 = _gameState[8];

        if (pos1 == pos2 && pos2 == pos3 && pos1 != 0)
            return pos1;

        pos1 = _gameState[2];
        pos2 = _gameState[4];
        pos3 = _gameState[6];

        if ((pos1 == pos2) && pos2 == pos3 && pos1 != 0)
            return pos1;

        return 0;
    }

    bool CheckIfFull()
    {
        for (int i = 0; i < _gameState.Length; i++)
        {
            if (_gameState[i] == 0)
                return false;
        }
        return true;
    }

    protected override void CheckCompletion()
    {
        int winner = CheckRows();

        if (winner == 0)
            winner = CheckColumns();
        if (winner == 0)
            winner = CheckDiagonals();

        if (winner != 0)
        {
            Debug.Log($"Winner {winner}");
            GameEnded(winner);
        }
        else
        {
            if (CheckIfFull())
            {
                Debug.Log("Draw");
                GameEnded(0);
            }
            Debug.Log("No winner");
        }
    }

    protected override void Test()
    {
        _gameState = new int[] { 1, 1, 1, 0, 2, 0, 0, 2, 0 };

        CheckCompletion();

        _gameState = new int[] { 0, 0, 0, 1, 1, 1, 0, 2, 0 };

        CheckCompletion();

        _gameState = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1 };

        CheckCompletion();

        _gameState = new int[] { 1, 0, 0, 0, 1, 0, 0, 2, 1 };

        CheckCompletion();

        _gameState = new int[] { 1, 0, 2, 0, 2, 0, 2, 2, 1 };

        CheckCompletion();

        _gameState = new int[] { 2, 0, 2, 2, 2, 0, 2, 2, 1 };

        CheckCompletion();

        _gameState = new int[] { 0, 2, 2, 0, 2, 0, 0, 2, 1 };

        CheckCompletion();

        _gameState = new int[] { 0, 0, 2, 0, 2, 2, 0, 2, 2 };

        CheckCompletion();

        _gameState = new int[] { 0, 0, 0, 0, 2, 2, 0, 0, 2 };

        CheckCompletion();

        _gameState = new int[] { 2, 0, 0, 0, 0, 0, 1, 0, 0 };

        CheckCompletion();
    }

}


//TODO create base class
public class GameLogic 
{
    // [SerializeField] private Button[] _buttons;
    // [SerializeField] private int[] _gameState;
    // [SerializeField] private IntVariable _playerSelectedTile;
    // [SerializeField] private IntVariable _enemySelectedTile;
    // [SerializeField] private BoolVariable _canSelectTile;
    // [SerializeField] private SimpleEvent _newGame;
    // [SerializeField] private StringVariable _gameStateDisplay;
    // [SerializeField] private UIState _endState;
    // [SerializeField] private GameObject _tileButton;
    // [SerializeField] private Vector2 _gridSize;
    // [SerializeField] private Vector2 _cellSize;
    // [SerializeField] private Vector3 _gridOffset;
    //
    // PhotonView photonView;
    //
    // private void OnEnable()
    // {
    //     _playerSelectedTile.onValueChanged += PlayerSelectedTile;
    //     _enemySelectedTile.onValueChanged += EnemySelectedTile;
    //     _newGame.Subscribe(ResetBoard);
    //
    //     //setup game
    //     GameObject grid = GameObject.FindGameObjectWithTag("GameGrid");
    //     RectTransform rectTransform = grid.GetComponent<RectTransform>();
    //     rectTransform.sizeDelta = new Vector2(_gridSize.x * _cellSize.x, _gridSize.y * _cellSize.y);
    //     rectTransform.anchoredPosition = _gridOffset;
    //     GridLayoutGroup gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
    //
    //     gridLayoutGroup.cellSize = _cellSize;
    //     gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    //     gridLayoutGroup.constraintCount = _gridSize.x.ToInt();
    //
    //     int gridSize = Mathf.RoundToInt(_gridSize.x * _gridSize.y);
    //
    //     _buttons = new Button[gridSize];
    //     _gameState = new int[gridSize];
    //
    //     for (int i = 0; i < gridSize; i++)
    //     {
    //         _buttons[i] = Instantiate(_tileButton, grid.transform).GetComponent<Button>();
    //         _buttons[i].GetComponent<TileHelperBase>().Setup(i);
    //     }
    // }
    //
    // void PlayerSelectedTile()
    // {
    //     if (_gameState[_playerSelectedTile.Value] != 0)
    //         return;
    //     if (!_canSelectTile.Value)
    //         return;
    //     int state = PhotonNetwork.IsMasterClient ? 1 : 2;
    //     photonView.RPC("ActivateTile", RpcTarget.All, _playerSelectedTile.Value, state);
    // }
    //
    // void EnemySelectedTile()
    // {
    //     //ActivateTile(_enemySelectedTile.Value);
    //     for (int i = 0; i < _buttons.Length; i++)
    //     {
    //         Destroy(_buttons[i]);
    //     }
    // }
    //
    // private void OnDisable()
    // {
    //     _playerSelectedTile.onValueChanged -= PlayerSelectedTile;
    //     _enemySelectedTile.onValueChanged -= EnemySelectedTile;
    //     _newGame.Unsubscribe(ResetBoard);
    // }
    //
    // void Start()
    // {
    //     ResetBoard();
    //     photonView = PhotonView.Get(this);
    // }
    //
    // void ResetBoard()
    // {
    //     for (int i = 0; i < _gameState.Length; i++)
    //     {
    //         _gameState[i] = 0;
    //         _buttons[i].GetComponent<TileHelperBase>().UpdateTile(0);
    //     }
    //
    //     _gameStateDisplay.Value = "";
    // }
    //
    // [PunRPC]
    // public void ActivateTile(int position, int state)
    // {
    //     _gameState[position] = state;
    //     _buttons[position].GetComponent<TileHelperBase>().UpdateTile(state);
    //     _canSelectTile.Value = !_canSelectTile.Value;
    //
    //     CheckCompletion();
    // }
    //
    //
    //
    // void GameEnded(int winner)
    // {
    //     _canSelectTile.Value = false;
    //
    //     PhotonNetwork.LeaveRoom();
    //
    //     int state = PhotonNetwork.IsMasterClient ? 1 : 2;
    //
    //     if (winner == 0)
    //     {
    //         _gameStateDisplay.Value = "DRAW";
    //         return;
    //     }
    //     _gameStateDisplay.Value = winner == state ? "Win" : "Loss";
    //     _endState.Activate();
    // }
    //
    // [Button]
    
}
