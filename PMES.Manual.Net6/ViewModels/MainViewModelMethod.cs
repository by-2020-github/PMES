using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreeSql.DataAnnotations;
using K4os.Hash.xxHash;
using Newtonsoft.Json;
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.Model;
using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES.Manual.Net6.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        #region 通用方法

        [ObservableProperty] private int _currentLogIndex;

        void ShowError(string msg)
        {
            Logger?.Error(msg);
            LogList.Add($"[{DateTime.Now:T}]: {msg}");
            CurrentLogIndex = LogList.Count - 1;
            //MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void ShowInfo(string msg)
        {
            Logger?.Information(msg);
            LogList.Add($"[{DateTime.Now:T}]: {msg}");
            //MessageBox.Show(msg, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private T_preheater_code GetTReelCode(ProductInfo product, string productCode, double netWeight,
            double grossWeight)
        {
            var tReelCode = new T_preheater_code
            {
                BatchNO = @$"{productCode}-{DateTime.Now: MMdd}A",
                CreateTime = DateTime.Now,
                CustomerCode = product.customer_number,
                CustomerId = product.customer_id,
                CustomerMaterialCode = product.customer_material_number,
                CustomerMaterialName = product.customer_material_name,
                CustomerMaterialSpec = product.customer_material_spec,
                CustomerName = product.customer_name,
                GrossWeight = grossWeight,
                ICMOBillNO = product.product_order_no, //生产工单 之前是null，todo:确认是否需要
                IsDel = 0,
                IsQualified = true, //是否合格 默认是合格的
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = netWeight,
                NoQualifiedReason = "",
                OperatorCode = product.operator_code,
                OperatorName = product.operator_name,
                PreheaterCode = product.xpzl_number,
                PreheaterId = product.xpzl_id,
                PreheaterName = product.xpzl_name,
                PreheaterSpec = product.xpzl_spec,
                PreheaterWeight = double.Parse(product.xpzl_weight),
                ProductCode = product.material_number,
                ProductDate = DateTime.Parse(product.product_date), // product.product_date
                ProductGBName = product.material_ns_model,
                ProductId = product.material_id,
                ProductionBarcode = productCode,
                ProductionOrgNO = product.product_org_number,
                ProductMnemonicCode = product.material_mnemonic_code,
                ProductName = product.material_name,
                ProductSpec = product.customer_material_spec,
                ProductStandardName = product.material_execution_standard,
                PSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}{0001}",
                Status = 1, //装箱状态
                StockCode = product.stock_number,
                StockId = product.stock_id,
                StockName = product.stock_name,
                UpdateTime = DateTime.Now,
                UserStandardCode = product.jsbz_number,
                UserStandardId = product.jsbz_id,
                UserStandardName = product.jsbz_name,
                WeightUserId = GlobalVar.CurrentUserInfo.userId,
                Material_themal_grade = product.material_thermal_grade,
                Material_spec = product.material_spec,
                Jsbz_short_name = product.jsbz_short_name,
            };
            return tReelCode;
        }

        int GetTodayCount()
        {
            var path = "config\\count.json";
            var count = 0;
            var key = DateTime.Now.ToString("yyyy-MM-dd");
            var dic = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(path));
            if (dic == null)
            {
                dic = new Dictionary<string, int>()
                {
                    { key, 0 }
                };
            }
            else
            {
                if (dic.ContainsKey(key))
                {
                    count = dic[key];
                }
                else
                {
                    dic.Add(key, 0);
                }
            }

            dic[key] = count + 1;
            File.WriteAllText(path, JsonConvert.SerializeObject(dic));
            return dic[key];
        }

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }

        #endregion
    }
}