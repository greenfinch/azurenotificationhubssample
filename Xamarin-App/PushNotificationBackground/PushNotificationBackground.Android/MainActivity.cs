using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using Firebase;
using Firebase.Iid;

namespace PushNotificationBackground.Droid
{
    [Activity(Label = "PushNotificationBackground", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static readonly string CHANNEL_ID = "pushNotificaitonsChannelId";
        internal static readonly int NOTIFICATION_ID = 100;

        static readonly string TAG = "MainActivity";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(savedInstanceState);
                if (Intent.Extras != null)
                {
                    foreach (var key in Intent.Extras.KeySet())
                    {
                        if (key != null)
                        {
                            var value = Intent.Extras.GetString(key);
                            Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                        }
                    }
                }

                IsPlayServicesAvailable();

                CreateNotificationChannel();

                FirebaseApp app = FirebaseApp.InitializeApp(Android.App.Application.Context);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
                LoadApplication(new App());
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    // msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                {
                    //Log.Debug(TAG, " Error: {1}", GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else
                {
                    //msgText.Text = "This device is not supported";
                    //Log.Debug(TAG, " Error: This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
               // msgText.Text = "Google Play Services is available.";
                return true;
            }
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                "FCM Notifications",
                NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);

        }

    }
}