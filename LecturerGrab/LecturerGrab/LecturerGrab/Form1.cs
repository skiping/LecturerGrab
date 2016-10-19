using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LecturerGrab
{
    public partial class Form1 : Form
    {
        int pageIndex = 1;
        int pageCount = 1;
        int totalCount = 1;
        public Form1()
        {
            InitializeComponent();

            lblMsg.Hide();

            lecturerList.View = View.Details;//设置视图  
            lecturerList.Columns.Add("序号", 50, HorizontalAlignment.Left);
            lecturerList.Columns.Add("姓名", 80, HorizontalAlignment.Left);
            lecturerList.Columns.Add("头衔", 150, HorizontalAlignment.Left);
            lecturerList.Columns.Add("费用", 120, HorizontalAlignment.Left);
            lecturerList.Columns.Add("电话", 120, HorizontalAlignment.Left);
            lecturerList.Columns.Add("助理电话", 120, HorizontalAlignment.Left);
            lecturerList.Columns.Add("擅长领域", 180, HorizontalAlignment.Left);
            lecturerList.Columns.Add("来源", 100, HorizontalAlignment.Left);
            lecturerList.Columns.Add("链接", 150, HorizontalAlignment.Left);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "请稍候，抓取中...";
            lblMsg.Show();
            try
            {
                pageCount = GetPageTotal();
                lblPage.Text = "共计" + pageCount + "页数据";
                GetLecturerFromZhongGuoJiangShi(1);
                //GetLecturerFromZhongHuaJiangShi(1);
                prevPage.Enabled = false;

                MessageBox.Show("抓取完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序异常！ 详细：" + ex.Message);
            }
            lblMsg.Hide();
        }

        private int GetPageTotal()
        {
            string url = "http://www.jiangshi99.com/Search/jiangshi/%E8%AE%B2%E5%B8%88/p/1.html";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream streamReceive = res.GetResponseStream();

            Encoding encoding = Encoding.GetEncoding("UTF-8");
            StreamReader streamReader = new StreamReader(streamReceive, encoding);

            string strResult = streamReader.ReadToEnd();

            Regex reg = new Regex(@"<li class=""jl_right"">(.|\s)*?</li>", RegexOptions.Multiline);
            MatchCollection ms = reg.Matches(strResult);

            string page = Regex.Match(strResult, @"<a href='/Search/jiangshi/讲师/p/(([0-9])*?).html' >最后一页</a>", RegexOptions.Multiline).Groups[1].Value;
            int.TryParse(page, out totalCount);

            return totalCount % 4 == 0 ? totalCount / 4 : totalCount / 4 + 1;
        }

        private void GetLecturerFromZhongGuoJiangShi(int pageIndex)
        {
            lecturerList.Items.Clear();
            lecturerList.BeginUpdate();

            lblPage1.Text = "第" + pageIndex + "页";

            int start = (pageIndex - 1) * 4 + 1;
            int end = start + 3;
            int no = (pageIndex - 1) * 56 + 1;

            List<Lecturer> list = new List<Lecturer>();
            while (start <= end)
            {
                string url = "http://www.jiangshi99.com/Search/jiangshi/%E8%AE%B2%E5%B8%88/p/" + start + ".html";
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string strResult = wc.DownloadString(url);


                Regex reg = new Regex(@"<li class=""jl_right"">(.|\s)*?</li>", RegexOptions.Multiline);
                MatchCollection ms = reg.Matches(strResult);

                foreach (Match NextMatch in ms)
                {
                    string matchStr = NextMatch.Groups[0].Value;

                    string nameMatch = Regex.Match(matchStr, @"<div class=""jl_jname"">(.|\s)*?</div>", RegexOptions.Multiline).Groups[0].Value;
                    string link = Regex.Match(nameMatch, "href=\"(.[^\"]*)\"", RegexOptions.Multiline).Groups[1].Value;
                    string title = Regex.Match(nameMatch, "title=\"(.[^\"]*)\"", RegexOptions.Multiline).Groups[1].Value;
                    string name = title.Split('-')[0];

                    string price = Regex.Match(matchStr, @"<span class=""kPrice"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    string area = Regex.Match(matchStr, @"<b>擅长领域：</b>((.)*?)</div>", RegexOptions.Multiline).Groups[1].Value;

                    if (string.IsNullOrEmpty(price))
                    {
                        string detail = wc.DownloadString("http://www.jiangshi99.com" + link);
                        price = Regex.Match(detail, @"<span class=""Red"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    }

                    string lectureName = link.Split('/')[2];

                    WebClient web = new WebClient();
                    web.Encoding = Encoding.UTF8;
                    web.Headers.Add(HttpRequestHeader.Host, "www.jiangshi99.com");
                    web.Headers.Add(HttpRequestHeader.Referer, "http://www.jiangshi99.com");
                    string phoneDetail = web.DownloadString("http://www.jiangshi99.com/jiangshis/contact_showv2/jshiname/" + lectureName);
                    string[] arr = phoneDetail.Split(',');

                    string status = arr[2].Split(':')[1];
                    string phone1 = "";
                    string phone2 = "";
                    if (status == "3}")
                    {
                        phone1 = "";
                        phone2 = arr[1].Split(':')[1].Replace("\"", "");
                    }
                    else if (status == "1}")
                    {
                        phone1 = arr[1].Split(':')[1].Replace("\"", "");
                        phone2 = arr[0].Split(':')[1].Replace("\"", "");
                    }
               
                    Lecturer lecturer = new Lecturer()
                    {
                        No = no,
                        Name = name,
                        DetialLink = "http://www.jiangshi99.com" + link,
                        Title = title,
                        From = "中国讲师网",
                        Price = price,
                        Phone1 = phone1 == "null" ? "" : phone1,
                        Phone2 = phone2 == "null" ? "" : phone2,
                        Area = area.Replace("&nbsp;", " ")
                    };
                    list.Add(lecturer);

                    ListViewItem item = new ListViewItem();
                    item.Text = lecturer.No.ToString();
                    item.SubItems.Add(lecturer.Name);
                    item.SubItems.Add(lecturer.Title);
                    item.SubItems.Add(lecturer.Price);
                    item.SubItems.Add(lecturer.Phone1);
                    item.SubItems.Add(lecturer.Phone2);
                    item.SubItems.Add(lecturer.Area);
                    item.SubItems.Add(lecturer.From);
                    item.SubItems.Add(lecturer.DetialLink);

                    lecturerList.Items.Add(item);
                    no++;
                }

                start++;
            }

            lecturerList.EndUpdate();
        }

        private void GetLecturerFromZhongHuaJiangShi(int pageIndex)
        {
            lecturerList.Items.Clear();
            lecturerList.BeginUpdate();

            lblPage1.Text = "第" + pageIndex + "页";

            int start = (pageIndex - 1) * 4 + 1;
            int end = start + 3;
            int no = (pageIndex - 1) * 56 + 1;

            List<Lecturer> list = new List<Lecturer>();
            while (start <= end)
            {
                string url = "http://www.jiangshi.org/search/kw_NULL_order_1_costmin_0_costmax_0_area_0_page_" + start + ".html";
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                //Stream streamReceive = res.GetResponseStream();
                //Encoding encoding = Encoding.GetEncoding("UTF-8");
                //StreamReader streamReader = new StreamReader(streamReceive, encoding);

                //string strResult = streamReader.ReadToEnd();

                WebClient wc = new WebClient();
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                wc.Encoding = Encoding.UTF8;
                string strResult = wc.DownloadString(url);

                Regex reg = new Regex(@"<div class=""fl_l"" style=""width: 566px;"">(.|\s)*?</div>", RegexOptions.Multiline);
                MatchCollection ms = reg.Matches(strResult);

                foreach (Match NextMatch in ms)
                {
                    string matchStr = NextMatch.Groups[0].Value;

                    string nameMatch = Regex.Match(matchStr, @"<a(.|\s)*?</a>", RegexOptions.Singleline).Groups[0].Value;
                    string link = Regex.Match(nameMatch, "href=\"(.[^\"]*)\"", RegexOptions.Multiline).Groups[1].Value;
                    string name = Regex.Match(nameMatch, @">((.)*?)</a>", RegexOptions.Multiline).Groups[1].Value;

                    string price = Regex.Match(matchStr, @"<span class=""kPrice"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    string phone = Regex.Match(matchStr, @"<span class=""Cl"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    string phone1 = Regex.Match(matchStr, @"<span class=""Cl Clz"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    phone = string.IsNullOrEmpty(phone) ? phone1 : phone;

                    string area = Regex.Match(matchStr, @"<b>擅长领域：</b>((.)*?)</div>", RegexOptions.Multiline).Groups[1].Value;

                    Lecturer lecturer = new Lecturer()
                    {
                        No = no,
                        Name = name,
                        DetialLink = "http://www.jiangshi99.com" + link,
                        Title = "",
                        From = "中国讲师网",
                        Price = price,
                        Phone1 = phone,
                        Area = area.Replace("&nbsp;", " ")
                    };
                    list.Add(lecturer);

                    ListViewItem item = new ListViewItem();
                    item.Text = lecturer.No.ToString();
                    item.SubItems.Add(lecturer.Name);
                    item.SubItems.Add(lecturer.Title);
                    item.SubItems.Add(lecturer.Price);
                    item.SubItems.Add(lecturer.Phone1);
                    item.SubItems.Add(lecturer.Area);
                    item.SubItems.Add(lecturer.From);
                    item.SubItems.Add(lecturer.DetialLink);

                    lecturerList.Items.Add(item);
                    no++;
                }

                start++;
            }

            lecturerList.EndUpdate();
        }


        /// <summary>
        /// 执行导出数据
        /// </summary>
        public void ExportToExecl()
        {
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "xls";
            sfd.Filter = "Excel文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DoExport(sfd.FileName);
            }
        }
        /// <summary>
        /// 具体导出的方法
        /// </summary>
        /// <param name="listView">ListView</param>
        /// <param name="strFileName">导出到的文件名</param>
        private void DoExport(string strFileName)
        {
            List<Lecturer> list = GetAllLecturer();
            int rowNum = list.Count;
            int rowIndex = 1;
            int columnIndex = 0;
            if (rowNum == 0 || string.IsNullOrEmpty(strFileName))
            {
                return;
            }
            if (rowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
                    return;
                }
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                //将ListView的列名导入Excel表第一行
                foreach (ColumnHeader dc in lecturerList.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.Text;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    rowIndex++;
                    Lecturer model = list[i];

                    xlApp.Cells[rowIndex, 1] = Convert.ToString(model.No);
                    xlApp.Cells[rowIndex, 2] = Convert.ToString(model.Name);
                    xlApp.Cells[rowIndex, 3] = Convert.ToString(model.Title);
                    xlApp.Cells[rowIndex, 4] = Convert.ToString(model.Price);
                    xlApp.Cells[rowIndex, 5] = Convert.ToString(model.Phone1);
                    xlApp.Cells[rowIndex, 6] = Convert.ToString(model.Phone2);
                    xlApp.Cells[rowIndex, 7] = Convert.ToString(model.Area);
                    xlApp.Cells[rowIndex, 8] = Convert.ToString(model.From);
                    xlApp.Cells[rowIndex, 9] = Convert.ToString(model.DetialLink);
                }

                xlBook.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlBook.Close();
                xlApp.Quit();
                xlApp = null;
                xlBook = null;
                GC.Collect();
                MessageBox.Show("导出完成");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (lecturerList.Items.Count == 0)
                {
                    MessageBox.Show("导师列表为空，请先抓取！");
                }
                else
                {
                    lblMsg.Text = "请稍候，导出中...";
                    lblMsg.Show();
                    ExportToExecl();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("程序异常！ 详细：" + ex.Message);
                GC.Collect();
            }
            lblMsg.Hide();
        }

        private void prevPage_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "请稍候，抓取中...";
            lblMsg.Show();
            try
            {
                pageIndex--;

                if (pageIndex == 1)
                    prevPage.Enabled = false;


                if (pageIndex != pageCount)
                    nextPage.Enabled = true;

                GetLecturerFromZhongGuoJiangShi(pageIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序异常！ 详细：" + ex.Message);
            }
            lblMsg.Hide();
        }

        private void nextPage_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "请稍候，抓取中...";
            lblMsg.Show();
            try
            {
                pageIndex++;

                if (pageIndex == pageCount)
                    nextPage.Enabled = false;

                if (pageIndex != 1)
                    prevPage.Enabled = true;

                GetLecturerFromZhongGuoJiangShi(pageIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序异常！ 详细：" + ex.Message);
            }
            lblMsg.Hide();
        }

        private List<Lecturer> GetAllLecturer()
        {
            int startPage = 1;
            int no = 1;
            List<Lecturer> list = new List<Lecturer>();
            while (startPage <= totalCount)
            {
                string url = "http://www.jiangshi99.com/Search/jiangshi/%E8%AE%B2%E5%B8%88/p/" + startPage + ".html";
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string strResult = wc.DownloadString(url);

                Regex reg = new Regex(@"<li class=""jl_right"">(.|\s)*?</li>", RegexOptions.Multiline);
                MatchCollection ms = reg.Matches(strResult);

                foreach (Match NextMatch in ms)
                {
                    string matchStr = NextMatch.Groups[0].Value;

                    string nameMatch = Regex.Match(matchStr, @"<div class=""jl_jname"">(.|\s)*?</div>", RegexOptions.Multiline).Groups[0].Value;
                    string link = Regex.Match(nameMatch, "href=\"(.[^\"]*)\"", RegexOptions.Multiline).Groups[1].Value;
                    string title = Regex.Match(nameMatch, "title=\"(.[^\"]*)\"", RegexOptions.Multiline).Groups[1].Value;
                    string name = title.Split('-')[0];

                    string price = Regex.Match(matchStr, @"<span class=""kPrice"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    string area = Regex.Match(matchStr, @"<b>擅长领域：</b>((.)*?)</div>", RegexOptions.Multiline).Groups[1].Value;

                    if (string.IsNullOrEmpty(price))
                    {
                        string detail = wc.DownloadString("http://www.jiangshi99.com" + link);
                        price = Regex.Match(detail, @"<span class=""Red"">((.)*?)</span>", RegexOptions.Multiline).Groups[1].Value;
                    }

                    string lectureName = link.Split('/')[2];

                    WebClient web = new WebClient();
                    web.Encoding = Encoding.UTF8;
                    web.Headers.Add(HttpRequestHeader.Host, "www.jiangshi99.com");
                    web.Headers.Add(HttpRequestHeader.Referer, "http://www.jiangshi99.com");
                    string phoneDetail = web.DownloadString("http://www.jiangshi99.com/jiangshis/contact_showv2/jshiname/" + lectureName);
                    string[] arr = phoneDetail.Split(',');

                    string status = arr[2].Split(':')[1];
                    string phone1 = "";
                    string phone2 = "";
                    if (status == "3}")
                    {
                        phone1 = "";
                        phone2 = arr[1].Split(':')[1].Replace("\"", "");
                    }
                    else if (status == "1}")
                    {
                        phone1 = arr[1].Split(':')[1].Replace("\"", "");
                        phone2 = arr[0].Split(':')[1].Replace("\"", "");
                    }

                    Lecturer lecturer = new Lecturer()
                    {
                        No = no,
                        Name = name,
                        DetialLink = "http://www.jiangshi99.com" + link,
                        Title = title,
                        From = "中国讲师网",
                        Price = price,
                        Phone1 = phone1,
                        Phone2 = phone2,
                        Area = area.Replace("&nbsp;", " ")
                    };
                    list.Add(lecturer);
                    no++;
                }

                startPage++;
            }

            return list;
        }
    }

    public class Lecturer
    {
        public int No { get; set; }
        public string Name { get; set; }

        public string DetialLink { get; set; }

        public string Title { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Price { get; set; }

        public string Area { get; set; }

        public string From { get; set; }
    }
}
