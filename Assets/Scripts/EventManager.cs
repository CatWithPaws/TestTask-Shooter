using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyUIEvent : UnityEvent { }
public class EventManager : MonoBehaviour
{
    public static MyUIEvent UpdateUIEvent = new MyUIEvent();
    public static UnityEvent UpdateTopListEvent = new UnityEvent();
    public static UnityEvent UsedGunsChangeEvent = new UnityEvent();
    public static UnityEvent DeadEvent = new UnityEvent();
}
