﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMES_Automatic_Net6 {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.9.0.0")]
    internal sealed partial class PMESConfig : global::System.Configuration.ApplicationSettingsBase {
        
        private static PMESConfig defaultInstance = ((PMESConfig)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new PMESConfig())));
        
        public static PMESConfig Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=172.16.3.253;User Id=sa;Password=Xianden.1984;Initial Catalog=PMES;En" +
            "crypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1")]
        public string ConnXdSqlServer {
            get {
                return ((string)(this["ConnXdSqlServer"]));
            }
            set {
                this["ConnXdSqlServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.10")]
        public string PlcIp {
            get {
                return ((string)(this["PlcIp"]));
            }
            set {
                this["PlcIp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int PlcSlot {
            get {
                return ((int)(this["PlcSlot"]));
            }
            set {
                this["PlcSlot"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int PlcRack {
            get {
                return ((int)(this["PlcRack"]));
            }
            set {
                this["PlcRack"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.150")]
        public string PlcXinJieIp {
            get {
                return ((string)(this["PlcXinJieIp"]));
            }
            set {
                this["PlcXinJieIp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("502")]
        public int PlcXinJiePort {
            get {
                return ((int)(this["PlcXinJiePort"]));
            }
            set {
                this["PlcXinJiePort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;User Id=sa;Password=Aa123.321;Initial Catalog=PMES;Encrypt=True;Tru" +
            "stServerCertificate=True;Pooling=true;Min Pool Size=1")]
        public string ConnLocalSqlServer {
            get {
                return ((string)(this["ConnLocalSqlServer"]));
            }
            set {
                this["ConnLocalSqlServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=127.0.0.1;Port=3306;User ID=root;Password=123456; Initial Catalog=pme" +
            "s;Charset=utf8; SslMode=none;Min pool size=1;AllowPublicKeyRetrieval=true")]
        public string ConnLocalMysql {
            get {
                return ((string)(this["ConnLocalMysql"]));
            }
            set {
                this["ConnLocalMysql"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.151")]
        public string PrinterName1 {
            get {
                return ((string)(this["PrinterName1"]));
            }
            set {
                this["PrinterName1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.152")]
        public string PrinterName2 {
            get {
                return ((string)(this["PrinterName2"]));
            }
            set {
                this["PrinterName2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.153")]
        public string PrinterName3 {
            get {
                return ((string)(this["PrinterName3"]));
            }
            set {
                this["PrinterName3"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.154")]
        public string PrinterName4 {
            get {
                return ((string)(this["PrinterName4"]));
            }
            set {
                this["PrinterName4"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.155")]
        public string PrinterName5 {
            get {
                return ((string)(this["PrinterName5"]));
            }
            set {
                this["PrinterName5"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192.168.1.156")]
        public string PrinterName6 {
            get {
                return ((string)(this["PrinterName6"]));
            }
            set {
                this["PrinterName6"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EmptyUnStacking {
            get {
                return ((bool)(this["EmptyUnStacking"]));
            }
            set {
                this["EmptyUnStacking"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EmptyStacking {
            get {
                return ((bool)(this["EmptyStacking"]));
            }
            set {
                this["EmptyStacking"] = value;
            }
        }
    }
}
