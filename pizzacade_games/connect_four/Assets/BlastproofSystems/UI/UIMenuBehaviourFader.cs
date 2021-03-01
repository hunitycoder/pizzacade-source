using System.Collections;
using System.Collections.Generic;
using Blastproof.Systems.UI;
using Blastproof.Tools.Elements;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIMenuBehaviourFader : ImageElement
{
    [SerializeField] private float _darkAlpha = .5f;

    [SerializeField] private bool _overrideBehaviour;
    [SerializeField] private UIBehaviour _overridenBehaviour;

    private UIBehaviour _behaviour;
    [BoxGroup("Info"), ShowInInspector, ReadOnly] protected UIBehaviour _Behaviour =>
        _overrideBehaviour ? _overridenBehaviour : _behaviour ?? (_behaviour = GetComponentInParent<UIBehaviour>());

    protected virtual void OnEnable()
    {
        _Behaviour.onOpen += OnOpen;
        _Behaviour.onClose += OnClose;
    }

    protected virtual void OnDisable()
    {
        _Behaviour.onOpen -= OnOpen;
        _Behaviour.onClose -= OnClose;
    }

    protected void OnOpen()
    {
        ThisImage.raycastTarget = true;
        var tween = ThisImage.DOFade(_darkAlpha, .33f);
    }
    protected void OnClose()
    {
        var tween = ThisImage.DOFade(0, .33f);
        tween.onComplete += () =>
        {
            tween.onComplete = null;
            ThisImage.raycastTarget = false;
        };
    }
}
