using System;
using Blastproof.Systems.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Blastproof.Systems.PopUp
{
    public class PopUpWindow : MonoBehaviour
    {
        
        [BoxGroup("References"), SerializeField] private TextMeshProUGUI titleText;
        [BoxGroup("References"), SerializeField] private TextMeshProUGUI infoText;

        public Action onClickYes;
        public Action onClickNo;

        public void InitializePopUp(Action _onClickYes, Action _onClickNo, string title, string info)
        {
            onClickYes = _onClickYes;
            onClickNo = _onClickNo;
            titleText.text = title;
            infoText.text = info;
        }

        public void ClickYes()
        {
            onClickYes.Fire();
            Destroy(gameObject);
        }

        public void ClickNo()
        {
            onClickNo.Fire();
            Destroy(gameObject);
        }
    }
}