using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;
public class AdsControl : MonoBehaviour
{
    public string androidInterstitial, iosInterstitial;
    public string androidBanner, iosBanner;
    public float adPeriod = 60;

    private float lastShowTime = float.MinValue;

	protected AdsControl ()
	{
	}
	
	private static AdsControl _instance;
	private InterstitialAd interstitial;
    private BannerView bannerView;

	public static AdsControl Instance { get {
			return _instance;
		} }
	
	void Awake ()
	{
		if (FindObjectsOfType (typeof(AdsControl)).Length > 1) {
			Destroy (gameObject);
			return;
		}
		
		_instance = this;

        RequestBanner();
		RequestInterstitial ();

		DontDestroyOnLoad (gameObject); //Already done by CBManager
	}

    public void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = androidBanner.Trim();
#elif UNITY_IPHONE
        string adUnitId = iosBanner.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
    }

    void RequestInterstitial ()
	{
#if UNITY_ANDROID
		interstitial = new InterstitialAd (androidInterstitial.Trim());
#endif
#if UNITY_IPHONE
		interstitial = new InterstitialAd (iosInterstitial.Trim());
#endif
        //interstitial.OnAdClosed += HandleInterstialAdClosed;
		//AdRequest request = new AdRequest.Builder ().Build ();
		//interstitial.LoadAd (request);
	}

	public void showAds ()
	{
        if (Time.time - lastShowTime > adPeriod)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
                lastShowTime = Time.time;
            }
        }
	}

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        HideBannerAds();
        print("HandleAdLoaded event received.");
    }

    public void HandleInterstialAdClosed(object sender, EventArgs args)
    {

        if (interstitial != null)
            interstitial.Destroy();
        RequestInterstitial();
    }

    public void HideBannerAds ()
	{
        bannerView.Hide();
    }

	public void ShowBannerAds ()
	{
        bannerView.Show();
    }
}

