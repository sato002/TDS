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
using static BVH.TDS.Utilities;

namespace BVH.TDS
{
    public partial class Form1 : Form
    {
        private static string ACCOUNT_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "accounts.json");
        private static string CHECKXU_FILE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "checkxu.txt");
        private int totalCoin = 0;
        private DateTime lastDate;
        private int lastCoin;
        private SemaphoreSlim semaphore;
        private List<AccountInfor> listAccountInfor;
        public Form1()
        {
            CreateFile(ACCOUNT_FILE_PATH, "[]");
            CreateFile(CHECKXU_FILE_PATH, DateTime.Now + "\n0");
            InitializeComponent();
            InitializeDataGridView();
            LoadTotalCoin();
            DateTime.TryParse(File.ReadAllText(CHECKXU_FILE_PATH).Split('\n')[0], out lastDate);
            lastCoin = Int32.Parse(File.ReadAllText(CHECKXU_FILE_PATH).Split('\n')[1]);
            UpdateXu();
        }

        private void InitializeDataGridView()
        {
            listAccountInfor = new List<AccountInfor>();
            LoadFile();
            gridAccInfor.AllowUserToAddRows = false;
            gridAccInfor.Columns[(int)EnumColumnOrder.Username].Width = 100;
            gridAccInfor.Columns[(int)EnumColumnOrder.Password].Width = 90;
            gridAccInfor.Columns[(int)EnumColumnOrder.AccessToken].Width = 90;
            gridAccInfor.Columns[(int)EnumColumnOrder.QuickLink].Width = 60;
            gridAccInfor.Columns[(int)EnumColumnOrder.TikUsername].Width = 80;
            gridAccInfor.Columns[(int)EnumColumnOrder.Coin].Width = 80;
            gridAccInfor.Columns[(int)EnumColumnOrder.State].Width = 200;
            gridAccInfor.Columns[(int)EnumColumnOrder.CoinDie].Width = 80;
        }

        private void grdAccount_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool hasRowSelected = gridAccInfor.SelectedRows.Count > 0;
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Thêm acc (Tài khoản|Mật khẩu|Token)", AddAccountFromClipboard));
                m.MenuItems.Add("-");
                m.MenuItems.Add(new MenuItem("Copy acc (Tài khoản|Mật khẩu)", CopyAccount));
                m.MenuItems.Add(new MenuItem("Copy acc (Tài khoản|Mật khẩu|Token)", CopyAccount2));
                m.MenuItems.Add(new MenuItem("Copy cột xu", CopyCoin));
                m.MenuItems.Add("-");
                m.MenuItems.Add(new MenuItem("Đổi mật khẩu (sinh random)", ChangePasswordMenuItem_Click));
                m.MenuItems.Add("-");
                m.MenuItems.Add(new MenuItem("Tặng xu", TangXu));
                m.MenuItems.Add("-");
                m.MenuItems.Add(new MenuItem("Xóa dòng đã chọn", RemoveAccount));
                m.MenuItems.Add(new MenuItem("Xóa dòng đã chọn (dưới 1.1M xu)", RemoveAccountSmall));
                m.MenuItems.Add(new MenuItem("Xóa dòng đã chọn (trên 1.1M xu)", RemoveAccountBig));

                m.MenuItems[2].Enabled = hasRowSelected;
                m.MenuItems[3].Enabled = hasRowSelected;
                m.MenuItems[4].Enabled = hasRowSelected;
                m.MenuItems[6].Enabled = hasRowSelected;
                m.MenuItems[8].Enabled = hasRowSelected && txtUserNhanXu.Text.Length > 0 && (numSoXuTang.Value >= 1000000 || this.ckbAllXu.Checked);
                m.MenuItems[10].Enabled = hasRowSelected;
                m.MenuItems[11].Enabled = hasRowSelected;
                m.MenuItems[12].Enabled = hasRowSelected;

                m.Show(gridAccInfor, new Point(e.X, e.Y));
            }
        }

        private void AddAccountFromClipboard(Object sender, System.EventArgs e)
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
                        gridAccInfor.Rows.Clear();
                        for (int i = 0; i < rowText.Length; i++)
                        {
                            string row = rowText[i];
                            string[] rowSplit = row.Split('|');
                            // duplicate check
                            List<AccountInfor> duplicateList = listAccountInfor.FindAll(
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
                                listAccountInfor.Add(new AccountInfor()
                                {
                                    Username = rowSplit[0],
                                    Password = rowSplit[1],
                                    AccessToken = rowSplit[2]
                                });
                            }
                            else if (rowSplit.Length == 2)
                            {
                                listAccountInfor.Add(new AccountInfor()
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

        // copy id | pass
        private void CopyAccount(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                string rawText = "";
                int count = 0;
                List<KeyValuePair<int, string>> listAcc = new List<KeyValuePair<int, string>>();
                foreach (DataGridViewRow selectedRow in gridAccInfor.SelectedRows)
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
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                string rawText = "";
                int count = 0;
                List<KeyValuePair<int, string>> listAcc = new List<KeyValuePair<int, string>>();
                foreach (DataGridViewRow selectedRow in gridAccInfor.SelectedRows)
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

        // copy coin
        private void CopyCoin(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                string rawText = "";
                int count = 0;
                List<KeyValuePair<int, string>> listAcc = new List<KeyValuePair<int, string>>();
                foreach (DataGridViewRow selectedRow in gridAccInfor.SelectedRows)
                {
                    count++;
                    var row = (AccountInfor)selectedRow.DataBoundItem;
                    listAcc.Add(new KeyValuePair<int, string>(selectedRow.Index, row.Coin + ""));
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
        private async void TangXu(Object sender, System.EventArgs e)
        {
            semaphore = new SemaphoreSlim((int)numLuong.Value);
            string userNhanXu = txtUserNhanXu.Text;
            int soXuTang = (int)numSoXuTang.Value;
            int soXuTang2 = (int)(soXuTang * 1.1);
            if (gridAccInfor.SelectedRows.Count > 0 && userNhanXu.Length > 0 && (soXuTang >= 1000000 || this.ckbAllXu.Checked))
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn tặng" + (this.ckbAllXu.Checked ? " TẤC CẢ" : "") + " xu của "
                    + gridAccInfor.SelectedRows.Count + " dòng đã chọn cho " + userNhanXu + " không?",
                           "Xác nhận tặng xu",
                           MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    var lstTask = new List<Task>();
                    foreach (DataGridViewRow selectedRow in gridAccInfor.SelectedRows)
                    {
                        var row = (AccountInfor)selectedRow.DataBoundItem;
                        // min 1.100.000
                        if (row.Coin < 1100000 && this.ckbAllXu.Checked)
                        {
                            row.State = "Thiếu xu để tặng (tối thiểu 1.100.000 xu)";
                        }
                        else if (row.Coin < soXuTang2 && !this.ckbAllXu.Checked)
                        {
                            row.State = "Thiếu xu để tặng (Xu + 10% thuế): " + (soXuTang2 - row.Coin);
                        }
                        else if (row.Username.Length == 0 || row.Password.Length == 0)
                        {
                            row.State = "Cần có tài khoản, mật khẩu để tặng xu";
                        }
                        else
                        {
                            await semaphore.WaitAsync();
                            row.State = "Đang đăng nhập vào tài khoản...";
                            if (this.ckbAllXu.Checked)
                            {
                                // max 5.000.000
                                if (row.Coin >= 5000000)
                                {
                                    soXuTang = 5000000;
                                }
                                else
                                {
                                    soXuTang = (int)Math.Floor(row.Coin / 1.1);
                                }
                            }
                            var task = TangXu(row, userNhanXu, soXuTang + "");
                            lstTask.Add(task);
                            task.Start();
                        }
                    }
                    await Task.WhenAll(lstTask.ToArray());
                    ReloadGrid();
                    SaveFile();
                    MessageBox.Show("Hoàn thành tặng xu, vui lòng bấm check xu để cập nhật lại số xu.");
                }
            }
        }

        private void RemoveAccount(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn xóa " + gridAccInfor.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận xóa dòng",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int count = 0;
                    foreach (DataGridViewRow row in gridAccInfor.SelectedRows)
                    {
                        listAccountInfor.Remove((AccountInfor)row.DataBoundItem);
                        gridAccInfor.Rows.RemoveAt(row.Index);
                        count++;
                    }
                    LoadTotalCoin();
                    SaveFile();
                    LoadGridInfor();
                    MessageBox.Show("Đã xóa " + count + " dòng.");
                }
            }
        }
        private void RemoveAccountSmall(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả acc DƯỚI 1.1M xu trong " + gridAccInfor.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận xóa dòng",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int count = 0;
                    foreach (DataGridViewRow row in gridAccInfor.SelectedRows)
                    {
                        var rowData = (AccountInfor)row.DataBoundItem;
                        if (rowData.Coin < 1100000)
                        {
                            listAccountInfor.Remove((AccountInfor)row.DataBoundItem);
                            gridAccInfor.Rows.RemoveAt(row.Index);
                            count++;
                        }
                    }
                    LoadTotalCoin();
                    SaveFile();
                    LoadGridInfor();
                    MessageBox.Show("Đã xóa " + count + " dòng.");
                }
            }
        }
        private void RemoveAccountBig(Object sender, System.EventArgs e)
        {
            if (gridAccInfor.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả acc TRÊN 1.1M xu trong " + gridAccInfor.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận xóa dòng",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int count = 0;
                    foreach (DataGridViewRow row in gridAccInfor.SelectedRows)
                    {
                        var rowData = (AccountInfor)row.DataBoundItem;
                        if (rowData.Coin >= 1100000)
                        {
                            listAccountInfor.Remove((AccountInfor)row.DataBoundItem);
                            gridAccInfor.Rows.RemoveAt(row.Index);
                            count++;
                        }
                    }
                    LoadTotalCoin();
                    SaveFile();
                    LoadGridInfor();
                    MessageBox.Show("Đã xóa " + count + " dòng.");
                }
            }
        }

        #region User Event Handler
        private async void btnGetCoin_Click(object sender, EventArgs e)
        {
            semaphore = new SemaphoreSlim((int)numLuong.Value);
            try
            {
                DateTime.TryParse(File.ReadAllText(CHECKXU_FILE_PATH).Split('\n')[0], out lastDate);
                lastCoin = Int32.Parse(File.ReadAllText(CHECKXU_FILE_PATH).Split('\n')[1]);
                var lstTask = new List<Task>();
                foreach (var row in listAccountInfor)
                {
                    await semaphore.WaitAsync();
                    row.State = "Đang đợi lấy xu";
                    var task = GetAccInfor(row);
                    lstTask.Add(task);
                    task.Start();
                }
                await Task.WhenAll(lstTask.ToArray());
                ReloadGrid();
                LoadTotalCoin();
                File.WriteAllText(CHECKXU_FILE_PATH, DateTime.Now.ToString() + "\n" + totalCoin);
                SaveFile();
                MessageBox.Show("Hoàn thành check xu của " + listAccountInfor.Count + " dòng.\nTính từ lúc " + lbLastCheck.Text + ", " + lbDiffXu.Text);
                UpdateXu();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async void btnNhanXuTim_Click(object sender, EventArgs e)
        {
            semaphore = new SemaphoreSlim((int)numLuong.Value);
            try
            {
                var lstTask = new List<Task>();
                foreach (var row in listAccountInfor)
                {
                    await semaphore.WaitAsync();
                    row.State = "Đang đợi nhận xu";
                    var task = NhanXuTim(row);
                    lstTask.Add(task);
                    task.Start();
                }
                await Task.WhenAll(lstTask.ToArray());
                ReloadGrid();
                SaveFile();
                MessageBox.Show("Hoàn thành nhận xu của " + listAccountInfor.Count + " dòng");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async void ChangePasswordMenuItem_Click(object sender, EventArgs e)
        {

            if (gridAccInfor.SelectedRows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Bạn có chắc chắn muốn đổi mật khẩu của " + gridAccInfor.SelectedRows.Count + " dòng đã chọn không?",
                       "Xác nhận đổi mật khẩu",
                       MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    semaphore = new SemaphoreSlim((int)numLuong.Value);
                    try
                    {
                        var lstTask = new List<Task>();
                        foreach (DataGridViewRow selectedRow in gridAccInfor.SelectedRows)
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
                        await Task.WhenAll(lstTask.ToArray());
                        ReloadGrid();
                        SaveFile();
                        MessageBox.Show("Hoàn thành đổi mật khẩu của " + gridAccInfor.SelectedRows.Count + " dòng đã chọn.");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        semaphore.Release();
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
                LoadFile();
            }
        }
        #endregion

        #region TDS Task
        private Task GetAccInfor(AccountInfor row)
        {
            return new Task(() =>
            {
                try
                {
                    if (String.IsNullOrEmpty(row.Username) || String.IsNullOrEmpty(row.Password))
                    {
                        row.State = "Tài khoản hoặc mật khẩu trống!";
                    }
                    else
                    {
                        var tdsProxy = new TDSProxy();
                        var res = tdsProxy.GetAccInfor(row).Result ?? throw new Exception("getTokenResponse null");
                        row.AccessToken = String.IsNullOrEmpty(res.access_token) ? "" : res.access_token;
                        row.Coin = String.IsNullOrEmpty(res.xu) ? 0 : int.Parse(res.xu);
                        row.CoinDie = String.IsNullOrEmpty(res.xudie) ? 0 : int.Parse(res.xudie);
                        row.State = "Thành công!";
                    }
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
                finally
                {
                    semaphore.Release();
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
                    var newPass = RandomString(16);
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
        private Task NhanXuTim(AccountInfor row)
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
                        var nhanXuResponse = tdsProxy.NhanXuTim(row).Result;
                        if (nhanXuResponse == null || nhanXuResponse.success != 200 || nhanXuResponse.data == null)
                        {
                            throw new Exception($"getCoinResponse error: {nhanXuResponse}");
                        }
                        var data = nhanXuResponse.data;
                        row.State = "Nhận xu thành công: " + data.msg;
                    }
                }
                catch (Exception ex)
                {
                    row.State = $"Lỗi khi nhận xu: {ex.Message}!";
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
            lbCountSelected.Text = gridAccInfor.SelectedRows.Count.ToString();
        }
        private void grdAccount_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == (int)EnumColumnOrder.State && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = Color.Black;
                var stateValue = gridAccInfor[e.ColumnIndex, e.RowIndex].Value;
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
            else if (e.ColumnIndex == (int)EnumColumnOrder.Coin && e.RowIndex >= 0)
            {
                int coinValue = Int32.Parse(gridAccInfor[e.ColumnIndex, e.RowIndex].Value + "");
                if (coinValue >= 1100000)
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
            }
            else if (e.ColumnIndex == (int)EnumColumnOrder.CoinDie && e.RowIndex >= 0)
            {
                int coinValue = Int32.Parse(gridAccInfor[e.ColumnIndex, e.RowIndex].Value + "");
                if (coinValue > 0)
                {
                    e.CellStyle.BackColor = Color.OrangeRed;
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAllXu.Checked)
            {
                this.numSoXuTang.Enabled = false;
            }
            else
            {
                this.numSoXuTang.Enabled = true;
            }
        }
        #endregion


        #region Private method
        private void ReloadGrid()
        {
            var bindingList = new SortableBindingList<AccountInfor>(listAccountInfor);
            var source = new BindingSource(bindingList, null);
            gridAccInfor.DataSource = source;
            gridAccInfor.Update();
            gridAccInfor.Refresh();
            LoadGridInfor();
        }
        private void LoadGridInfor()
        {
            listAccountInfor = listAccountInfor ?? new List<AccountInfor>();
            lbCountAll.Text = listAccountInfor.Count.ToString();
            lbCountSelected.Text = gridAccInfor.SelectedRows.Count.ToString();
        }
        private void LoadTotalCoin()
        {
            if (listAccountInfor != null && listAccountInfor.Count > 0)
            {
                totalCoin = listAccountInfor.Select(_ => _.Coin).Sum();
                lbTotalCoin.Text = PrettyNumber(totalCoin);
                var canSell = listAccountInfor.Where(_ => _.Coin >= 1100000);
                var totalCoinSell = canSell.Select(_ => _.Coin).Sum();
                lbTotalCoinSell.Text = PrettyNumber(totalCoinSell);
                lblCountCanSell.Text = canSell.ToList().Count.ToString();
            }
            else
            {
                totalCoin = 0;
                lbTotalCoin.Text = "0";
                lbTotalCoinSell.Text = "0";
                lblCountCanSell.Text = "0";
            }
        }
        private void UpdateXu()
        {
            double diffMin = Math.Round((DateTime.Now - lastDate).TotalMinutes * 10) / 10;
            lbLastCheck.Text = lastDate + " (" + Math.Floor(diffMin / 60) + "h:" + Math.Floor(diffMin % 60) + "m)";
            int diffXu = totalCoin - lastCoin;
            if (diffXu < 0)
            {
                lbDiffXu.ForeColor = Color.Red;
                lbDiffXu.Text = "-" + PrettyNumber(diffXu) + " Xu";
            }
            else
            {
                lbDiffXu.ForeColor = Color.Green;
                lbDiffXu.Text = "+" + PrettyNumber(diffXu) + " Xu";
            }
        }
        private void LoadFile()
        {
            if (File.Exists(ACCOUNT_FILE_PATH))
            {
                var json = File.ReadAllText(ACCOUNT_FILE_PATH);
                if (!String.IsNullOrEmpty(json))
                {
                    listAccountInfor = JsonConvert.DeserializeObject<List<AccountInfor>>(json);
                    ReloadGrid();
                }
            }
        }
        private void SaveFile()
        {
            listAccountInfor = listAccountInfor ?? new List<AccountInfor>();
            string json = JsonConvert.SerializeObject(listAccountInfor);

            //write string to file
            File.WriteAllText(ACCOUNT_FILE_PATH, json);
        }
        private string PrettyNumber(int value)
        {
            return String.Format(new CultureInfo("vi-VN"), "{0:N0}", value);
        }
        #endregion
    }
}
