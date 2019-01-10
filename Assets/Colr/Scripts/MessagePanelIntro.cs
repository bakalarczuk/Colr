using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Habtic.Games.Colr
{


    public class MessagePanelIntro : MonoBehaviour
    {

        public Button PanelButton;
        public TMP_Text PanelText;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Exit()
        {
            Hide();
            LevelManager.Instance.LoadScene(0);
        }

    }

}
