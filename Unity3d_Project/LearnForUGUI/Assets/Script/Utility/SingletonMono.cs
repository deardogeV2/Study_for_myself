using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T:class
{
    private static T instance;

    public static T Instance
    {
        get { return SingletonMono<T>.instance; }
    }
	public virtual void Awake () {
        instance = this as T;
	}
}
