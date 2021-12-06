
using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using BreakOut;

namespace TileBreakerGame29
{
    class AdController
    {

        public static InterstitialAd interstitialHandle = null;
        public static IRewardedVideoAd rewardHandle = null;


        // APP ID This is a test use yours from Admob
        private static string appID = "pub-3940256099942544~f08c47fec0942fa0";  // test id
        // AD ID's This is a test use yours from Admob
        public static string adID1 = "ca-app-pub-3940256099942544/1033173712"; // standard full page ad
        
        
        

        public static bool adRegularLoaded = false;
        public static bool adRewardLoaded = false;
        public static bool adRewarded = false;
        public static bool adRewardClosed = false;
        public static bool youMadeMoney = false;

        
        public static void InitRegularAd()
        {
            MobileAds.Initialize(Game1.Activity, appID);                                                 
            interstitialHandle = new InterstitialAd((Activity1)Game1.Activity);
            interstitialHandle.AdUnitId = adID1;                                                     
            interstitialHandle.AdListener = null;
            ListeningRegular listening = new ListeningRegular();
            interstitialHandle.AdListener = listening;
            
        }
        
        public static void ShowRegularAd()
        {
            if (adRegularLoaded)
            {
                adRegularLoaded = false;
                adRewarded = false;
                interstitialHandle.Show();
            }
        }
        
        public static void InitRewardAd()
        {
            ListeningReward listening = new ListeningReward();                                          // create a pointer to our listen class      
            rewardHandle = MobileAds.GetRewardedVideoAdInstance(Game1.Activity);                        // initialize the handle      
            rewardHandle.UserId = appID;                                                                // set the App ID      
            rewardHandle.RewardedVideoAdListener = listening;                                           // point to the rewards Listen class      
            rewardHandle.LoadAd(adID1, new AdRequest.Builder().AddTestDevice("9CD370D8DDF43BFE").AddTestDevice("B5D38665705D39A2").AddTestDevice(AdRequest.DeviceIdEmulator).Build());                                 // load the first one      
        }
        
        public static void ShowRewardAd()
        {
            if (adRewardLoaded)                                                                          
            {
                adRewardLoaded = false;                                                                       
                adRewarded = false;
                adRewardClosed = false;
                rewardHandle.Show();                                                                           
            }
        }


        public static void LoadRewardAd()
        {
            AdController.rewardHandle.LoadAd(AdController.adID1, new AdRequest.Builder().AddTestDevice("9CD370D8DDF43BFE").AddTestDevice("B5D38665705D39A2").AddTestDevice(AdRequest.DeviceIdEmulator).Build());
        }
    }
    
    internal class ListeningRegular : AdListener
    {
        public override void OnAdLoaded()
        {
            AdController.adRegularLoaded = true;
            base.OnAdLoaded();
        }
        public override void OnAdClosed()
        {

            // load the next ad ready to display
            AdController.interstitialHandle.LoadAd(new AdRequest.Builder().Build());
            base.OnAdClosed();
        }
        public override void OnAdOpened()
        {
            base.OnAdOpened();
        }
    }
    
    internal class ListeningReward : AdListener, IRewardedVideoAdListener
    {
        public ListeningReward()
        {
            
        }
        public void OnRewarded(IRewardItem reward)
        {            
            AdController.adRewarded = true;            
        }
        public void OnRewardedVideoAdClosed()
        {
            AdController.adRewardClosed = true;            
            AdController.rewardHandle.LoadAd(AdController.adID1, new AdRequest.Builder().AddTestDevice("9CD370D8DDF43BFE").AddTestDevice("B5D38665705D39A2").AddTestDevice(AdRequest.DeviceIdEmulator).Build());
        }        

        public void OnRewardedVideoAdFailedToLoad(int errorCode)
        {
            
        }
        public void OnRewardedVideoAdLeftApplication()
        {
            
            AdController.youMadeMoney = true;
        }
        public void OnRewardedVideoAdLoaded()
        {
            
            AdController.adRewardLoaded = true;
        }
        public void OnRewardedVideoAdOpened()
        {
            
        }

        public void OnRewardedVideoCompleted()
        {            
            
        }

        public void OnRewardedVideoStarted()
        {            
            
        }


    }
}