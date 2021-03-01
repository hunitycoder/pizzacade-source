using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using UnityEngine;

[CreateAssetMenu(menuName = "Blastproof/Variables/PlayerPrefsInt")]
public class PlayerPrefsInt : IntVariable
{
    public string _pref;
    [SerializeField] SimpleEvent _loadEvent;
    

    private void OnEnable()
    {
        _loadEvent.Subscribe(LoadVariable);
    }

    private void OnDisable()
    {
        _loadEvent.Unsubscribe(LoadVariable);
    }

    private void LoadVariable()
    {
        Value = PlayerPrefs.GetInt(_pref);
    }

    protected override void UpdateBackingField(int newValue)
    {
        base.UpdateBackingField(newValue);
        PlayerPrefs.SetInt(_pref, newValue);
    }
}
