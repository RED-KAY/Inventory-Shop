using UnityEngine;

public class GenericMonoSingleton<T> : MonoBehaviour where T : GenericMonoSingleton<T>
{
    private static T m_Instance;
    public static T Instance { get { return m_Instance; } }

    protected void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = (T)this;
            DontDestroyOnLoad(m_Instance);
        }
        else
        {
            Debug.LogError("Trying to create another" + this.GetType().ToString() + " singleton!");
            Destroy(this.gameObject);
        }
    }
}