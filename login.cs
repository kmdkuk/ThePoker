using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace ThePoker
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        DataTable IDlist = new DataTable();
        CSV _csv = new CSV();
        Encrypt _encrypt = new Encrypt();
        private int _id;
        public int nid
        {
            get { return _id; }
            set { _id = value; }
        }
        private void login_Load(object sender, EventArgs e)
        {
            IDlist.Columns.Clear();
            IDlist.Clear();
            _csv.ReadCSV(IDlist, false, "coin.csv", ",", false);
        }
        

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "" && txtPass.Text != "")
            {
                bool flg = false;
                for (int i = 0; i < IDlist.Rows.Count; i++)
                {
                    if (txtID.Text == IDlist.Rows[i][0].ToString())
                    { flg = true; }
                }
                if (flg)
                {
                    MessageBox.Show("申し訳ありません。\n" +
                        "そのIDはすでに使用されています。");
                    init();
                }
                else
                {
                    DataRow datarow = IDlist.NewRow();
                    datarow[0] = txtID.Text;
                    datarow[1] = _encrypt.EncryptString(txtPass.Text, txtPass.Text);//passでpassを暗号化
                    datarow[2] = _encrypt.EncryptString("1000", txtID.Text);//idでcoinを暗号化
                    IDlist.Rows.Add(datarow);
                    _id = IDlist.Rows.Count - 1;

                    MessageBox.Show("ようこそ" + IDlist.Rows[_id][0] + "様！\nコイン1000枚プレゼント！");
                    this.Close();
                }
            }
            else if (txtID.Text == "" && txtPass.Text == "")
            {
                MessageBox.Show("IDとパスワードを入力してください");
                init();
            }
            else if (txtID.Text == "")
            {
                MessageBox.Show("IDを入力してください");
                init();
            }
            else if (txtPass.Text == "")
            {
                MessageBox.Show("パスワードを入力してください");
                init();
            }
        }
        private void init()
        {
            txtID.Text = "";
            txtPass.Text = "";
            txtID.Focus();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int number;
            bool p = false;
            for (number = 0; number < IDlist.Rows.Count; number++)
            {
                if (IDlist.Rows[number][0].ToString() == txtID.Text)
                {
                    if (IDlist.Rows[number][1].ToString() == _encrypt.EncryptString(txtPass.Text, txtPass.Text))
                    {
                        _id = number;
                        p = true;
                    }
                    break;
                }
            }
            if (p)
            {
                MessageBox.Show("ようこそ　" + IDlist.Rows[_id][0].ToString() + "様\n" +
                    "現在のコインの枚数は" + _encrypt.DecryptString(IDlist.Rows[_id][2].ToString(), IDlist.Rows[_id][0].ToString()) + "枚です");
                this.Close();

            }
            else
            {
                MessageBox.Show("IDまたはパスワードが正しくありません。");
                _id = 0;
                init();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(IDlist.Rows[nid][0].ToString() + "様お疲れ様です。\nログアウトしました。");
            _id = 0;
        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            //CSV処理
            IDlist.Rows[0][0] = "Guest";
            IDlist.Rows[0][1] = "";
            IDlist.Rows[0][2] = _encrypt.EncryptString("1000", IDlist.Rows[0][0].ToString());
            _csv.ConvertDataTableToCsv(IDlist, "coin.csv", false);
        }
    }
}
