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
            //ָ����ӡ��
            printDocument.PrinterSettings.PrinterName = "����ΪWPS PDF";// ��ӡ����������             
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            try
            {
                printDocument.Print();
            }
            catch (InvalidPrinterException)
            {
                // ��ӡ����
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
            templateBox.Print("����ΪWPS PDF");*/
        }

        // �ļ���ʽ����
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            e.PageSettings.PaperSize = new PaperSize
            {
                Height = 400,
                PaperName = "MyOwn",
                RawKind = 0,
                Width = 300
            };
            Font titleFont = new Font("����", 28, FontStyle.Bold);// ��������
            Font fntTxt = new Font("����", 20, FontStyle.Regular);// ��������
            Brush brush = new SolidBrush(Color.Black);// ��ˢ
            Pen pen = new Pen(Color.Black); // ������ɫ
            Point po = new Point(10, 10);  // (���ұ߾�, �����м��)
            try
            {
                e.Graphics.DrawString("   �շ�Ʊ��", titleFont, brush, new Point(10, 30));  //��ӡ��������
                e.Graphics.DrawString(GetPrintSW().ToString(), fntTxt, brush, po);  //��ӡ����
            }

            catch (Exception ex)
            {
               
            }

        }

        // ��ӡ���ݣ�ʹ�� StringBuilder�� AppendLineΪ��ռһ��
        public StringBuilder GetPrintSW()
        {
            StringBuilder printTxt = new StringBuilder();

            printTxt.AppendLine(" \n");
            printTxt.AppendLine(" \n");

            printTxt.AppendLine("����: " + "00008476\n");

            printTxt.AppendLine("����: " + "�����˹���� \n");

            printTxt.AppendLine("���֤��: \n");
            printTxt.AppendLine("350781196663078325 \n");

            printTxt.AppendLine("���ɱ�: " + "10 \n");

            printTxt.AppendLine("֧��ʱ�䣺 \n");
            printTxt.AppendLine(DateTime.Now.ToString() + " \n");

            printTxt.AppendLine("֧����ʽ��" + "΢��֧�� \n");

            printTxt.AppendLine("Ʊ�ݴ�ӡʱ�䣺 \n");
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