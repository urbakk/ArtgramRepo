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
using winsdkfb;
using winsdkfb.Graph;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Artgram
{
    public sealed partial class AppBar : UserControl
    {
        //bool logged_in;

        Frame customFrame = Window.Current.Content as Frame;

        public AppBar()
        {
            this.InitializeComponent();

            FBSession sess = FBSession.ActiveSession;
            sess.FBAppId = "230142530675566";
            sess.WinAppId = "s-1-15-2-2081801503-2397520940-272619875-1590401876-1557732794-2496746503-2085674722";

            if (sess.User != null)
            {
                FBUser user = sess.User;
                UserName.Text = user.Name;

                string userId = user.Id;
                string username = user.Name;
                string locale = user.Locale;

                ProfilePic.UserId = sess.User.Id;
                //Debug.WriteLine(sess.User.Id);
                //Debug.WriteLine(sess.User.Name);

                //LoginStatus = 1;
                Logout.Visibility = Visibility.Visible;     //Ukrywanie, pokazywanie przycisku zaloguj i wyloguj
                Login.Visibility = Visibility.Collapsed;
                ProfilePic.Visibility = Visibility.Visible;
                ProfilePicNone.Visibility = Visibility.Collapsed;
                Login.Content = "Zaloguj z FB";

            }
        }

        private void button_Logo_Click(object sender, RoutedEventArgs e)
        {
            //Button b = sender as Button;

            customFrame.Navigate(typeof(MainPage));

            /*//Ze sprawdzaniem zawartości strony
            if (b != null && b.Tag != null)
            {
                Type pageType = Type.GetType(b.Tag.ToString());

                // Make sure the page type exists, but don't navigate to it if it's already the current page.
                if (pageType != null && rootFrame.CurrentSourcePageType != pageType)
                {
                    
                    rootFrame.Navigate(typeof(MainPage));
                    //(App.Current as App).navigate;
                }
                else if (pageType == null)
                {
                    // TODO: Optional - Do something if page not found.
                }
            }*/
        }

        private void button_Add_Click(object sender, RoutedEventArgs e)
        {
            customFrame.Navigate(typeof(Add));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Login_Click(object sender, RoutedEventArgs e)
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
            permissionList.Add("user_photos");
            //permissionList.Add("user_friends");
            //permissionList.Add("user_likes");
            //permissionList.Add("user_location");
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

                //LoginStatus = 1;
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

            //LoginStatus = 0;
            Logout.Visibility = Visibility.Collapsed;     //Ukrywanie, pokazywanie przycisku zaloguj i wyloguj
            Login.Visibility = Visibility.Visible;
            ProfilePic.Visibility = Visibility.Collapsed;
            ProfilePicNone.Visibility = Visibility.Visible;

            UserName.Text = "Nie zalogowano";
        }
    }
}
