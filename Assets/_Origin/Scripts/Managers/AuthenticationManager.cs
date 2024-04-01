using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance;
    private bool isAuthenticated = false;
    public UnityEvent OnAuthenticated;
    public bool IsAuthenticated 
    {
        get { return isAuthenticated; }
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SignIn();
    }
    
    async Task SignIn()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            isAuthenticated = AuthenticationService.Instance.IsSignedIn;
            OnAuthenticated.Invoke();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Sign in failed!!");
            Debug.LogException(ex);
        }
    }
}