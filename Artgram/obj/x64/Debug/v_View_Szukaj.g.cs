﻿#pragma checksum "C:\Users\urbak\Documents\GitHub\ArtgramRepo\Artgram\v_View_Szukaj.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0A21DFD11F56AFE083C9B674C2DCE9E8"
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
    partial class v_View_Szukaj : 
        global::Windows.UI.Xaml.Controls.Page, 
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
                    this.button = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 2:
                {
                    this.textBlock_nazwa = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.button_Wow = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 18 "..\..\..\v_View_Szukaj.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button_Wow).Click += this.button_Wow_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.button_Contact = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 5:
                {
                    this.button_Report = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 6:
                {
                    this.textBlock_opis = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.textBlock_WOW = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.button_Powrot = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 28 "..\..\..\v_View_Szukaj.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button_Powrot).Click += this.button_Powrot_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.textBlock_ID_Obrazu1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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

