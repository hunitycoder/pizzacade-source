using System;
using System.Collections;
using System.Collections.Generic;
using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using UnityEngine;

public class ReactiveStringEmptyEnabler : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private StringVariable _enabler;

    private void OnEnable()
    {
        EnableThis();

        _enabler.onValueChanged += EnableThis;
    }


    private void OnDisable()
    {
        _enabler.onValueChanged -= EnableThis;
    }

    private void EnableThis()
    {
        _content.SetActive(_enabler.Value.EmptyOrNull());
    }
}
