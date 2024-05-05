// ReSharper disable InconsistentNaming

namespace PMES.Model;

public class ProductInfo
{
    public string product_date { get; set; } = "2024-04-22";
    public string product_org_number { get; set; } = "100";

    /// <summary>
    ///     生产工单
    /// </summary>
    public string product_order_no { get; set; } = "G24040656";

    public int customer_id { get; set; } = 102385;

    public string customer_number { get; set; } = "KH001299";
    public string customer_name { get; set; } = "青岛云路新能源科技有限公司";
    public int material_id { get; set; } = 113889;

    /// <summary>
    ///     产品代码
    /// </summary>
    public string material_number { get; set; } = "21.T.Y.61.2.1100";

    /// <summary>
    ///     产品型号
    /// </summary>
    public string material_name { get; set; } = "1EI/AIW/200";

    public string material_mnemonic_code { get; set; } = "TY6121100";

    /// <summary>
    ///     国标型号
    /// </summary>
    public string material_execution_standard { get; set; } = "GB/T6109.20-2008";

    public string material_ns_model { get; set; } = "Q(ZY/XY)-2/200";
    public int jsbz_id { get; set; } = 2655086;
    public string jsbz_number { get; set; } = "BZ155";
    public string jsbz_name { get; set; } = "云路定制B";
    public int xpzl_id { get; set; } = 128056;
    public string xpzl_number { get; set; } = "15.001";
    public string xpzl_name { get; set; } = "PT90ABS新料";
    public string xpzl_spec { get; set; } = "PT90";
    public string xpzl_weight { get; set; } = "3.9000000000";
    public int machine_id { get; set; } = 3129677;
    public string machine_number { get; set; } = "W19";
    public string machine_name { get; set; } = "W19号漆包机";
    public int stock_id { get; set; } = 115304;
    public string stock_number { get; set; } = "11";
    public string stock_name { get; set; } = "成品库(吴兴)";
    public PackageInfo package_info { get; set; } = new();

    public string operator_code { get; set; } = "G69";
    public string operator_name { get; set; } = "周成忠";
    public string customer_material_number { get; set; } = null;
    public string customer_material_name { get; set; } = null;
    public string customer_material_spec { get; set; } = null;

    protected bool Equals(ProductInfo other)
    {
        return material_number == other.material_number && jsbz_number == other.jsbz_number &&
               customer_material_number == other.customer_material_number && package_info.code.Equals(other.package_info.code);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ProductInfo)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(material_number, jsbz_number, customer_material_number, package_info.code);
    }
}

/// <summary>
///     包装信息
/// </summary>
public class PackageInfo
{
    /// <summary>
    ///     包装代码
    /// </summary>
    public string code { get; set; } = "0002";

    /// <summary>
    ///     包装名称
    /// </summary>
    public string name { get; set; } = "PT200金宇";

    /// <summary>
    ///     包装皮重
    /// </summary>
    public string tare_weight { get; set; } = "0.0000000000";

    /// <summary>
    ///     铜线满盘重量
    /// </summary>
    public string cu_full_coil_weight { get; set; } = "180.0000000000";

    /// <summary>
    ///     铜线最小重量
    /// </summary>
    public string cu_min_weight { get; set; } = "100.0000000000";

    /// <summary>
    ///     铜线最大重量
    /// </summary>
    public string cu_max_weight { get; set; } = "205.0000000000";

    /// <summary>
    ///     线盘内包装名称
    /// </summary>
    public string wire_reel_inside_package_name { get; set; } = null;

    /// <summary>
    ///     线盘外包装名称
    /// </summary>
    public string wire_reel_external_package_name { get; set; } = null;

    /// <summary>
    ///     发货子托盘名称
    /// </summary>
    public string delivery_sub_tray_name { get; set; } = null;

    /// <summary>
    ///     装箱件数
    /// </summary>
    public int packing_quantity { get; set; } = 0;

    /// <summary>
    ///     每层堆垛数量
    /// </summary>
    public int stacking_per_layer { get; set; } = 0;

    /// <summary>
    ///     堆垛层数
    /// </summary>
    public int stacking_layers { get; set; } = 0;
}