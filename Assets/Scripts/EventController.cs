using System;

public class EventController<T1, T2>
{
    public event Action<T1, T2> m_BaseEvent;
    public void InvokeEvent(T1 type1, T2 type2) => m_BaseEvent?.Invoke(type1, type2);
    public void AddListener(Action<T1, T2> listener) => m_BaseEvent += listener;
    public void RemoveListener(Action<T1, T2> listener) => m_BaseEvent -= listener;
}

public class EventController<T>
{
    public event Action<T> m_BaseEvent;
    public void InvokeEvent(T type) => m_BaseEvent?.Invoke(type);
    public void AddListener(Action<T> listener) => m_BaseEvent += listener;
    public void RemoveListener(Action<T> listener) => m_BaseEvent -= listener;
}

public class EventController
{
    public event Action m_BaseEvent;
    public void InvokeEvent() => m_BaseEvent?.Invoke();
    public void AddListener(Action listener) => m_BaseEvent += listener;
    public void RemoveListener(Action listener) => m_BaseEvent -= listener;

}

public class EventControllerFunc<T1, T2>
{
    public event Func<T1, T2> m_Func2; 

    public T2 Invoke(T1 t1)
    {
        return m_Func2.Invoke(t1);
    }

    public void AddListener(Func<T1, T2> listener) => m_Func2 += listener;
    public void RemoveListener(Func<T1, T2> listener) => m_Func2 -= listener;

}