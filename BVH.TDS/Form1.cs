using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using BVHAU.TDS;
using System.IO;
using System.Globalization;
using System.Threading;

namespace BVH.TDS
{
    public partial class Form1 : Form
    {

        private static string ACCOUNT_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "accounts.json");

        private SemaphoreSlim semaphore;

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
                bool hasRowSelected = grdAccount.SelectedRows.Count > 0;
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Thêm acc (Tài khoản|Mật khẩu|Token)", AddAccountFromClibboard));
                m.MenuItems.Add(new MenuItem("Copy acc (Tài khoản|Mật khẩu)", CopyAccount));
                m.MenuItems.Add(new MenuItem("Copy acc (Tài khoản|Mật khẩu|Token)", CopyAccount2));
                m.MenuItems.Add(new MenuItem("Thêm acc (Token) check xu", AddToken));
                m.MenuItems.Add(new MenuItem("Đổi mật khẩu (sinh random)", ChangePasswordMenuItem_Click));
                m.MenuItems.Add(new MenuItem("Tặng xu", TangXu));
                m.MenuItems.Add(new MenuItem("Xóa", RemoveAccount));

                m.MenuItems[1].Enabled = hasRowSelected;
                m.MenuItems[2].Enabled = hasRowSelected;
                m.MenuItems[4].Enabled = hasRowSelected;
                m.MenuItems[5].Enabled = hasRowSelected && txtUserNhanXu.Text.Length > 0 && numSoXuTang.Value >= 1000000;
                m.MenuItems[6].Enabled = hasRowSelected;

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
                            // duplicate check
                            List<AccountInfor> duplicateList = lstAccountInfor.FindAll(
                                delegate (AccountInfor acc)
                                {
                                    return acc.Username.Equals(rowSplit[0]);
                                });
                            if (duplicateList.Count > 0)
                            {
                                continue;
                            }
                            if (rowSplit.Length == 3)
                            {
                                lstAccountInfor.Add(new AccountInfor()
                                {
                                    Username = rowSplit[0],
                                    Password = rowSplit[1],
                                    AccessToken = rowSplit[2]
                                });
                            }
                            else if (rowSplit.Length == 2)
                            {
                                //grdAccount.Rows.Add(i + 1, rowSplit[0], rowSplit[1], "", "", "");
                                lstAccountInfor.Add(new AccountInfor()
                                {
                                    Username = rowSplit[0],
                                    Password = rowSplit[1],
                                    AccessToken = ""
                                });
                            }
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

        private void AddToken(Object sender, System.EventArgs e)
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
                            // duplicate check
                            List<AccountInfor> duplicateList = lstAccountInfor.FindAll(
                                delegate (AccountInfor acc)
                                {
                                    return acc.AccessToken.Equals(rowText[i]);
                                });
                            if (duplicateList.Count == 0)
                            {
                                lstAccountInfor.Add(new AccountInfor()
                                {
                                    Username = "",
                                    Password = "",
                                    AccessToken = rowText[i]
                                });
                            }
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

