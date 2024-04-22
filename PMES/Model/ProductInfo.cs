using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace PMES.Model
{
    public class ProductInfo
    {
        public string product_date { get; set; } = "2024-04-22";
        public string product_org_number { get; set; } = "100";
        public string product_order_no { get; set; } = "G24040656";
        public int customer_id { get; set; } = 102385;
        public string customer_number { get; set; } = "KH001299";
        public string customer_name { get; set; } = "青岛云路新能源科技有限公司";
        public int material_id { get; set; } = 113889;
        public string material_number { get; set; } = "21.T.Y.61.2.1100";
        public string material_name { get; set; } = "1EI/AIW/200";
        public string material_mnemonic_code { get; set; } = "TY6121100";
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
        public PackageInfo package_info { get; set; } = new PackageInfo();
    }

    public class PackageInfo
    {
        public string code { get; set; } = "0002";
        public string name { get; set; } = "PT200金宇";
        public string tare_weight { get; set; } = "0.0000000000";
        public string cu_full_coil_weight { get; set; } = "180.0000000000";
        public string cu_min_weight { get; set; } = "100.0000000000";
        public string cu_max_weight { get; set; } = "205.0000000000";
        public string wire_reel_inside_package_name { get; set; } = null;
        public string wire_reel_external_package_name { get; set; } = null;
        public string delivery_sub_tray_name { get; set; } = null;
        public int packing_quantity { get; set; } = 0;
        public int stacking_per_layer { get; set; } = 0;
        public int stacking_layers { get; set; } = 0;
    }


}