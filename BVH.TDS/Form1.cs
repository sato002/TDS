using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using RestSharp;
using BVH.TDS.Properties;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using BVHAU.TDS;
using System.IO;
using System.Globalization;
using System.Text.Encodings.Web;
using AnyCaptchaHelper;
using AnyCaptchaHelper.Api;

namespace BVH.TDS
{
    public partial class Form1 : Form
    {
        
        private static string ACCOUNT_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "accounts.json");

      
        private List<AccountInfor> lstAccountInfor;

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            LoadTotalCoin();
        }

        private void InitializeDataGridView()
        {
            lstAccountInfor = new List<AccountInfor>();
            LoadFile();
            grdAccount.AllowUserToAddRows = false;
            grdAccount.Columns[(int)EnumColumnOrder.Username].Width = 100;
            grdAccount.Columns[(int)EnumColumnOrder.Password].Width = 100;
            grdAccount.Columns[(int)EnumColumnOrder.AccessToken].Width = 160;
            grdAccount.Columns[(int)EnumColumnOrder.QuickLink].Width = 60;
            grdAccount.Columns[(int)EnumColumnOrder.TikUsername].Width = 100;
            grdAccount.Columns[(int)EnumColumnOrder.Coin].Width = 80;
            grdAccount.Columns[(int)EnumColumnOrder.State].Width = 197;
            
        }

        

        private void grdAccount_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Thêm từ clipboard (Tài khoản|Mật khẩu)", AddAccountFromClibboard));
                m.MenuItems.Add(new MenuItem("Đổi mật khẩu (sinh random)", ChangePasswordMenuItem_Click));
                m.MenuItems.Add(new MenuItem("Xóa", RemoveAccount));

                m.Show(grdAccount, new Point(e.X, e.Y));

            }
        }

        private void AddAccountFromClibboard(Object sender, System.EventArgs e)
        {
            try
            {
                string rawText;
                string[] rowText;


                if (Clipboard.ContainsText())
                {
                    rawText = Clipboard.GetText(TextDataFormat.Text);

                    if (rawText.Length > 1)
                    {
                        //  unify all line breaks to \r
                        rawText = rawText.Replace("\r\n", "\r").Replace("\n", "\r");

                        //  create an array of lines
                        rowText = rawText.Split('\r');

                        grdAccount.Rows.Clear();
                        for (int i = 0; i < rowText.Length; i++)
                        {
                            string row = rowText[i];
                            string[] rowSplit = row.Split('|');
                            //grdAccount.Rows.Add(i + 1, rowSplit[0], rowSplit[1], "", "", "");
                            lstAccountInfor.Add(new AccountInfor()
                            {
                                Username = rowSplit[0],
                                Password = rowSplit[1]
                            });
                        }
                    }
                }
                ReloadGrid();
                SaveFile();
            }
            catch (Exception)
            {
                MessageBox.Show("Clipboard không đúng định dạng!");
            }
        }

        private void RemoveAccount(Object sender, System.EventArgs e)
        {
            foreach (DataGridViewRow row in grdAccount.SelectedRows)
            {
                lstAccountInfor.Remove((AccountInfor)row.DataBoundItem);
                grdAccount.Rows.RemoveAt(row.Index);
            }
            LoadTotalCoin();
            SaveFile();
            LoadGridInfor();
        }

        #region User Event Handler
        private async void btnGetToken_Click(object sender, EventArgs e)
        {
            if (lstAccountInfor.Count > 0)
            {
                var lstTask = new List<Task>();
                foreach (var row in lstAccountInfor)
                {
                    row.State = "Đang đợi lấy token";
                    var task = GetToken(row);
                    lstTask.Add(task);
                }
                ReloadGrid();

                lstTask.ForEach(_ => _.Start());
                await Task.WhenAll(lstTask.ToArray());

                ReloadGrid();
                SaveFile();
                MessageBox.Show("Hoàn thành");
            }
        }

        private async void btnGetCoin_Click(object sender, EventArgs e)
        {
            if (lstAccountInfor.Count > 0)
            {
                var lstTask = new List<Task>();
                foreach (var row in lstAccountInfor)
                {
                    row.State = "Đang đợi lấy xu";
                    lstTask.Add(GetCoin(row));
                }

                ReloadGrid();

                lstTask.ForEach(_ => _.Start());
                await Task.WhenAll(lstTask.ToArray());

                ReloadGrid();
                LoadTotalCoin();
                SaveFile();
                MessageBox.Show("Hoàn thành");
            }
        }

        private async void ChangePasswordMenuItem_Click(object sender, EventArgs e)
        {
            if (grdAccount.SelectedRows.Count > 0)
            {
                var lstTask = new List<Task>();
                foreach (DataGridViewRow selectedRow in grdAccount.SelectedRows)
                {
                    var row = (AccountInfor)selectedRow.DataBoundItem;
                    row.State = "Đang đợi đổi mật khẩu";

                    var task = ChangePassword(row);
                    lstTask.Add(task);
                }
                ReloadGrid();

                lstTask.ForEach(_ => _.Start());
                await Task.WhenAll(lstTask.ToArray());

                ReloadGrid();
                SaveFile();
            }
            else
            {
                MessageBox.Show("Chưa chọn dòng!");
            }
        }

        private void grdAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveAccount(null, null);
            }
        }
        #endregion

        #region TDS Task
        private Task GetToken(AccountInfor row)
        {
            return new Task(() =>
            {
                try
                {
                    var tdsProxy = new TDSProxy();
                    var getTokenResponse = tdsProxy.GetToken(row).Result;
                    if (getTokenResponse == null)
                    {
                        throw new Exception("getTokenResponse null");
                    }

                    if (String.IsNullOrEmpty(getTokenResponse.access_token))
                    {
                        throw new Exception("access_token null");
                    }

                    row.AccessToken = getTokenResponse.access_token;
                    row.State = "Thành công!";
                }
                catch (Exception ex)
                {
                    row.State = $"Lỗi: {ex.Message}!";
                }
            });
        }

        private Task GetCoin(AccountInfor row)
        {
            return new Task(() => {
                try
                {
                    if (String.IsNullOrEmpty(row.AccessToken))
                    {
                        row.State = "Chưa lấy token!";
                    }
                    else
                    {
                        var tdsProxy = new TDSProxy();
                        var getCoinResponse = tdsProxy.GetCoin(row).Result;
                        if (getCoinResponse == null || getCoinResponse.success != 200 || getCoinResponse.data == null)
                        {
                            throw new Exception($"getCoinResponse error: {getCoinResponse}");
                        }

                        var data = getCoinResponse.data;
                        if (String.IsNullOrEmpty(data.xu))
                        {
                            throw new Exception($"getCoinResponse xu error: {data.error}");
                        }

                        row.Coin = String.IsNullOrEmpty(data.xu) ? 0 : int.Parse(data.xu);
                        row.State = "Thành công!";
                    }
                }
                catch (Exception ex)
                {
                    row.State = $"Có lỗi xảy ra: {ex.Message}!";
                }
            });
        }

        private Task ChangePassword(AccountInfor row)
        {
            return new Task(() =>
            {
                try
                {
                    var tdsProxy = new TDSProxy();
                    tdsProxy.Login(row).Wait();
                    var newPass = Utilities.RandomString(6);
                    var changePasswordResponse = tdsProxy.ChangePassword(row, newPass).Result;
                    if(changePasswordResponse != "0")
                    {
                        throw new Exception($"changePasswordResponse: {changePasswordResponse}");
                    }

                    row.Password = newPass;
                    row.State = "Đổi mật khẩu thành công";
                }
                catch (Exception ex)
                {
                    row.State = $"Lỗi khi đổi mk: {ex.Message}!";
                }
            });
        }
        #endregion


        #region Trigger Event Handler
        private void grdAccount_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        private void grdAccount_SelectionChanged(object sender, EventArgs e)
        {
            lbCountSelected.Text = grdAccount.SelectedRows.Count.ToString();
        }
        private void grdAccount_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == (int)EnumColumnOrder.State && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = Color.Black;
                var stateValue = grdAccount[e.ColumnIndex, e.RowIndex].Value;
                if (stateValue != null)
                {
                    var state = stateValue.ToString();
                    if (state.StartsWith("Thành công"))
                    {
                        e.CellStyle.ForeColor = Color.DarkGreen;
                    }
                    else if (state.StartsWith("Có lỗi"))
                    {
                        e.CellStyle.ForeColor = Color.DarkRed;
                    }
                }
            }
        }
        #endregion


        #region Private method
        private void ReloadGrid()
        {
            var bindingList = new SortableBindingList<AccountInfor>(lstAccountInfor);
            var source = new BindingSource(bindingList, null);
            grdAccount.DataSource = source;
            grdAccount.Update();
            grdAccount.Refresh();
            LoadGridInfor();
        }
        private void LoadGridInfor()
        {
            lstAccountInfor = lstAccountInfor ?? new List<AccountInfor>();
            lbCountAll.Text = lstAccountInfor.Count.ToString();
            lbCountSelected.Text = grdAccount.SelectedRows.Count.ToString();
        }
        private void LoadTotalCoin()
        {
            if (lstAccountInfor != null && lstAccountInfor.Count > 0)
            {
                var totalCoint = lstAccountInfor.Select(_ => _.Coin).Sum();
                lbTotalCoin.Text = PrettyNumber(totalCoint);
            }
            else
            {
                lbTotalCoin.Text = "0";
            }
        }
        private void LoadFile()
        {
            if (System.IO.File.Exists(ACCOUNT_FILE_PATH))
            {
                var json = System.IO.File.ReadAllText(ACCOUNT_FILE_PATH);
                if (!String.IsNullOrEmpty(json))
                {
                    lstAccountInfor = JsonConvert.DeserializeObject<List<AccountInfor>>(json);
                    ReloadGrid();
                }
            }
        }
        private void SaveFile()
        {
            lstAccountInfor = lstAccountInfor ?? new List<AccountInfor>();
            string json = JsonConvert.SerializeObject(lstAccountInfor);

            //write string to file
            System.IO.File.WriteAllText(ACCOUNT_FILE_PATH, json);
        }
        private string PrettyNumber(int value)
        {
            return String.Format(new CultureInfo("vi-VN"), "{0:N0}", value);
        }
        
        #endregion
    }
}