        // copy id | pass
        private void CopyAccount(Object sender, System.EventArgs e)
        {
            if (grdAccount.SelectedRows.Count > 0)
            {
                string rawText = "";
                int count = 0;
                List<KeyValuePair<int, string>> listAcc = new List<KeyValuePair<int, string>>();
                foreach (DataGridViewRow selectedRow in grdAccount.SelectedRows)
                {
                    count++;
                    var row = (AccountInfor)selectedRow.DataBoundItem;
                    listAcc.Add(new KeyValuePair<int, string>(selectedRow.Index, row.Username + "|" + row.Password));
                }
                // sort by index
                var listAcc2 = listAcc.OrderBy(o => o.Key).ToList();
                for (int i = 0; i < listAcc2.Count; i++)
                {
                    rawText += listAcc2[i].Value;
                    rawText += i < listAcc2.Count - 1 ? "\n" : "";
                }
                Clipboard.SetText(rawText);
            }
        }
        // copy id | pass | token
        private void CopyAccount2(Object sender, System.EventArgs e)
        {
            if (grdAccount.SelectedRows.Count > 0)
            {
                string rawText = "";
                int count = 0;
                List<KeyValuePair<int, string>> listAcc = new List<KeyValuePair<int, string>>();
                foreach (DataGridViewRow selectedRow in grdAccount.SelectedRows)
                {
                    count++;
                    var row = (AccountInfor)selectedRow.DataBoundItem;
                    listAcc.Add(new KeyValuePair<int, string>(selectedRow.Index, row.Username + "|" + row.Password + "|" + row.AccessToken));
                }
                // sort by index
                var listAcc2 = listAcc.OrderBy(o => o.Key).ToList();
                for (int i = 0; i < listAcc2.Count; i++)
                {
                    rawText += listAcc2[i].Value;
                    rawText += i < listAcc2.Count - 1 ? "\n" : "";
                }
                Clipboard.SetText(rawText);
            }
        }
        private void TangXu(Object sender, System.EventArgs e)
        {
            string userNhanXu = txtUserNhanXu.Text;
            int soXuTang = (int)numSoXuTang.Value;
            int soXuTang2 = (int)(soXuTang * 1.1);
            if (grdAccount.SelectedRows.Count > 1)
            {
                MessageBox.Show("Chỉ tặng xu trên từng acc một lần");
            }
            else if (grdAccount.SelectedRows.Count == 1 && userNhanXu.Length > 0 && soXuTang >= 1000000)
            {
                foreach (DataGridViewRow selectedRow in grdAccount.SelectedRows)
                {
                    var row = (AccountInfor)selectedRow.DataBoundItem;
                    if (row.Coin < soXuTang2)
                    {
                        MessageBox.Show("Thiếu xu để tặng (Xu + 10% thuế): " + (soXuTang2 - row.Coin));
                    }
                    else if (row.Username.Length == 0 || row.Password.Length == 0)
                    {
                        MessageBox.Show("Cần có tài khoản, mật khẩu để tặng xu");
                    }
                    else
                    {
                        DialogResult result1 = MessageBox.Show("Bạn có chắc chắn tặng " + soXuTang + " xu cho " + userNhanXu + " không?",
                                   "Xác nhận tặng xu",
                                   MessageBoxButtons.YesNo);
                        if (result1 == DialogResult.Yes)
                        {
                            var task = TangXu(row, userNhanXu, soXuTang + "");
                            task.Start();
                        }
                    }
                    ReloadGrid();
                    SaveFile();
                }
            }
        }

        private void RemoveAccount(Object sender, System.EventArgs e)
        {
            if (grdAccount.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn xóa " + grdAccount.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận xóa dòng",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
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
            }
        }

