﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMES.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.9.0.0")]
    internal sealed partial class PMES_Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static PMES_Settings defaultInstance = ((PMES_Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new PMES_Settings())));
        
        public static PMES_Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM5")]
        public string COM {
            get {
                return ((string)(this["COM"]));
            }
            set {
                this["COM"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("19200")]
        public int BaudRate {
            get {
                return ((int)(this["BaudRate"]));
            }
            set {
                this["BaudRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ConnStr {
            get {
                return ((string)(this["ConnStr"]));
            }
            set {
                this["ConnStr"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\\\ProgramData\\\\PMES_Templates")]
        public string TemplatePath {
            get {
                return ((string)(this["TemplatePath"]));
            }
            set {
                this["TemplatePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastTemplate {
            get {
                return ((string)(this["LastTemplate"]));
            }
            set {
                this["LastTemplate"] = value;
            }
        }
    }
}
