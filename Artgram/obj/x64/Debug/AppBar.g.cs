﻿#pragma checksum "C:\Users\Mateusz\Documents\GitHub\ArtgramRepo\Artgram\AppBar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "32C4E6DF61C074AA4DCA1E41D5BE7DE8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Artgram
{
    partial class AppBar : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.button_Logo = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 12 "..\..\..\AppBar.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button_Logo).Click += this.button_Logo_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.ProfilePic = (global::winsdkfb.ProfilePictureControl)(target);
                }
                break;
            case 3:
                {
                    this.ProfilePicNone = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 4:
                {
                    this.UserName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.button_Add = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\AppBar.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button_Add).Click += this.button_Add_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.Login = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\AppBar.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Login).Click += this.Login_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.Logout = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\AppBar.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Logout).Click += this.Logout_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.textBox2 = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9:
                {
                    this.button1 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 25 "..\..\..\AppBar.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button1).Click += this.button1_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

