using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.PopUp
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/PopUp/PopUp System")]
    public class PopUpSystem : SerializedScriptableObject
    {
        [BoxGroup("References"), SerializeField] private PopUpWindow windowPrefab;

        public void ShowYesNoPopUp(Action onClickYes, Action onClickNo, string title, string info)
        {
            var popUpWindow = Instantiate(windowPrefab);
            popUpWindow.InitializePopUp(onClickYes, onClickNo, title, info);
        }

        #if UNITY_EDITOR
        [Button]
        public void ShowLoadGamePopUp() // Test PopUp Window
        {
            ShowYesNoPopUp(() => {Debug.Log("Clicked yes");}, () => {Debug.Log("Clicked no");}, "Load game?", "Do you want to load your game progress with level X? /n Warning: progress in the current game will be lost.");
        }
        #endif
    }
}