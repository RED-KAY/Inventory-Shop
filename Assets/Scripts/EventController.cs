using System;

public class EventController<T1, T2>
{
    public event Action<T1, T2> baseEvent;
    public void InvokeEvent(T1 type1, T2 type2) => baseEvent?.Invoke(type1, type2);
    public void AddListener(Action<T1, T2> listener) => baseEvent += listener;
    public void RemoveListener(Action<T1, T2> listener) => baseEvent -= listener;
}

public class EventController<T>
{
    public event Action<T> baseEvent;
    public void InvokeEvent(T type) => baseEvent?.Invoke(type);
    public void AddListener(Action<T> listener) => baseEvent += listener;
    public void RemoveListener(Action<T> listener) => baseEvent -= listener;
}

public class EventController
{
    public event Action baseEvent;
    public void InvokeEvent() => baseEvent?.Invoke();
    public void AddListener(Action listener) => baseEvent += listener;
    public void RemoveListener(Action listener) => baseEvent -= listener;

}

public class EventControllerFunc<T1, T2>
{
    public event Func<T1, T2> func2; 

    public T2 Invoke(T1 t1)
    {
        return func2.Invoke(t1);
    }

    public void AddListener(Func<T1, T2> listener) => func2 += listener;
    public void RemoveListener(Func<T1, T2> listener) => func2 -= listener;

}