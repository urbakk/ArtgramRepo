using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Authentication.Web;
using winsdkfb;
using winsdkfb.Graph;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    

    public sealed partial class MainPage : Page
    {
        int LoginStatus;    //Do sprawdzania stanu logowania (ma być w tym miejscu?) :O

        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private async void Login_Click(object sender, RoutedEventArgs e)    //Przycisk "Zaloguj"
        {
            //string SID = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString(); //SID aplikacji na Sklepie MS
            Login.Content = "Logowanie...";

            FBSession sess = FBSession.ActiveSession;
            sess.FBAppId = "230142530675566";
            sess.WinAppId = "s-1-15-2-2081801503-2397520940-272619875-1590401876-1557732794-2496746503-2085674722";

            /*if (sess.LoggedIn)
            {
                await sess.LogoutAsync(); //Wylogowanie
            }*/

            /*sess.AddPermission("public_profile"); //stary sposób! już nie działa
            sess.AddPermission("user_friends");
            sess.AddPermission("user_location");*/

            List<String> permissionList = new List<String>();
            permissionList.Add("public_profile");
            //permissionList.Add("user_friends");
            //permissionList.Add("user_likes");
            //permissionList.Add("user_location");
            permissionList.Add("user_photos");
            //permissionList.Add("publish_actions");
            FBPermissions permissions = new FBPermissions(permissionList);

            //FBResult result = await sess.LoginAsync();
            FBResult result = await sess.LoginAsync(permissions);


            if (result.Succeeded)
            {
                FBUser user = sess.User;
                UserName.Text = user.Name;

                string userId = user.Id;
                string username = user.Name;
                string locale = user.Locale;

                ProfilePic.UserId = sess.User.Id;
                //Debug.WriteLine(sess.User.Id);
                //Debug.WriteLine(sess.User.Name);

                LoginStatus = 1;
                Logout.Visibility = Visibility.Visible;     //Ukrywanie, pokazywanie przycisku zaloguj i wyloguj
                Login.Visibility = Visibility.Collapsed;
                ProfilePic.Visibility = Visibility.Visible;
                ProfilePicNone.Visibility = Visibility.Collapsed;
                Login.Content = "Zaloguj z FB";
            }
            else
            {
                //Login failed
                Login.Content = "Zaloguj z FB";
            }

        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            FBSession sess = FBSession.ActiveSession;

            //sess.FBAppId = "230142530675566";
            //sess.WinAppId = "s-1-15-2-2081801503-2397520940-272619875-1590401876-1557732794-2496746503-2085674722";

            await sess.LogoutAsync(); //Wylogowanie

            var myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookieManager = myFilter.CookieManager;
            var myCookieJar = cookieManager.GetCookies(new Uri("https://facebook.com"));
            foreach (var cookie in myCookieJar)
            {
                cookieManager.DeleteCookie(cookie);
            }

            LoginStatus = 0;
            Logout.Visibility = Visibility.Collapsed;     //Ukrywanie, pokazywanie przycisku zaloguj i wyloguj
            Login.Visibility = Visibility.Visible;
            ProfilePic.Visibility = Visibility.Collapsed;
            ProfilePicNone.Visibility = Visibility.Visible;

            UserName.Text = "Nie zalogowano";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
