﻿#pragma checksum "C:\Users\Mateusz\Source\Repos\ArtgramRepo\Artgram\v_Add.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "16C8B66EAF6907ADD8ACABFE4EB0D0E6"
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
    partial class Add : 
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
                    this.button_Add = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 3:
                {
                    this.comboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 4:
                {
                    this.textBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5:
                {
                    this.textBox_Copy = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6:
                {
                    this.button_Cancel = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 20 "..\..\..\v_Add.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button_Cancel).Click += this.button_Cancel_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.button_Accept = (global::Windows.UI.Xaml.Controls.Button)(target);
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

