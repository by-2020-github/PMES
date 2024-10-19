using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.XtraReports.UI;
using PMES.Manual.Net6.Core;
using PMES_Respository.tbs_sqlserver;

namespace PMES.Manual.Net6.Model
{
    public class MyProductTaskInfo
    {
        /// <summary>
        ///     产品订单编号
        /// </summary>
        public string ProductOrderBarCode { get; set; } = "";

        /// <summary>
        ///     根据ProductBarCode获取到的产品信息
        /// </summary>
        public ProductInfo ProductOrderInfo { get; set; } = new ProductInfo();

        /// <summary>
        ///     称1的原始重量
        /// </summary>
        public double Weight1 { get; set; }

        #region 生成的条码

        public string? GenerateReelCode { get; set; }
        public string? GenerateBoxCode { get; set; }

        #endregion

        #region 盘码和箱码的信息也保存下

        /// <summary>
        ///     线盘顶部盘码信息（合格证）---一箱一个线盘
        ///     包括毛重，净重，是否合格，不合格原因
        /// </summary>
        public PMES_Respository.tbs_sqlserver.T_preheater_code TbReelInfo { get; set; } = new T_preheater_code();

        /// <summary>
        ///     一个托盘 放了好多箱线盘
        /// </summary>
        public PMES_Respository.tbs_sqlserver.T_box TbBoxInfo { get; set; } = new T_box();

        #endregion


        public XtraReport? Report { get; set; }


        public async Task<bool> WeightValidate()
        {
            Debug.Assert(TbReelInfo != null, nameof(TbReelInfo) + " != null");

            switch (ProductOrderInfo.product_order_no.Substring(0, 3))
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