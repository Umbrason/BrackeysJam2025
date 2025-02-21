using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Authentication: MonoBehaviour
{
    private static Authentication instance = null;
    public static Authentication Instance
    {
        get => instance;
    }

    public async void ChangePlayerName(string playerName)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }
    private async void Initialize()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Initialized authentication");
    }

    private void CheckForDuplicateInstances(Scene currentScene, Scene newScene)
    {
        if (instance == null)
        {
            return;
        }

        var allInstances = FindObjectsByType<Authentication>(FindObjectsInactive.Include,
                                                                     FindObjectsSortMode.InstanceID);

        for (int i = 0; i < allInstances.Length; i++)
        {
            var current = allInstances[i];
            if (current.gameObject.GetInstanceID() == instance.gameObject.GetInstanceID())
            {
                continue;
            }
            else
            {
                Destroy(current.gameObject);
            }
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += CheckForDuplicateInstances;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= CheckForDuplicateInstances;
    }
}