using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class GameManager : SerializedMonoBehaviour
{
    [SerializeField,ShowInInspector]
    Dictionary<string, GameObject> _games = new Dictionary<string, GameObject>();
    private GameObject _currentGame;

    [SerializeField] private Image _grid;
    [SerializeField] private StringVariable _selectedGame;

    private void OnEnable()
    {
        _selectedGame.onValueChanged += SelectGame;
    }

    private void OnDisable()
    {
        _selectedGame.onValueChanged -= SelectGame;
    }

    private void Start()
    {
        //_selectedGame.Value = "m";
        //_selectedGame.Value = "m";
    }

    void SelectGame()
    {
        if (_currentGame)
            DestroyImmediate(_currentGame);

        Debug.Log($"Selecting game {_selectedGame.Value}");

        _currentGame = Instantiate(_games["m"]);
        _grid.color = _currentGame.GetComponent<GameLogicBase>().GetGridColor();
    }
}
