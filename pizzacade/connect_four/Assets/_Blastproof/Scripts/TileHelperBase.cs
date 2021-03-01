using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TileHelperBase : MonoBehaviour
{
    private Button _button;
    private int _tileNumber;

    [SerializeField] private IntVariable _playerSelectedTile;

    public virtual void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    public void Setup(int number)
    {
        _tileNumber = number;
    }

    void OnButtonClick()
    {
        _playerSelectedTile.Value = _tileNumber;
    }

    [Button]
    public virtual void UpdateTile(int state)
    {
    }
}
