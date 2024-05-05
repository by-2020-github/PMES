using System;

namespace PMES_WPF_Automatic_Slide.Models.Ui
{
    public class MRecord
    {
        public string Code { get; set; }

        /// <summary>
        ///     顶贴码
        /// </summary>
        public string TopCode { get; set; }

        public DateTime DateTime { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }

        public string NetWeight { get; set; }
        public string Code2 { get; set; }
        public string BatchCode { get; set; }

        /// <summary>
        /// 侧贴码
        /// </summary>
        public string SlideCode { get; set; }
    }
}