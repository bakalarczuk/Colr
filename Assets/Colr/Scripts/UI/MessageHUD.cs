using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    public class MessageHUD : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text messageText;

        [Tooltip("The amount of seconds the message is shown.")]
        [SerializeField]
        private readonly float duration = 2f;

        [SerializeField]
        private Color infoColor;

        [SerializeField]
        private Color warningColor;

        [SerializeField]
        private Color positiveColor;

        [SerializeField]
        private Color negativeColor;
        public bool canShowMessage = true;


        private RectTransform _rect;
        void Start()
        {
            _rect = GetComponent<RectTransform>();
            messageText.text = "";
            messageText.enabled = false;
        }

        void OnEnable()
        {
            GameManager.OnMessageBroadcasted += ShowMessage;
            GameManager.OnNextLevel += OnNextLevel;
        }

        void OnDisable()
        {
            GameManager.OnMessageBroadcasted -= ShowMessage;
            Clear();
        }

        private void OnNextLevel(NextLevelEventArgs args)
        {
            canShowMessage = false;
        }

        private void ShowMessage(BroadcastMessageEventArgs args)
        {
            StopAllCoroutines();
            messageText.text = "";
            StartCoroutine(IEShowMessage(args.Type, args.Message));
        }

        IEnumerator IEShowMessage(MessageType type, string message)
        {
            if (canShowMessage)
            {
                switch (type)
                {
                    case MessageType.INFO:
                        messageText.color = infoColor;
                        break;
                    case MessageType.WARNING:
                        messageText.color = warningColor;
                        break;
                    case MessageType.POSITIVE:
                        messageText.color = positiveColor;
                        break;
                    case MessageType.NEGATIVE:
                        messageText.color = negativeColor;
                        break;
                }

                messageText.text = message;
                messageText.enabled = true;
                _rect.localScale = Vector3.zero;
                LeanTween.scale(_rect, Vector3.one, 0.2f).setEaseInQuad();

                yield return new WaitForSeconds(duration);

                LeanTween.scale(_rect, Vector3.zero, 0.2f).setEaseInQuad().setOnComplete(Clear);
            } else{
                canShowMessage = true;
            }
        }

        void Clear()
        {
            StopAllCoroutines();
            messageText.text = "";
            messageText.enabled = false;
        }
    }
}
