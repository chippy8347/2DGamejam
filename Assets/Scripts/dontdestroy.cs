using UnityEngine;

public class PersistentAudioManager : MonoBehaviour
{
    private static PersistentAudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed when a new scene is loaded
            GetComponent<AudioSource>().Play(); // Start playing the audio
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate instances
        }
    }
}