using DevExpress.XtraReports.UI;
using PMES_Automatic_Net6.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMES_Automatic_Net6.Core;
using PMES.Model;
using PMES_Respository.tbs_sqlserver;

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
        public PMES_Respository.tbs_sqlserver.T_preheater_code TbReelInfo { get; set; } = new T_preheater_code();

        /// <summary>
        ///     一个托盘 放了好多箱线盘
        /// </summary>
        public PMES_Respository.tbs_sqlserver.T_box TbBoxInfo { get; set; } = new T_box();

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

        private sealed class MyProductTaskInfoEqualityComparer : IEqualityComparer<MyProductTaskInfo>
        {
            public bool Equals(MyProductTaskInfo x, MyProductTaskInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.ProductOrderBarCode == y.ProductOrderBarCode && x.WorkShop == y.WorkShop &&
                       x.ProductOrderInfo.Equals(y.ProductOrderInfo) && x.Weight1.Equals(y.Weight1) &&
                       x.Weight2.Equals(y.Weight2) && x.ReelCode1 == y.ReelCode1 && x.ReelCode2 == y.ReelCode2 &&
                       x.BoxCode1 == y.BoxCode1 && x.BoxCode2 == y.BoxCode2 &&
                       x.GenerateReelCode == y.GenerateReelCode && x.GenerateBoxCode == y.GenerateBoxCode &&
                       x.TbReelInfo.Equals(y.TbReelInfo) && x.TbBoxInfo.Equals(y.TbBoxInfo) &&
                       Equals(x.ReportReel, y.ReportReel) && Equals(x.ReportTop, y.ReportTop) &&
                       Equals(x.ReportEdge, y.ReportEdge) && x.NeedPack == y.NeedPack;
            }

            public int GetHashCode(MyProductTaskInfo obj)
            {
                var hashCode = new HashCode();
                hashCode.Add(obj.ProductOrderBarCode);
                hashCode.Add((int)obj.WorkShop);
                hashCode.Add(obj.ProductOrderInfo);
                hashCode.Add(obj.Weight1);
                hashCode.Add(obj.Weight2);
                hashCode.Add(obj.ReelCode1);
                hashCode.Add(obj.ReelCode2);
                hashCode.Add(obj.BoxCode1);
                hashCode.Add(obj.BoxCode2);
                hashCode.Add(obj.GenerateReelCode);
                hashCode.Add(obj.GenerateBoxCode);
                hashCode.Add(obj.TbReelInfo);
                hashCode.Add(obj.TbBoxInfo);
                hashCode.Add(obj.ReportReel);
                hashCode.Add(obj.ReportTop);
                hashCode.Add(obj.ReportEdge);
                hashCode.Add(obj.NeedPack);
                return hashCode.ToHashCode();
            }
        }

        public static IEqualityComparer<MyProductTaskInfo> MyProductTaskInfoComparer { get; } =
            new MyProductTaskInfoEqualityComparer();

        protected bool Equals(MyProductTaskInfo other)
        {
            return ProductOrderBarCode == other.ProductOrderBarCode && WorkShop == other.WorkShop &&
                   ProductOrderInfo.Equals(other.ProductOrderInfo) && Weight1.Equals(other.Weight1) &&
                   Weight2.Equals(other.Weight2) && ReelCode1 == other.ReelCode1 && ReelCode2 == other.ReelCode2 &&
                   BoxCode1 == other.BoxCode1 && BoxCode2 == other.BoxCode2 &&
                   GenerateReelCode == other.GenerateReelCode && GenerateBoxCode == other.GenerateBoxCode &&
                   TbReelInfo.Equals(other.TbReelInfo) && TbBoxInfo.Equals(other.TbBoxInfo) &&
                   Equals(ReportReel, other.ReportReel) && Equals(ReportTop, other.ReportTop) &&
                   Equals(ReportEdge, other.ReportEdge) && NeedPack == other.NeedPack;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyProductTaskInfo)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(ProductOrderBarCode);
            hashCode.Add((int)WorkShop);
            hashCode.Add(ProductOrderInfo);
            hashCode.Add(Weight1);
            hashCode.Add(Weight2);
            hashCode.Add(ReelCode1);
            hashCode.Add(ReelCode2);
            hashCode.Add(BoxCode1);
            hashCode.Add(BoxCode2);
            hashCode.Add(GenerateReelCode);
            hashCode.Add(GenerateBoxCode);
            hashCode.Add(TbReelInfo);
            hashCode.Add(TbBoxInfo);
            hashCode.Add(ReportReel);
            hashCode.Add(ReportTop);
            hashCode.Add(ReportEdge);
            hashCode.Add(NeedPack);
            return hashCode.ToHashCode();
        }
    }
}