        #region User Event Handler
        private async void btnGetToken_Click(object sender, EventArgs e)
        {
            semaphore = new SemaphoreSlim((int)numLuong.Value);
            try
            {
                // to do: get by selected row, not all
                if (grdAccount.SelectedRows.Count > 0)
                {
                    var lstTask = new List<Task>();
                    foreach (DataGridViewRow sltRow in grdAccount.SelectedRows)
                    {
                        var row = (AccountInfor)sltRow.DataBoundItem;
                        if (row.Username.Length < 1 || row.Password.Length < 1)
                        {
                            row.State = "Tài khoản, mật khẩu trống";
                            continue;
                        }
                        await semaphore.WaitAsync();
                        row.State = "Đang đợi lấy token";
                        var task = GetToken(row);
                        lstTask.Add(task);
                        task.Start();
                    }
                    ReloadGrid();
                    await Task.WhenAll(lstTask.ToArray());

                    ReloadGrid();
                    SaveFile();
                    MessageBox.Show("Hoàn thành");
                }
                else if (lstAccountInfor.Count > 0)
                {
                    var lstTask = new List<Task>();
                    foreach (var row in lstAccountInfor)
                    {
                        if (row.Username.Length < 1 || row.Password.Length < 1)
                        {
                            row.State = "Tài khoản, mật khẩu trống";
                            continue;
                        }
                        await semaphore.WaitAsync();
                        row.State = "Đang đợi lấy token";
                        var task = GetToken(row);
                        lstTask.Add(task);
                        task.Start();
                    }
                    ReloadGrid();
                    await Task.WhenAll(lstTask.ToArray());
                    ReloadGrid();
                    SaveFile();
                    MessageBox.Show("Hoàn thành");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                semaphore.Release(40);
            }
        }

        private async void btnGetCoin_Click(object sender, EventArgs e)
        {
            semaphore = new SemaphoreSlim((int)numLuong.Value);
            try
            {
                // to do: get by selected row, not all
                if (grdAccount.SelectedRows.Count > 0)
                {
                    var lstTask = new List<Task>();
                    foreach (DataGridViewRow sltRow in grdAccount.SelectedRows)
                    {
                        await semaphore.WaitAsync();
                        var row = (AccountInfor)sltRow.DataBoundItem;
                        bool emptId = row.Username.Length < 1 || row.Password.Length < 1;
                        row.State = "Đang đợi lấy xu";
                        var task = emptId ? GetCoin(row) : GetCoin2(row);
                        lstTask.Add(task);
                        task.Start();
                    }
                    ReloadGrid();
                    await Task.WhenAll(lstTask.ToArray());
                    ReloadGrid();
                    LoadTotalCoin();
                    SaveFile();
                    MessageBox.Show("Hoàn thành");
                }
                else if (lstAccountInfor.Count > 0)
                {
                    var lstTask = new List<Task>();
                    foreach (var row in lstAccountInfor)
                    {
                        await semaphore.WaitAsync();
                        bool emptId = row.Username.Length < 1 || row.Password.Length < 1;
                        row.State = "Đang đợi lấy xu";
                        var task = emptId ? GetCoin(row) : GetCoin2(row);
                        lstTask.Add(task);
                        task.Start();
                    }

                    ReloadGrid();
                    await Task.WhenAll(lstTask.ToArray());

                    ReloadGrid();
                    LoadTotalCoin();
                    SaveFile();
                    MessageBox.Show("Hoàn thành");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                semaphore.Release((int)numLuong.Value);
            }
        }

        private async void ChangePasswordMenuItem_Click(object sender, EventArgs e)
        {

            if (grdAccount.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn đổi mật khẩu của " + grdAccount.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận đổi mật khẩu",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    semaphore = new SemaphoreSlim((int)numLuong.Value);
                    try
                    {
                        var lstTask = new List<Task>();
                        foreach (DataGridViewRow selectedRow in grdAccount.SelectedRows)
                        {
                            var row = (AccountInfor)selectedRow.DataBoundItem;
                            if (row.Username.Length > 0 && row.Password.Length > 0)
                            {
                                await semaphore.WaitAsync();
                                row.State = "Đang đợi đổi mật khẩu";
                                var task = ChangePassword(row);
                                lstTask.Add(task);
                                task.Start();
                                Thread.Sleep(60);
                            }
                            else
                            {
                                row.State = "Tài khoản hoặc mật khẩu trống";
                            }
                        }
                        ReloadGrid();
                        await Task.WhenAll(lstTask.ToArray());
                        ReloadGrid();
                        SaveFile();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        semaphore.Release((int)numLuong.Value);
                    }
                }
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
                RemoveAccount(sender, e);
            }
            LoadFile();
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
                finally
                {
                    semaphore.Release();
                }
            });
        }

        // Get coin by token
        private Task GetCoin(AccountInfor row)
        {
            return new Task(() =>
            {
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
                finally
                {
                    semaphore.Release();
                }
            });
        }

        // Get coin by Id Pass
        private Task GetCoin2(AccountInfor row)
        {
            return new Task(() =>
            {
                try
                {
                    if (String.IsNullOrEmpty(row.AccessToken))
                    {
                        row.State = "Chưa lấy token!";
                    }
                    else
                    {
                        var tdsProxy = new TDSProxy();
                        var getCoinResponse = tdsProxy.GetCoin2(row).Result;
                        if (getCoinResponse == null || String.IsNullOrEmpty(getCoinResponse.xu))
                        {
                            throw new Exception($"getCoinResponse error: {getCoinResponse}");
                        }

                        row.Coin = String.IsNullOrEmpty(getCoinResponse.xu) ? 0 : int.Parse(getCoinResponse.xu);
                        row.State = "Thành công!";
                    }
                }
                catch (Exception ex)
                {
                    row.State = $"Có lỗi xảy ra: {ex.Message}!";
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }

        private Task TangXu(AccountInfor row, string userNhan, string soXu)
        {
            return new Task(() =>
            {
                try
                {
                    var tdsProxy = new TDSProxy();
                    tdsProxy.Login(row).Wait();
                    var tangXuResponse = tdsProxy.TangXu(row, userNhan, soXu).Result;
                    row.State = "Tặng xu thành công";
                }
                catch (Exception ex)
                {
                    row.State = $"Lỗi khi tặng xu: {ex.Message}!";
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
                    var newPass = Utilities.RandomString(16);
                    var changePasswordResponse = tdsProxy.ChangePassword(row, newPass).Result;
                    if (changePasswordResponse != "0")
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
                finally
                {
                    semaphore.Release();
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
