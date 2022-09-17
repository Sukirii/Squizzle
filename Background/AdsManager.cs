using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    readonly string placement = "Interstitial_Android";
    readonly string ID = "4824993";

    static float adCooldown = 30f;

    void Start()
    {
        Advertisement.Initialize(ID, false, this);
    }

    void Update()
    {
        adCooldown -= Time.deltaTime;

        adCooldown = Mathf.Clamp(adCooldown, 0, Mathf.Infinity);
    }

    public void ShowAd()
    {
        if (adCooldown > 0f)
            return;

        adCooldown = 120f;

        StartCoroutine(WaitForInitialization());
    }

    IEnumerator WaitForInitialization()
    {
        yield return new WaitUntil(() => Advertisement.isInitialized);

        Advertisement.Show(placement, this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }
    public void OnUnityAdsShowStart(string placementId)
    {

    }

    void IUnityAdsInitializationListener.OnInitializationComplete()
    {

    }

    void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }
}