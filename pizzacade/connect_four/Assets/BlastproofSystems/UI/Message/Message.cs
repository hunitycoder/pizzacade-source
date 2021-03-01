using System.Collections;
using Blastproof.Systems.Core;
using Blastproof.Utility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Blastproof.UI.Message
{
    public class Message : Singleton<Message>
    {
        [BoxGroup("References"), SerializeField] private RectTransform textPrefab;

        [BoxGroup("Values"), SerializeField] private float movingSize;
        [BoxGroup("Values"), SerializeField] private Vector3 direction;
        [BoxGroup("Values"), SerializeField] private float startAlpha;
        [BoxGroup("Values"), SerializeField] private float endAlpha;
        
        [Button]
        public void ShowMessage(string message, float time)
        {
            Debug.Log(message);
            StartCoroutine(StartMoving(message, time));
        }
        
        private IEnumerator StartMoving(string messageString, float movingTime)
        {
            var message = Instantiate(textPrefab, transform);
            var messageText = message.GetComponent<TextMeshProUGUI>();
            
            messageText.text = messageString; // Set text
            
            var startColor = messageText.color; // Set Color
            startColor.a = startAlpha;
            messageText.color = startColor;
            
            float time = 0;
            var messageTextPosition = message.position;
            var startPosition = messageTextPosition;
            var endPosition = messageTextPosition + direction * movingSize; 
            while (time <= movingTime)
            {
                time += Time.deltaTime;
                message.position = Vector3.Lerp(startPosition, endPosition, time / movingTime);
                var newColor = messageText.color;
                newColor.a = Mathf.Lerp(startAlpha, endAlpha, time / movingTime);
                messageText.color = newColor;
                yield return null;
            }
            Destroy(messageText.gameObject);
        }
        
    }
}