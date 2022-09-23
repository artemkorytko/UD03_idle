using Firebase;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;

public class AnalyticSystem : MonoBehaviour
{
    public static AnalyticSystem Instance { get; private set; }

    private void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }

    public void StartGame()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
    }

    public void UpgradeBuild(string build,int level)
    {
        FirebaseAnalytics.LogEvent("Upgrade build", build, level);
    }

    public void BuyBuild()
    {
        FirebaseAnalytics.LogEvent("Buy build");
    }
}
