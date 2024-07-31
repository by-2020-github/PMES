using System.Diagnostics;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using PMES.UC.reports;
using System.Linq;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var pa = new Person1
            {
                Id = 1,
                Name = "zs"
            };
            var p2 = JsonConvert.DeserializeObject<Person2>(JsonConvert.SerializeObject(pa));
            Trace.WriteLine(JsonConvert.SerializeObject(p2));

            object p = pa;
            Debug.WriteLine(p is Person1);
            Debug.WriteLine(p is Person2);
        }       
        [TestMethod]
        public void TestMethod2()
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            var printDocument = new PrintDocument();
            //指定打印机
            printDocument.PrinterSettings.PrinterName = "导出为WPS PDF";// 打印驱动的名称             
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            try
            {
                printDocument.Print();
            }
            catch (InvalidPrinterException)
            {
                // 打印出错
            }
            finally
            {
                printDocument.Dispose();
            }
           
        }

        [TestMethod]
        public void TestMethod3()
        {
            var templateBox = new TemplateBox();
            templateBox.Print("导出为WPS PDF");
        }

        // 文件格式设置
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            e.PageSettings.PaperSize = new PaperSize
            {
                Height = 400,
                PaperName = "MyOwn",
                RawKind = 0,
                Width = 300
            };
            Font titleFont = new Font("宋体", 28, FontStyle.Bold);// 标题字体
            Font fntTxt = new Font("宋体", 20, FontStyle.Regular);// 正文文字
            Brush brush = new SolidBrush(Color.Black);// 画刷
            Pen pen = new Pen(Color.Black); // 线条颜色
            Point po = new Point(10, 10);  // (左右边距, 上下行间距)
            try
            {
                e.Graphics.DrawString("   收费票据", titleFont, brush, new Point(10, 30));  //打印标题内容
                e.Graphics.DrawString(GetPrintSW().ToString(), fntTxt, brush, po);  //打印内容
            }

            catch (Exception ex)
            {
               
            }

        }

        // 打印内容，使用 StringBuilder， AppendLine为独占一行
        public StringBuilder GetPrintSW()
        {
            StringBuilder printTxt = new StringBuilder();

            printTxt.AppendLine(" \n");
            printTxt.AppendLine(" \n");

            printTxt.AppendLine("卡号: " + "00008476\n");

            printTxt.AppendLine("姓名: " + "尼古拉斯张三 \n");

            printTxt.AppendLine("身份证号: \n");
            printTxt.AppendLine("350781196663078325 \n");

            printTxt.AppendLine("卡成本: " + "10 \n");

            printTxt.AppendLine("支付时间： \n");
            printTxt.AppendLine(DateTime.Now.ToString() + " \n");

            printTxt.AppendLine("支付方式：" + "微信支付 \n");

            printTxt.AppendLine("票据打印时间： \n");
            printTxt.AppendLine(DateTime.Now.ToString() + " \n");

            return printTxt;
        }

    }


    public class Person1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Person2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Des { get; set; } = "des";
    }
}