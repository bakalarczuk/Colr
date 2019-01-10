using System.Collections;
using System.Collections.Generic;
using Habtic.Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using UniRx;

namespace Habtic.YUI
{
    public class YUILocalizedText : YUIText
    {
        [SerializeField] protected string _localizationKey;
        [SerializeField] protected bool setTextOnStart;

        private bool firstBoot = true;

        // private void Start()
        // {
        //     if (setTextOnStart)
        //     {
        //         MessageBroker.Default
        //                         .Receive<ApplicationStateChanged>()
        //                         .Where(s => s.newState == Core.ApplicationState.PostBoot)
        //                         .Subscribe(stateChange => SetLocalizedString());
        //     }
        // }

        // private void OnEnable()
        // {
        //     ManagerLocator.LocalizationManager.LanguageChanged += OnLanguageChanged;
        //     if (!firstBoot)
        //     {
        //         SetLocalizedString();
        //     }
        //     firstBoot = false;
        // }

        // private void OnDisable()
        // {
        //     ManagerLocator.LocalizationManager.LanguageChanged -= OnLanguageChanged;
        // }

        // public void SetTextKey(string key)
        // {
        //     _localizationKey = key;
        //     SetLocalizedString();
        // }

        // private void OnLanguageChanged(string language)
        // {
        //     SetLocalizedString();
        // }

        // private void SetLocalizedString()
        // {
        //     if(!string.IsNullOrEmpty(_localizationKey))
        //     {
        //         string localizedString = ManagerLocator.LocalizationManager.GetLocalizedString(_localizationKey);
        //         if(localizedString != _localizationKey)
        //             TextComponent.text = localizedString;
        //     }
        // }

        public string GetLocalizationKey() {
            return _localizationKey;
        }
    }
}
