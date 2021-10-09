﻿using System;
using UnityEngine;

public abstract class BaseNetworkEventSO : ScriptableObject {

    //[SerializeField] private NetworkEventKeys networkEventKey;
    //public NetworkEventKeys NetworkEventKey { get => networkEventKey; private set => networkEventKey = value; }

    public abstract void Raise(object[] data);

}

public abstract class BaseNetworkEventChannelSO : BaseNetworkEventSO {

    public Action OnEventRaised;

    public override void Raise(object[] data) {
        if (OnEventRaised == null) { return; }

        OnEventRaised.Invoke();
    }

    public void Raise() {
        Raise(null);
    }

    public void NetworkRaise() {
    }

}

public abstract class BaseNetworkEventChannelSO<T0> : BaseNetworkEventSO {

    public Action<T0> OnEventRaised;

    public override void Raise(object[] data) {
        if (OnEventRaised == null) { return; }

        OnEventRaised.Invoke((T0)data[0]);
    }

    public void NetworkRaise(T0 arg0) {
    }

}

public abstract class BaseNetworkEventChannelSO<T0, T1> : BaseNetworkEventSO {

    public Action<T0, T1> OnEventRaised;

    public override void Raise(object[] data) {
        if (OnEventRaised == null) { return; }

        OnEventRaised.Invoke((T0)data[0], (T1)data[1]);
    }

    public void NetworkRaise(T0 arg0, T1 arg1) {

    }

}

public abstract class BaseNetworkEventChannelSO<T0, T1, T2> : BaseNetworkEventSO {

    public Action<T0, T1, T2> OnEventRaised;

    public override void Raise(object[] data) {
        if (OnEventRaised == null) { return; }

        OnEventRaised.Invoke((T0)data[0], (T1)data[1], (T2)data[2]);
    }

    public void NetworkRaise(T0 arg0, T1 arg1, T2 arg2) {
    }

}