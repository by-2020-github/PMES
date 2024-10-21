using System;
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
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.XtraReports.UI;
using FreeSql.DataAnnotations;
using K4os.Hash.xxHash;
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.Model;
using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES.Manual.Net6.ViewModels
{
    public class ViewProductModel : ObservableObject
    {
        public ViewProductModel(ProductInfo productOrderInfo, string productQrCode)
        {
            ProductOrderInfo = productOrderInfo;
            ProductQrCode = productQrCode;
        }

        public ProductInfo ProductOrderInfo { get; set; }

        #region 额外包装信息

        public string? MotherTrayCode { get; set; }
        public string? ChildTrayName { get; set; }
        public string? PackagePaperBox { get; set; }
        public int? ReelNumPerBox { get; set; }

        #endregion

        //标签模板的名字 【packageCode + packageName】
        public string LabelTemplateName { get; set; } = "";

        /// <summary>
        ///     生产工单
        /// </summary>
        public string ProductOrder { get; set; } = "";

        /// <summary>
        ///    产品代码
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        ///    产品型号
        /// </summary>
        public string ProductModel { get; set; } = "";

        /// <summary>
        ///    国际型号
        /// </summary>
        public string InternationalModel { get; set; } = "";

        /// <summary>
        ///    产品规格
        /// </summary>
        public string ProductSpecification { get; set; } = "";

        /// <summary>
        ///    用户标准代码
        /// </summary>
        public string UserStandardCode { get; set; } = "";

        /// <summary>
        ///    用户标准名称
        /// </summary>
        public string UserStandardName { get; set; } = "";

        public string ReelCode { get; set; } = "";
        public string ReelName { get; set; } = "";

        /// <summary>
        ///     包装代码
        /// </summary>
        public string PackingCode { get; set; } = "";

        /// <summary>
        ///     包装名称
        /// </summary>
        public string PackingName { get; set; } = "";

        public string ExecutionStandard { get; set; } = "";
        public string ProductionDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// 生产机台
        /// </summary>
        public string ProductionMachine { get; set; } = "";

        /// <summary>
        /// 生产批号
        /// </summary>
        public string ProductionBatchNumber { get; set; } = "";

        /// <summary>
        /// 生产工号
        /// </summary>
        public string ProductionNumber { get; set; } = "";

        /// <summary>
        /// 包装组编号
        /// </summary>
        public string PackingGroupCode { get; set; } = "";

        /// <summary>
        /// 包装组名称
        /// </summary>
        public string PackingGroupName { get; set; } = "";

        /// <summary>
        /// 产品码（生产条码 扫描到的码）
        /// </summary>
        public string ProductQrCode { get; set; }


        /// <summary>
        ///     箱码
        /// </summary>
        public BitmapImage? ImageBox { get; set; }

        #region 称重信息

        public double ReelWeight { get; set; } = new Random().NextDouble();
        public double GrossWeight { get; set; } 
        public double NetWeight => GrossWeight - PackagePaperWeight - ReelWeight;
        public double PackagePaperWeight { get; set; } 

        #endregion

        #region 条码信息

        public string BoxQrCode { get; set; } = "";

        #endregion

        public static ViewProductModel GetViewProductModel(ProductInfo product, string productQrCode)
        {
            var viewProductModel = new ViewProductModel(product, productQrCode)
            {
                MotherTrayCode = null,
                ProductOrder = product.product_order_no,
                ProductCode = product.material_number,
                ProductModel = product.material_name,
                InternationalModel = product.material_ns_model,
                ProductSpecification = product.material_spec,
                UserStandardCode = product.jsbz_number,
                UserStandardName = product.jsbz_name,
                ReelCode = product.xpzl_number,
                ReelName = product.xpzl_name,
                PackingCode = product.package_info.code ?? "",
                PackingName = product.package_info.name ?? "",
                ExecutionStandard = product.material_execution_standard,
                ProductionDate = product.product_date,
                ProductionMachine = product.machine_number,
                ProductionBatchNumber = null,
                ProductionNumber = product.operator_code,
                PackingGroupCode = null,
                PackingGroupName = null,
                ImageBox = null,
                ReelWeight = 0,
                GrossWeight = 0,
                PackagePaperWeight = 0,
                BoxQrCode = null,
                PackageWeightWeight = 0,
                ChildTrayName = product.package_info.delivery_sub_tray_name,
                //TODO: 纸箱不知道是哪个字段
                PackagePaperBox = product.package_info.wire_reel_external_package_name,
                ReelNumPerBox = (int)product.package_info.packing_quantity!,
                LabelTemplateName = null
            };
            if (double.TryParse(product.xpzl_weight, out var w))
            {
                viewProductModel.ReelWeight = w;
            }
            else
            {
                viewProductModel.ReelWeight = -1;
            }

            if (double.TryParse(product.package_info.tare_weight, out double reelWeight))
            {
                viewProductModel.ReelWeight = reelWeight;
            }
            else
            {
                viewProductModel.ReelWeight = -1;
            }

            return viewProductModel;
        }

        #region 辅助信息

        public double PackageWeightWeight { get; set; }

        #endregion
    }

    /// <summary>
    ///     箱码信息
    /// </summary>
    public class MyBoxInfo
    {
        private static int _number = 0;

        public int Number { get; set; }
        public string BoxQrCoed { get; set; }
        public List<MyReelInfo> ReelList { get; set; } = new List<MyReelInfo>();
        public double GrossWeight => ReelList.Sum(s => s.GrossWeight);
        public double NetWeight => ReelList.Sum(s => s.NetWeight);
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool HasPrint { get; set; } = false;
        public XtraReport? Report { get; set; }
        public ViewProductModel ViewProductModel { get; set; }
        public T_label_template LabelTemplate { get; set; }

        public MyBoxInfo()
        {
            Number = ++_number;
        }

        public static void Reset()
        {
            _number = 0;
        }

        public bool IsFull()
        {
            if (ReelList.Count == 0)
                return false;
            return ReelList.First().CountOneBox == ReelList.Count;
        }
    }

    /// <summary>
    ///     盘码信息
    /// </summary>
    [Table(Name = "tb_temp_reels")]
    public class MyReelInfo
    {
        private static int _number = 0;

        public int Number { get; set; }

        /// <summary>
        ///     每箱装多少个
        /// </summary>
        public int CountOneBox { get; set; }

        public string ReelQrCode { get; set; } = "B10001-1";
        public string ReelProductCode { get; set; } = "G23100061G010001";
        public double GrossWeight { get; set; } = new Random().NextDouble();
        public double NetWeight { get; set; } = new Random().NextDouble();
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public T_preheater_code PreheaterCode { get; set; }

        public ProductInfo ProductInfo { get; set; }

        public MyReelInfo()
        {
            Number = ++_number;
        }


        public static void Reset()
        {
            _number = 0;
        }
    }
}