using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Systems.UI;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : SerializedMonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private int[] _gameState = new int[9];
    [SerializeField] private IntVariable _playerSelectedTile;
    [SerializeField] private IntVariable _enemySelectedTile;
    [SerializeField] private BoolVariable _canSelectTile;
    [SerializeField] private SimpleEvent _newGame;
    [SerializeField] private StringVariable _gameStateDisplay;
    [SerializeField] private UIState endState;

    PhotonView photonView;

    private void OnEnable()
    {
        _playerSelectedTile.onValueChanged += PlayerSelectedTile;
        _enemySelectedTile.onValueChanged += EnemySelectedTile;
        _newGame.Subscribe(ResetBoard);
    }

    void PlayerSelectedTile()
    {
        if (_gameState[_playerSelectedTile.Value] != 0)
            return;
        if (!_canSelectTile.Value)
            return;
        int state = PhotonNetwork.IsMasterClient ? 1 : 2;
        photonView.RPC("ActivateTile", RpcTarget.All, _playerSelectedTile.Value, state);
    }

    void EnemySelectedTile()
    {
        //ActivateTile(_enemySelectedTile.Value);
    }

    private void OnDisable()
    {
        _playerSelectedTile.onValueChanged -= PlayerSelectedTile;
        _enemySelectedTile.onValueChanged -= EnemySelectedTile;
        _newGame.Unsubscribe(ResetBoard);
    }

    void Start()
    {
        ResetBoard();
        photonView = PhotonView.Get(this);
    }

    void ResetBoard()
    {
        for(int i=0; i<_gameState.Length; i++)
        {
            _gameState[i] = 0;
            _buttons[i].GetComponent<TileHelper>().UpdateText(0);
        }

        _gameStateDisplay.Value = "";
    }

    [PunRPC]
    public void ActivateTile(int position, int state)
    {
        _gameState[position] = state;
        _buttons[position].GetComponent<TileHelper>().UpdateText(state);
        _canSelectTile.Value = !_canSelectTile.Value;

        CheckCompletion();
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
        for(int i=0; i < _gameState.Length ;i++)
        {
            if (_gameState[i] == 0)
                return false;
        }
        return true;
    }

    void GameEnded(int winner)
    {
        _canSelectTile.Value = false;

        PhotonNetwork.LeaveRoom();
        
        int state = PhotonNetwork.IsMasterClient ? 1 : 2;

        if (winner == 0)
        {
            _gameStateDisplay.Value = "DRAW";
            return;
        }
        _gameStateDisplay.Value = winner == state ? "Win" : "Loss";
        endState.Activate();
    }

    [Button]
    void CheckCompletion()
    {
        int winner = 0;

        winner = CheckRows();

        if (winner == 0)
            winner = CheckColumns();
        if (winner == 0)
            winner = CheckDiagonals();

        if(winner != 0)
        {
            Debug.Log($"Winner {winner}");
            GameEnded(winner);
        }
        else
        {
            if(CheckIfFull())
            {
                Debug.Log("Draw");
                GameEnded(0);
            }
            Debug.Log("No winner");
        }
    }

    [Button]
    void Test()
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
