using System.Diagnostics;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using PMES.UC.reports;
using System.Linq;
 
using Newtonsoft.Json.Linq;
using System.Net.Http;


namespace Test
{
    [TestClass]
    public class UnitTest1
    {
      

        [TestMethod]
        public void TestMethod1()
        {
           
            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.137.75:8089/api/stacking/excludePosition");
            //var collection = new List<KeyValuePair<string, string>>();
            //collection.Add(new("emptyTrayWorkshopId", "-1"));
            //var content = new FormUrlEncodedContent(collection);
            //request.Content = content;
            //var response =   client.Send(request);
            //response.EnsureSuccessStatusCode();
            //Debug.WriteLine(  response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void TestMethod22()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.101.4:8089/api/stacking/excludePosition");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("emptyTrayWorkshopId", "-1"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = client.Send(request);
            response.EnsureSuccessStatusCode();
            Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<bool> ClearErrorStack(int workId)
        {
            try
            {
                HttpClient _httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.137.75:8089/api/stacking/excludePosition");
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("emptyTrayWorkshopld", $"{workId}"));
                var content = new FormUrlEncodedContent(collection);
                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseStr = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseStr );
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
           /* var templateBox = new TemplateBox();
            templateBox.Print("导出为WPS PDF");*/
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