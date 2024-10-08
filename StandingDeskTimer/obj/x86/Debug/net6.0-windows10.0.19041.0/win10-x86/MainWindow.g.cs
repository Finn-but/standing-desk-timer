﻿#pragma checksum "D:\GitProjects\BUT\standing-desk-timer\StandingDeskTimer\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CDD29694D52EFB6197F0E280A0969F0A8347B7F9697D6352CDEC657480B7DA63"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StandingDeskTimer
{
    partial class MainWindow : 
        global::Microsoft.UI.Xaml.Window, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Microsoft_UI_Xaml_Controls_NumberBox_Value(global::Microsoft.UI.Xaml.Controls.NumberBox obj, global::System.Double value)
            {
                obj.Value = value;
            }
            public static void Set_Microsoft_UI_Xaml_Controls_ToggleMenuFlyoutItem_IsChecked(global::Microsoft.UI.Xaml.Controls.ToggleMenuFlyoutItem obj, global::System.Boolean value)
            {
                obj.IsChecked = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class MainWindow_obj1_Bindings :
            global::Microsoft.UI.Xaml.Markup.IDataTemplateComponent,
            global::Microsoft.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Microsoft.UI.Xaml.Markup.IComponentConnector,
            IMainWindow_Bindings
        {
            private global::StandingDeskTimer.MainWindow dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Microsoft.UI.Xaml.Controls.NumberBox obj10;
            private global::Microsoft.UI.Xaml.Controls.NumberBox obj11;
            private global::Microsoft.UI.Xaml.Controls.ToggleMenuFlyoutItem obj14;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj10ValueDisabled = false;
            private static bool isobj11ValueDisabled = false;
            private static bool isobj14IsCheckedDisabled = false;

            private MainWindow_obj1_BindingsTracking bindingsTracking;

            public MainWindow_obj1_Bindings()
            {
                this.bindingsTracking = new MainWindow_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 34 && columnNumber == 77)
                {
                    isobj10ValueDisabled = true;
                }
                else if (lineNumber == 37 && columnNumber == 77)
                {
                    isobj11ValueDisabled = true;
                }
                else if (lineNumber == 14 && columnNumber == 55)
                {
                    isobj14IsCheckedDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 10: // MainWindow.xaml line 34
                        this.obj10 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NumberBox>(target);
                        this.bindingsTracking.RegisterTwoWayListener_10(this.obj10);
                        break;
                    case 11: // MainWindow.xaml line 37
                        this.obj11 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NumberBox>(target);
                        this.bindingsTracking.RegisterTwoWayListener_11(this.obj11);
                        break;
                    case 14: // MainWindow.xaml line 14
                        this.obj14 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ToggleMenuFlyoutItem>(target);
                        this.bindingsTracking.RegisterTwoWayListener_14(this.obj14);
                        break;
                    default:
                        break;
                }
            }
                        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
                        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target) 
                        {
                            return null;
                        }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                nextPhase = -1;
            }

            public void Recycle()
            {
                return;
            }

            // IMainWindow_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = global::WinRT.CastExtensions.As<global::StandingDeskTimer.MainWindow>(newDataRoot);
                    return true;
                }
                return false;
            }

            public void Activated(object obj, global::Microsoft.UI.Xaml.WindowActivatedEventArgs data)
            {
                this.Initialize();
            }

            public void Loading(global::Microsoft.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::StandingDeskTimer.MainWindow obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_StandingValue(obj.StandingValue, phase);
                        this.Update_SittingValue(obj.SittingValue, phase);
                        this.Update_EnableTripleTwenty(obj.EnableTripleTwenty, phase);
                    }
                }
            }
            private void Update_StandingValue(global::System.Int32 obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainWindow.xaml line 34
                    if (!isobj10ValueDisabled)
                    {
                        XamlBindingSetters.Set_Microsoft_UI_Xaml_Controls_NumberBox_Value(this.obj10, obj);
                    }
                }
            }
            private void Update_SittingValue(global::System.Int32 obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainWindow.xaml line 37
                    if (!isobj11ValueDisabled)
                    {
                        XamlBindingSetters.Set_Microsoft_UI_Xaml_Controls_NumberBox_Value(this.obj11, obj);
                    }
                }
            }
            private void Update_EnableTripleTwenty(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // MainWindow.xaml line 14
                    if (!isobj14IsCheckedDisabled)
                    {
                        XamlBindingSetters.Set_Microsoft_UI_Xaml_Controls_ToggleMenuFlyoutItem_IsChecked(this.obj14, obj);
                    }
                }
            }
            private void UpdateTwoWay_10_Value()
            {
                if (this.initialized)
                {
                    if (this.dataRoot != null)
                    {
                        this.dataRoot.StandingValue = (global::System.Int32)this.obj10.Value;
                    }
                }
            }
            private void UpdateTwoWay_11_Value()
            {
                if (this.initialized)
                {
                    if (this.dataRoot != null)
                    {
                        this.dataRoot.SittingValue = (global::System.Int32)this.obj11.Value;
                    }
                }
            }
            private void UpdateTwoWay_14_IsChecked()
            {
                if (this.initialized)
                {
                    if (this.dataRoot != null)
                    {
                        this.dataRoot.EnableTripleTwenty = this.obj14.IsChecked;
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class MainWindow_obj1_BindingsTracking
            {
                private global::System.WeakReference<MainWindow_obj1_Bindings> weakRefToBindingObj; 

                public MainWindow_obj1_BindingsTracking(MainWindow_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<MainWindow_obj1_Bindings>(obj);
                }

                public MainWindow_obj1_Bindings TryGetBindingObject()
                {
                    MainWindow_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                }

                public void RegisterTwoWayListener_10(global::Microsoft.UI.Xaml.Controls.NumberBox sourceObject)
                {
                    sourceObject.RegisterPropertyChangedCallback(global::Microsoft.UI.Xaml.Controls.NumberBox.ValueProperty, (sender, prop) =>
                    {
                        var bindingObj = this.TryGetBindingObject();
                        if (bindingObj != null)
                        {
                            bindingObj.UpdateTwoWay_10_Value();
                        }
                    });
                }
                public void RegisterTwoWayListener_11(global::Microsoft.UI.Xaml.Controls.NumberBox sourceObject)
                {
                    sourceObject.RegisterPropertyChangedCallback(global::Microsoft.UI.Xaml.Controls.NumberBox.ValueProperty, (sender, prop) =>
                    {
                        var bindingObj = this.TryGetBindingObject();
                        if (bindingObj != null)
                        {
                            bindingObj.UpdateTwoWay_11_Value();
                        }
                    });
                }
                public void RegisterTwoWayListener_14(global::Microsoft.UI.Xaml.Controls.ToggleMenuFlyoutItem sourceObject)
                {
                    sourceObject.RegisterPropertyChangedCallback(global::Microsoft.UI.Xaml.Controls.ToggleMenuFlyoutItem.IsCheckedProperty, (sender, prop) =>
                    {
                        var bindingObj = this.TryGetBindingObject();
                        if (bindingObj != null)
                        {
                            bindingObj.UpdateTwoWay_14_IsChecked();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainWindow.xaml line 32
                {
                    this.InactivePanel = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 3: // MainWindow.xaml line 41
                {
                    this.ActivePanel = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 4: // MainWindow.xaml line 48
                {
                    this.ProgressBar1 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Slider>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Slider)this.ProgressBar1).ValueChanged += this.Slider_ValueChanged;
                    ((global::Microsoft.UI.Xaml.Controls.Slider)this.ProgressBar1).ManipulationStarted += this.Slider_ManipulationStarted;
                    ((global::Microsoft.UI.Xaml.Controls.Slider)this.ProgressBar1).ManipulationCompleted += this.Slider_ManipulationCompleted;
                }
                break;
            case 5: // MainWindow.xaml line 51
                {
                    this.PlayButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.PlayButton).Click += this.PlayButton_Click;
                }
                break;
            case 6: // MainWindow.xaml line 52
                {
                    this.SwitchButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.SwitchButton).Click += this.SwitchButton_Click;
                }
                break;
            case 7: // MainWindow.xaml line 53
                {
                    this.PauseButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.PauseButton).Click += this.PauseButton_Click;
                }
                break;
            case 8: // MainWindow.xaml line 43
                {
                    this.StandingTime = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 9: // MainWindow.xaml line 45
                {
                    this.SittingTime = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 10: // MainWindow.xaml line 34
                {
                    global::Microsoft.UI.Xaml.Controls.NumberBox element10 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NumberBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.NumberBox)element10).ValueChanged += this.NumberBox_ValueChanged;
                }
                break;
            case 11: // MainWindow.xaml line 37
                {
                    global::Microsoft.UI.Xaml.Controls.NumberBox element11 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NumberBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.NumberBox)element11).ValueChanged += this.NumberBox_ValueChanged;
                }
                break;
            case 12: // MainWindow.xaml line 28
                {
                    this.StandingImage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                }
                break;
            case 13: // MainWindow.xaml line 29
                {
                    this.SittingImage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // MainWindow.xaml line 2
                {                    
                    global::Microsoft.UI.Xaml.Window element1 = (global::Microsoft.UI.Xaml.Window)target;
                    MainWindow_obj1_Bindings bindings = new MainWindow_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Activated += bindings.Activated;
                }
                break;
            }
            return returnValue;
        }
    }
}

