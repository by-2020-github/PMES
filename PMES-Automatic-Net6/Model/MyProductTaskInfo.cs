using DevExpress.XtraReports.UI;
using PMES_Automatic_Net6.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMES_Automatic_Net6.Core;
using PMES_Respository.tbs;
using PMES.Model;

namespace PMES_Automatic_Net6.Model
{
    public class MyProductTaskInfo
    {
        /// <summary>
        ///     产品订单编号
        /// </summary>
        public string ProductOrderBarCode { get; set; } = "";

        /// <summary>
        ///     产品所处位置
        /// </summary>
        public EnumWorkShop WorkShop { get; set; }

        /// <summary>
        ///     根据ProductBarCode获取到的产品信息
        /// </summary>
        public ProductInfo ProductOrderInfo { get; set; } = new ProductInfo();

        #region 称重条码数据

        /// <summary>
        ///     称1的原始重量
        /// </summary>
        public double Weight1 { get; set; }

        /// <summary>
        ///     称2的原始重量
        /// </summary>
        public double Weight2 { get; set; }

        #endregion

        #region code 复核

        /// <summary>
        ///     校验时顶部会扫到盘码（合格证）和产品生产码，1/2只要有一个合格就Ok
        /// </summary>
        public string? ReelCode1 { get; set; }

        public string? ReelCode2 { get; set; }


        public string? BoxCode1 { get; set; }
        public string? BoxCode2 { get; set; }

        #endregion

        #region 生成的条码

        public string? GenerateReelCode { get; set; }
        public string? GenerateBoxCode { get; set; }

        #endregion

        #region 盘码和箱码的信息也保存下

        /// <summary>
        ///     线盘顶部盘码信息（合格证）---一箱一个线盘
        ///     包括毛重，净重，是否合格，不合格原因
        /// </summary>
        public PMES_Respository.tbs.T_preheater_code TbReelInfo { get; set; } = new T_preheater_code();

        /// <summary>
        ///     一个托盘 放了好多箱线盘
        /// </summary>
        public PMES_Respository.tbs.T_box TbBoxInfo { get; set; } = new T_box();

        #endregion

        #region 标签信息

        public XtraReport? ReportReel { get; set; }
        public XtraReport? ReportTop { get; set; }
        public XtraReport? ReportEdge { get; set; }

        #endregion

        public bool NeedPack { get; set; }

        /// <summary>
        ///     盘码校验
        /// </summary>
        public bool ReelCodeValidate => (ReelCode1 == GenerateReelCode || ReelCode2 == GenerateReelCode) &&
                                        (!string.IsNullOrEmpty(GenerateReelCode));


        /// <summary>
        ///     箱码校验
        /// </summary>
        public bool BoxCodeValidate => (BoxCode1 == GenerateBoxCode || BoxCode1 == GenerateBoxCode) &&
                                       (!string.IsNullOrEmpty(GenerateBoxCode));


        public async Task<bool> WeightValidate()
        {
            Debug.Assert(TbReelInfo != null, nameof(TbReelInfo) + " != null");

            switch (ProductOrderInfo.product_order_no.Substring(0,3))
            {
                case "777":
                    TbReelInfo.NoQualifiedReason = "测试数据";
                    TbReelInfo.IsQualified = false;
                    break;
                case "888":
                    TbReelInfo.NoQualifiedReason = "扫码失败";
                    TbReelInfo.IsQualified = false;
                    break;
                case "999":
                    TbReelInfo.NoQualifiedReason = "ERP访问失败";
                    TbReelInfo.IsQualified = false;
                    break;
                default:
                {
                    try
                    {
                        var validateOrder =
                            await ValidateOrder(TbReelInfo.NetWeight, ProductOrderBarCode!);
                        if (validateOrder.Item1) return true;
                        TbReelInfo.NoQualifiedReason = validateOrder.Item2;
                        TbReelInfo.IsQualified = false;
                    }
                    catch (Exception e)
                    {
                        TbReelInfo.NoQualifiedReason = "ERP访问失败";
                        TbReelInfo.IsQualified = false;
                    }

                    break;
                }
            }

            return false;
        }


        private async Task<Tuple<bool, string>> ValidateOrder(double weight, string order)
        {
            var validate =
                await WebService.Instance.GetJObjectValidate(
                    $"{ApiUrls.ValidateOrder}net_weight={weight:F2}&semi_finished={order}");

            return validate;
        }
    }
}