﻿#pragma checksum "..\..\..\Views\HomePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5F21F629E38D0EB013E78C23F2BBE26E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace VideoPlayer {
    
    
    /// <summary>
    /// HomePage
    /// </summary>
    public partial class HomePage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 53 "..\..\..\Views\HomePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _uiSettingsButton;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Views\HomePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _uiVideosButton;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Views\HomePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _uiLoadButton;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Views\HomePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _uiCleanButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VideoPlayer;component/views/homepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\HomePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this._uiSettingsButton = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\..\Views\HomePage.xaml"
            this._uiSettingsButton.Click += new System.Windows.RoutedEventHandler(this._uiSettingsButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this._uiVideosButton = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\Views\HomePage.xaml"
            this._uiVideosButton.Click += new System.Windows.RoutedEventHandler(this._uiVideosButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this._uiLoadButton = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\Views\HomePage.xaml"
            this._uiLoadButton.Click += new System.Windows.RoutedEventHandler(this._uiLoadButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this._uiCleanButton = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\Views\HomePage.xaml"
            this._uiCleanButton.Click += new System.Windows.RoutedEventHandler(this._uiCleanButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

