using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static T instance = null;
    static public T Instance{
        get {
            if (instance == null)
                Debug.LogError($"Null sinleton of {nameof(T)}\n Check attached any component or awake is overrieded");
            return instance;
        }
    }
    void Awake(){
        if (instance != null)
            Debug.LogError($"Multiple sinleton of {nameof(T)}");
        instance = (T)this;
        instance.Init();
        DontDestroyOnLoad(gameObject);
    }
    public virtual void Init(){}
}