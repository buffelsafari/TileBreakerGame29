using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using BreakOut;
using Microsoft.Xna.Framework;



namespace TileBreakerGame29
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 game;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Process.SetThreadPriority(Android.OS.ThreadPriority.UrgentAudio);           


            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 0);
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 0);
            }

            SystemUiFlags flags = SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky | SystemUiFlags.Immersive; Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags; Immersive = true;

            game = new Game1();
            var gameView = (View)game.Services.GetService(typeof(View));

            // A container to show the add at the top of the page
            var adContainer = new LinearLayout(this);
            adContainer.Orientation = Orientation.Horizontal;
            adContainer.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.Top);
            adContainer.SetBackgroundColor(Android.Graphics.Color.Transparent); 

            // A layout to hold the ad container and game view
            var mainLayout = new FrameLayout(this);
            mainLayout.AddView(gameView);
            mainLayout.AddView(adContainer);

            SetContentView(mainLayout);            

            AdView bannerAd;
            
            bannerAd = new AdView(this)
            {                
                AdUnitId = "ca-app-pub-3940256099942544/6300978111",    // test add
                AdSize = AdSize.SmartBanner,                
            };

            
            game.DestroyAd = () => bannerAd.Destroy();

            game.ShowAd = () => adContainer.AddView(bannerAd);
            game.HideAd = () => adContainer.RemoveView(bannerAd);

            game.LoadAd = () => bannerAd.LoadAd(new AdRequest.Builder()
                    .AddTestDevice("9CD370D8DDF43BFE").AddTestDevice("B5D38665705D39A2").AddTestDevice(AdRequest.DeviceIdEmulator).Build());

            game.Run();

            
        }
        protected override void OnPause()
        {
            base.OnPause();
            game.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            SystemUiFlags flags = SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky | SystemUiFlags.Immersive; Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags; Immersive = true;

            game.OnResume();

        }

        


    }
}
