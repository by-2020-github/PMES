﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreeSql.DataAnnotations;
using K4os.Hash.xxHash;
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

        void ShowError(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void ShowInfo(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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

        #endregion
    }
}