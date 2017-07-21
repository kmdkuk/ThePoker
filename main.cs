using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;

namespace ThePoker
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }
        #region 宣言処理
        enum mode//ゲームモードの種類
        {
            MODE_SELECT,
            MODE_ALONE,
            MODE_VS,
        }
        enum suit//マークhdscj
        {
            SUIT_HEART,
            SUIT_DIA,
            SUIT_SPADE,
            SUIT_CLOVER,
            SUIT_JOKER,
        }
        string[] suitList = new string[5] { "h", "d", "s", "c", "j" };
        enum result//役
        {
            HighCards,
            OnePair,
            TwoPair,
            ThereeCard,
            Straight,
            Flush,
            FullHouse,
            FourCard,
            StraightFlush,
            RoyalStraightFlush,
            FiveCard,
        }
        mode gamemode = new mode();
        Random rnd = new Random();
        CSV _csv = new CSV();
        Encrypt _encrypt = new Encrypt();
        int[] Stack = new int[54];//山札
        int[] PHands = new int[5];//プレイヤー手札
        bool[] PHolds = new bool[5];//プレイヤー手札と対応
        result pre = new result();//プレイヤーResult
        string pra;//プレイヤーランク
        int[] DHands = new int[5];//ディーラー手札
        bool[] DHolds = new bool[5];//ディーラー手札と対応
        result dre = new result();//ディーラーリザルト
        string dra;//ディーラーランク
        int bet;//ベット額
        int Snow;//stackの枚数
        DataTable IDlist = new DataTable();
        int number;//ID番号保存用
        string ID;//ID保存
        int coin;//コイン枚数保存
        #endregion
        #region 初期化処理
        private void main_Load(object sender, EventArgs e)
        {
            forminit();
        }
        public void forminit()
        {
            menuStrip1.Visible = true;
            readIDlist();
            #region ゲーム選択画面への初期化
            gamemode = mode.MODE_SELECT;
            this.Size = new Size(400, 250);
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            lblID.Visible = true;
            lblCoin.Visible = true;
            btnPlaymode1.Visible = true;
            btnPlaymode2.Visible = true;
            label1.Location = new Point(30, 68);
            label2.Location = new Point(25, 130);
            label3.Location = new Point(160, 45);
            lblID.Location = new Point(161, 26);
            lblCoin.Location = new Point(280, 45);
            btnPlaymode1.Location = new Point(60, 150);
            btnPlaymode2.Location = new Point(235, 150);
            timer1.Stop();

            btnStart.Visible = false;
            btnDraw.Visible = false;
            picStack.Visible = false;
            lblStart.Visible = false;
            lblStackCount.Visible = false;

            label5.Visible = false;
            DResult.Visible = false;
            DHands1.Visible = false;
            DHands2.Visible = false;
            DHands3.Visible = false;
            DHands4.Visible = false;
            DHands5.Visible = false;
            for (int i = 1; i < 55; i++)
            {
                string s = suitList[(int)getsuit(i)];
                string r = getrank(i, 0);
                //pictureboxをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find(s + r, true);
                //pictureboxが見つかれば、locationを変更する
                if (cs.Length > 0)
                {
                    ((PictureBox)cs[0]).Visible = false;
                }
            }
            新規ゲームNToolStripMenuItem.Enabled = false;
            #endregion
        }
        #endregion
        #region ゲームの処理
        private void btnPlaymode1_Click(object sender, EventArgs e)
        {
            //btndebug.Visible = true;
            #region 一人ポーカーフォームの初期化
            gamemode = mode.MODE_ALONE;
            this.Size = new Size(500, 500);
            label1.Visible = false;
            label2.Visible = false;
            btnPlaymode1.Visible = false;
            btnPlaymode2.Visible = false;

            lblStart.Visible = true;
            lblDraw.Visible = true;
            btnStart.Visible = true;
            btnDraw.Visible = true;

            picStack.Visible = true;
            lblStackCount.Visible = true;

            label4.Visible = true;
            PResult.Visible = true;
            PHands1.Visible = true;
            PHands2.Visible = true;
            PHands3.Visible = true;
            PHands4.Visible = true;
            PHands5.Visible = true;

            hold1.Visible = true;
            hold2.Visible = true;
            hold3.Visible = true;
            hold4.Visible = true;
            hold5.Visible = true;
            checkBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            checkBox5.Visible = true;


            label3.Location = new Point(280, 45);
            lblID.Location = new Point(275, 26);
            lblCoin.Location = new Point(394, 45);

            lblStart.Location = new Point(240, 100);
            lblDraw.Location = new Point(85, 400);
            btnStart.Location = new Point(300, 150);
            btnDraw.Location = new Point(209, 435);

            picStack.Location = new Point(95, 70);
            lblStackCount.Location = new Point(95, 190);

            label4.Location = new Point(56, 230);
            PResult.Location = new Point(186, 227);
            PHands1.Location = new Point(55, 260);
            PHands2.Location = new Point(132, 260);
            PHands3.Location = new Point(209, 260);
            PHands4.Location = new Point(286, 260);
            PHands5.Location = new Point(363, 260);

            hold1.Location = new Point(55, 300);
            hold2.Location = new Point(132, 300);
            hold3.Location = new Point(209, 300);
            hold4.Location = new Point(286, 300);
            hold5.Location = new Point(363, 300);

            checkBox1.Location = new Point(55, 370);
            checkBox2.Location = new Point(132, 370);
            checkBox3.Location = new Point(209, 370);
            checkBox4.Location = new Point(286, 370);
            checkBox5.Location = new Point(363, 370);

            timer1.Start();
            btnStart.Enabled = true;
            btnDraw.Enabled = false;
            新規ゲームNToolStripMenuItem.Enabled = true;
            #endregion
        }

        private void btnPlaymode2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Visible = true;
            #region　VSポーカーフォーム初期化
            gamemode = mode.MODE_VS;
            this.Size = new Size(650, 640);
            label1.Visible = false;
            label2.Visible = false;
            btnPlaymode1.Visible = false;
            btnPlaymode2.Visible = false;

            lblStart.Visible = true;
            lblDraw.Visible = true;
            lblBet.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            btnStart.Visible = true;
            btnDraw.Visible = true;
            btnBet.Visible = true;
            btnCall.Visible = true;
            btnRaise.Visible = true;


            picStack.Visible = true;
            lblStackCount.Visible = true;

            label4.Visible = true;
            PResult.Visible = true;
            PHands1.Visible = true;
            PHands2.Visible = true;
            PHands3.Visible = true;
            PHands4.Visible = true;
            PHands5.Visible = true;

            label5.Visible = true;
            DResult.Visible = true;
            DHands1.Visible = true;
            DHands2.Visible = true;
            DHands3.Visible = true;
            DHands4.Visible = true;
            DHands5.Visible = true;

            hold1.Visible = true;
            hold2.Visible = true;
            hold3.Visible = true;
            hold4.Visible = true;
            hold5.Visible = true;
            checkBox1.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox4.Visible = true;
            checkBox5.Visible = true;


            label3.Location = new Point(425, 45);
            lblID.Location = new Point(425, 26);
            lblCoin.Location = new Point(544, 45);

            lblStart.Location = new Point(265, 244);
            lblDraw.Location = new Point(111, 544);
            btnStart.Location = new Point(323, 297);
            btnDraw.Location = new Point(236, 579);

            picStack.Location = new Point(120, 215);
            lblStackCount.Location = new Point(120, 335);

            label4.Location = new Point(82, 373);
            PResult.Location = new Point(212, 370);
            PHands1.Location = new Point(81, 404);
            PHands2.Location = new Point(158, 404);
            PHands3.Location = new Point(235, 404);
            PHands4.Location = new Point(312, 404);
            PHands5.Location = new Point(389, 404);

            hold1.Location = new Point(81, 444);
            hold2.Location = new Point(158, 444);
            hold3.Location = new Point(235, 444);
            hold4.Location = new Point(312, 444);
            hold5.Location = new Point(389, 444);

            checkBox1.Location = new Point(81, 514);
            checkBox2.Location = new Point(158, 514);
            checkBox3.Location = new Point(235, 514);
            checkBox4.Location = new Point(312, 514);
            checkBox5.Location = new Point(389, 514);

            timer1.Start();
            btnStart.Enabled = false;
            btnDraw.Enabled = false;
            btnBet.Enabled = true;

            numericUpDown1.Maximum = coin;
            label7.Text = "現在" + bet.ToString() + "枚\n賭けられています";
            新規ゲームNToolStripMenuItem.Enabled = true;
            #endregion
        }
        #region デバック用
        private void button3_Click(object sender, EventArgs e)
        {
            PHands = new int[] { 1, 14, 54, 33, 34 };//作りたい手札
            /*
             * デバックリスト
             * 1,14,27,40,53 =five(A)J              
             * 1,14,27,54,53 =five(A)J              
             * 
             * 1,10,11,12,13 = RStraightFlush(A)    
             * 1,10,11,12,53 = RStraightFlush(A)J   
             * 1,10,11,54,53 = RStraightFlush(A)JJ  
             *
             * 1,2,3,4,5 = straightFlush(5)         
             * 1,2,3,4,53 = straightFlush(5)J       
             * 1,2,3,54,53 = straightFlush(5)JJ     
             *
             * 1,14,27,40,23 = four(A)              
             * 1,14,27,53,23 = four(A)J             
             * 1,14,54,53,23 = four(A)JJ              
             * 
             * 1,14,27,2,15 = fullhouse(A)             
             * 1,14,54,2,15 = fullhouse(A)J                
             * 
             * 2,4,6,8,10 = flush(10)                  
             * 2,4,6,8,53 = flush(A)J               
             * 2,8,7,53,54 = flush(A)JJ                 ○
             * 
             * 1,15,29,43,5 = straight(5)           
             * 1,15,29,43,54 = straight(5)J         
             * 1,15,29,53,54 = straight(5)JJ        
             *
             * 1,14,27,33,34 = three(A)             
             * 1,14,54,33,34 = three(A)J            
             * 1,54,53,33,34 = three(A)JJ               ○
             * 
             * 1,14,2,15,48 = two(A)                    ○
             * 
             * 1,14,2,48,26 = one(A)                    ○
             * 54,1,13,15,17 = one(A)J                  ○
             * 
             * 37,34,28,1,6 = high(A)                   ○
             */
            showcard(PHands, PHands1.Location.X, PHands1.Location.Y);
            pre = Judge(PHands);
            pra = RankJudge(PHands);
            PResult.Text = pre.ToString() + "(" + pra + ")";
        }
        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            #region ゲームの初期化
            DResult.Text = "";
            PResult.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            Snow = 54;
            Stack = gameinit(Stack);
            #endregion
            switch (gamemode)
            {
                #region gamemodo = MODE_ALONE
                case mode.MODE_ALONE:
                    //手札配布
                    for (int i = 0; i < 5; i++)
                    {
                        Snow--;
                        PHands[i] = Stack[Snow];
                    }
                    break;
                #endregion
                #region gamemode = MODE_VS
                case mode.MODE_VS:
                    //プレイヤーとディーラーに手札配布
                    for (int i = 0; i < 5; i++)
                    {
                        Snow--;
                        PHands[i] = Stack[Snow];
                        DHands[i] = Stack[Snow - 6];
                    }
                    Snow -= 5;
                    break;
                #endregion
                default:
                    break;
            }
            //カード表示
            showcard(PHands, PHands1.Location.X, PHands1.Location.Y);
            //役の判定及び表示
            pre = Judge(PHands);
            pra = RankJudge(PHands);
            PResult.Text = pre.ToString() + "(" + pra + ")";
            btnStart.Enabled = false;
            btnDraw.Enabled = true;
            lblStart.Visible = false;

        }
        #region Hold操作
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            hold(1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            hold(2);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            hold(3);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            hold(4);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            hold(5);
        }
        #endregion
        private void btnDraw_Click(object sender, EventArgs e)
        {
            #region 共通する初期処理
            //手札交換
            for (int i = 0; i < 5; i++)
            {
                if (!PHolds[i])
                {
                    Snow--;
                    PHands[i] = Stack[Snow];
                }
            }
            lblStackCount.Text = "残り" + Snow.ToString() + "枚";
            //カード表示
            showcard(PHands, PHands1.Location.X, PHands1.Location.Y);
            //役の判定及び表示
            pre = Judge(PHands);
            pra = RankJudge(PHands);
            PResult.Text = pre.ToString() + "(" + pra + ")";
            #endregion
            switch (gamemode)
            {
                #region gamemodo = MODE_ALONE
                case mode.MODE_ALONE:
                    //コインの配布
                    MessageBox.Show("コイン" + (((int)Judge(PHands) + 1) * 10).ToString() + "枚獲得！！");
                    coin += ((int)Judge(PHands) + 1) * 10;
                    lblCoin.Text = coin.ToString() + "枚";
                    //終了処理                    
                    btnStart.Enabled = true;
                    PHands1.BringToFront();
                    PHands2.BringToFront();
                    PHands3.BringToFront();
                    PHands4.BringToFront();
                    PHands5.BringToFront();
                    PResult.Text = "";
                    break;
                #endregion
                #region gamemode = MODE_VS
                case mode.MODE_VS:
                    //DEALER交換
                    for (int i = 0; i < 5; i++)
                    { DHolds[i] = false; }
                    for (int i = 0; i < 5; i++)
                    {
                        if (DHands[i] == 53 || DHands[i] == 54)
                        {
                            DHolds[i] = true;//JOKERは交換しない
                        }
                    }
                    switch (Judge(PHands))//交換するものを判定
                    {
                        case result.HighCards://もっと考えた行動を目標に

                            break;
                        case result.OnePair:
                        case result.TwoPair:
                        case result.ThereeCard:
                        case result.FourCard:
                            for (int i = 0; i < 4; i++)
                            {

                                for (int j = i + 1; j < 5; j++)
                                {
                                    if (getsuit(DHands[i]) != suit.SUIT_JOKER || getsuit(DHands[j]) != suit.SUIT_JOKER)
                                    {
                                        if (getrank(DHands[i], 0) == getrank(DHands[j], 0))
                                        {
                                            DHolds[i] = true;
                                            DHolds[j] = true;
                                        }
                                    }
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                if (DHolds[i] == false)
                                {
                                    Snow--;
                                    DHands[i] = Stack[Snow];
                                }
                            }
                            break;
                        default:
                            for (int i = 0; i < 5; i++)
                            { DHolds[i] = true; }
                            break;
                    }
                    //終了処理
                    lblStackCount.Text = "残り" + Snow.ToString() + "枚";
                    btnCall.Enabled = true;
                    btnRaise.Enabled = true;
                    break;
                #endregion
                default:
                    break;
            }
            #region 共通する終了処理
            btnDraw.Enabled = false;
            lblDraw.Visible = false;
            #endregion
        }
        private void btnBet_Click(object sender, EventArgs e)
        {
            //bet額の決定
            bet = (int)numericUpDown1.Value;
            coin -= bet;
            lblCoin.Text = coin.ToString() + "枚";
            label7.Text = "現在" + bet.ToString() + "枚\n賭けられています";
            //終了処理
            numericUpDown1.Enabled = false;
            btnStart.Enabled = true;
            btnBet.Enabled = false;
            lblBet.Visible = false;
            lblBet.Text = "コール？orレイズ？";
        }
        private void btnCall_Click(object sender, EventArgs e)
        {
            CRcomon();
        }
        private void btnRaise_Click(object sender, EventArgs e)
        {
            if (coin > bet)
            {
                bet *= 2;
                coin -= bet;
            }
            else
            {
                MessageBox.Show("コインが足りないので全額賭けます。");
                bet += coin;
                coin = 0;
            }
            label7.Text = "現在" + bet.ToString() + "枚\n賭けられています";
            lblCoin.Text = coin.ToString() + "枚";
            CRcomon();
        }
        private void CRcomon()
        {
            //ディーラーのカード表示
            showcard(DHands, DHands1.Location.X, DHands1.Location.Y);
            //ディーラーの役及び表示
            dre = Judge(DHands);
            dra = RankJudge(DHands);
            DResult.Text = dre.ToString() + "(" + dra + ")";
            //勝ち負け判定及びコイン処理
            #region 勝ち負け処理
            if (pre > dre)//役で勝ち
            {
                MessageBox.Show("You win!!\nコイン" + (bet * 2).ToString() + "枚獲得！！");
                coin += bet * 2;
                lblCoin.Text = coin.ToString() + "枚";
            }
            else if (pre < dre)//役で負け
            {
                MessageBox.Show("You lose...\nコイン" + bet.ToString() + "枚失った...");
            }
            else//役は引き分け
            {
                if (Ranktoint(pra) > Ranktoint(dra))//数字で勝ち
                {
                    MessageBox.Show("You win!!\nコイン" + (bet * 2).ToString() + "枚獲得！！");
                    coin += bet * 2;
                    lblCoin.Text = coin.ToString() + "枚";
                }
                else if (Ranktoint(pra) < Ranktoint(dra))//数字で負け
                {
                    MessageBox.Show("You lose...\nコイン" + bet.ToString() + "枚失った...");
                }
                else//数字でも引き分け
                {
                    MessageBox.Show("Draw\n賭けた分のコイン" + bet.ToString() + "枚獲得");
                    coin += bet;
                    lblCoin.Text = coin.ToString() + "枚";
                }
            }
            #endregion
            //終了処理
            bet = 0;
            btnCall.Enabled = false;
            btnRaise.Enabled = false;
            numericUpDown1.Enabled = true;
            btnBet.Enabled = true;
            lblBet.Text = "ベッティングタイム";
            label7.Text = "現在" + bet.ToString() + "枚\n賭けられています";
            numericUpDown1.Maximum = coin;
            lblCoin.Text = coin.ToString() + "枚";
            PHands1.BringToFront();
            PHands2.BringToFront();
            PHands3.BringToFront();
            PHands4.BringToFront();
            PHands5.BringToFront();
            DHands1.BringToFront();
            DHands2.BringToFront();
            DHands3.BringToFront();
            DHands4.BringToFront();
            DHands5.BringToFront();
            PResult.Text = "";
            DResult.Text = "";
        }
        private void showcard(int[] Hands, int x, int y)
        {
            for (int i = 0; i < 5; i++)//手札画像配置
            {
                string s = suitList[(int)getsuit(Hands[i])];
                string r = getrank(Hands[i], 0);
                //pictureboxをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find(s + r, true);
                //pictureboxが見つかれば、locationを変更する
                if (cs.Length > 0)
                {
                    ((PictureBox)cs[0]).Location = new Point(i * 77 + x, y);
                    ((PictureBox)cs[0]).Visible = true;
                    ((PictureBox)cs[0]).BringToFront();
                }
            }
        }
        private void hold(int i)
        {
            //チェックボックスをさがす。子コントロールも検索する。
            Control[] cb = this.Controls.Find("checkBox" + (i).ToString(), true);
            Control[] lb = this.Controls.Find("hold" + (i).ToString(), true);
            //チェックボックスが見つかれば、処理を開始
            if (cb.Length > 0 && lb.Length > 0)
            {
                if (((CheckBox)cb[0]).Checked)
                {
                    ((Label)lb[0]).Visible = true;
                    ((Label)lb[0]).BringToFront();
                }
                else
                {
                    ((Label)lb[0]).Visible = false;
                }
                PHolds[i - 1] = ((Label)lb[0]).Visible;


            }
        }
        private suit getsuit(int card)
        {
            suit s = new suit();
            int d;
            if (card - 1 < 0)
            {
                d = 0;
            }
            else
            {
                d = (card - 1) / 13;
            }
            switch (d)
            {
                case 0:
                    s = suit.SUIT_HEART;
                    break;
                case 1:
                    s = suit.SUIT_DIA;
                    break;
                case 2:
                    s = suit.SUIT_SPADE;
                    break;
                case 3:
                    s = suit.SUIT_CLOVER;
                    break;
                case 4:
                    s = suit.SUIT_JOKER;
                    break;
                default:
                    break;
            }
            return s;
        }
        private string getrank(int card, int type)//type=0 数字　type=1 英数字
        {
            string rank;
            if (type == 0)
            {
                rank = (card % 13).ToString();
                if (rank == "0") rank = "13";
            }
            else
            {
                switch (card % 13)
                {
                    case 1:
                        rank = "A";
                        break;
                    case 11:
                        rank = "J";
                        break;
                    case 12:
                        rank = "Q";
                        break;
                    case 0:
                        rank = "K";
                        break;
                    default:
                        rank = (card % 13).ToString();
                        break;
                }
            }
            return rank;
        }
        private int[] gameinit(int[] Stack)
        {
            //新しいカードを持ってくる
            for (int i = 0; i < 54; i++)
            {
                Stack[i] = i + 1;
            }
            int n = Stack.Length;
            Random rnd = new Random();
            //シャッフル
            while (n > 1)//Fisher-Yates法
            {
                n--;
                int k = rnd.Next(n + 1);
                int tmp = Stack[k];
                Stack[k] = Stack[n];
                Stack[n] = tmp;
            }
            return Stack;
        }
        private result Judge(int[] Hands)
        {
            #region 宣言
            result h = new result();
            int[] js = new int[5];//judge用マーク1=h 2=d 3=s 4=c 5=j
            int[] jr = new int[13];//judge用
            int pairFlag;//ペアの数
            bool threecardFlag;//スリーカード
            bool fourcardFlag;//フォーカード
            bool fivecardFlag;//ファイブカード
            bool flushFlag;//フラッシュ
            bool straightFlag;//ストレート
            bool royalFlag;//ロイヤル
            #endregion
            #region 初期化
            h = result.HighCards;
            for (int i = 0; i < 5; i++)
            { js[i] = 0; }
            for (int i = 0; i < 13; i++)
            { jr[i] = 0; }
            pairFlag = 0;//ペアの数
            threecardFlag = false;//スリーカード
            fourcardFlag = false;//フォーカード
            fivecardFlag = false;
            flushFlag = false;//フラッシュ
            straightFlag = false;//ストレート
            royalFlag = false;//ロイヤル
            #endregion
            //ハンドの内容を読み取り
            for (int i = 0; i < 5; i++)
            {
                js[(int)getsuit(Hands[i])]++;
                if (Hands[i] != 53 && Hands[i] != 54)
                {
                    jr[int.Parse(getrank(Hands[i], 0)) - 1]++;
                }
            }
            #region フラグ立て
            for (int i = 0; i < 4; i++)//マーク
            {
                switch (js[i])
                {
                    case 5:
                        flushFlag = true;
                        break;
                    case 4:
                        if (js[4] == 1) flushFlag = true;
                        break;
                    case 3:
                        if (js[4] == 2) flushFlag = true;
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < 13; i++)//ペアスリーフォーファイブ
            {
                switch (jr[i])
                {
                    case 4:
                        if (js[4] == 1) fivecardFlag = true;
                        else fourcardFlag = true;
                        break;
                    case 3:
                        switch (js[4])
                        {
                            case 2:
                                fivecardFlag = true;
                                break;
                            case 1:
                                fourcardFlag = true;
                                break;
                            default:
                                threecardFlag = true;
                                break;
                        }
                        break;
                    case 2:
                        switch (js[4])
                        {
                            case 2:
                                fourcardFlag = true;
                                break;
                            case 1:
                                if (threecardFlag) pairFlag++;
                                else threecardFlag = true;
                                break;
                            default:
                                pairFlag++;
                                break;
                        }
                        break;
                    default:
                        break;
                }

            }
            int sc;//ストレートカウンター
            for (int i = 1; i < 11; i++)//ストレートロイヤル
            {
                sc = 0;
                for (int j = 0; j < 5; j++)
                {
                    if (jr[Ranktoint(((i + j - 1) % 13).ToString())] == 1) sc++;
                }
                if (sc + js[4] == 5)
                {
                    straightFlag = true;
                    if (i == 10)
                    {
                        royalFlag = true;
                    }
                }

            }
            #endregion
            #region result取得
            if (fivecardFlag) h = result.FiveCard;
            if (flushFlag && royalFlag) h = result.RoyalStraightFlush;
            if (flushFlag && straightFlag && !royalFlag) h = result.StraightFlush;
            if (fourcardFlag) h = result.FourCard;
            if (pairFlag == 1 && threecardFlag) h = result.FullHouse;
            if (flushFlag)
            {
                if ((int)h < (int)result.Flush) h = result.Flush;
            }
            if (straightFlag)
            {
                if ((int)h < (int)result.Straight) h = result.Straight;
            }
            if (threecardFlag)
            {
                if ((int)h < (int)result.ThereeCard) h = result.ThereeCard;
            }
            if (pairFlag == 2) h = result.TwoPair;
            if (pairFlag == 1)
            {
                if ((int)h < (int)result.OnePair) h = result.OnePair;
            }
            if (h == result.HighCards)
            {
                switch (js[4])
                {
                    case 2:
                        h = result.ThereeCard;
                        break;
                    case 1:
                        h = result.OnePair;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            return h;
        }
        private string RankJudge(int[] Hands)
        {
            result r = Judge(Hands);
            string s = null;
            int i, j, k, joker, min, max;
            switch (r)
            {
                #region fivecard
                case result.FiveCard:
                    i = 0;
                    while (getsuit(Hands[i]) == suit.SUIT_JOKER)
                    {
                        s = getrank(Hands[i], 1);
                        i++;
                    }
                    break;
                #endregion
                #region RoyalstraightFlush
                case result.RoyalStraightFlush:
                    s = "A";
                    break;
                #endregion
                #region StraightFlush & Straight
                case result.StraightFlush:
                case result.Straight:
                    min = 99;
                    bool Aflag = false; ;
                    for (i = 0; i < 5; i++)//12345のときAと表示されてしまう
                    {
                        if (getsuit(Hands[i]) == suit.SUIT_JOKER) continue;
                        if (Ranktoint(getrank(Hands[i], 0)) == 1) Aflag = true;
                        if (Ranktoint(getrank(Hands[i], 1)) < min)
                        {
                            min = Ranktoint(getrank(Hands[i], 1));

                        }
                    }
                    s = getrank(min + 4, 1);
                    if (s == "6" && Aflag) s = "5";
                    break;
                #endregion
                #region fourcard
                case result.FourCard:
                    for (i = 0; i < 4; i++)
                    {
                        for (j = i + 1; j < 5; j++)
                        {
                            if (getrank(Hands[i], 0) == getrank(Hands[j], 0))
                            {
                                s = getrank(Hands[i], 1);
                                break;
                            }
                            if (s != null) break;
                        }
                    }
                    break;
                #endregion
                #region fullhouse
                case result.FullHouse:
                    joker = 0;
                    max = 0;
                    for (i = 0; i < 5; i++)
                    {
                        if (Hands[i] == 53 || Hands[i] == 54)
                        {
                            joker++;
                            break;
                        }

                    }
                    if (joker == 1)
                    {
                        for (i = 0; i < 4; i++)
                        {
                            for (j = i + 1; j < 5; j++)
                            {
                                if (getrank(Hands[i], 0) == getrank(Hands[j], 0) && max < Ranktoint(getrank(Hands[i], 1)))
                                {
                                    max = Ranktoint(getrank(Hands[i], 1));
                                    s = getrank(Hands[i], 1);
                                    break;
                                }
                            }
                        }
                    }
                    else//ばぐ
                    {
                        for (i = 0; i < 3; i++)
                        {
                            for (j = i + 1; j < 4; j++)
                            {
                                for (k = j + 1; k < 5; k++)
                                {
                                    if (getrank(Hands[i], 0) == getrank(Hands[j], 0) && getrank(Hands[j], 0) == getrank(Hands[k], 0))
                                    {
                                        s = getrank(Hands[i], 1);
                                    }

                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region Flush & highcard
                case result.Flush:
                case result.HighCards:
                    max = 0;
                    for (i = 0; i < 5; i++)
                    {
                        if (getsuit(Hands[i]) == suit.SUIT_JOKER)
                        {
                            s = "A";
                            break;
                        }
                        if (Ranktoint(getrank(Hands[i], 1)) > max)
                        {
                            max = Ranktoint(getrank(Hands[i], 1));
                            s = getrank(Hands[i], 1);
                        }
                    }
                    break;
                #endregion
                #region threecard
                case result.ThereeCard:
                    joker = 0;
                    for (i = 0; i < 5; i++)
                    {
                        if (PHands[i] == 53 || PHands[i] == 54)
                        {
                            joker++;
                        }
                    }
                    switch (joker)
                    {
                        case 2:
                            max = 0;
                            for (i = 0; i < 5; i++)
                            {
                                if (getsuit(Hands[i]) == suit.SUIT_JOKER)
                                {
                                    continue;
                                }
                                if (Ranktoint(getrank(Hands[i], 1)) > max)
                                {
                                    max = Ranktoint(getrank(Hands[i], 1));
                                    s = getrank(Hands[i], 1);
                                }
                            }
                            break;
                        case 1:
                            for (i = 0; i < 4; i++)
                            {
                                for (j = i + 1; j < 5; j++)
                                {
                                    if (getrank(Hands[i], 0) == getrank(Hands[j], 0))
                                    {
                                        s = getrank(Hands[i], 1);
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            for (i = 0; i < 4; i++)
                            {
                                for (j = i + 1; j < 5; j++)
                                {
                                    if (getrank(Hands[i], 0) == getrank(Hands[j], 0))
                                    {
                                        max = Ranktoint(getrank(Hands[i], 1));
                                        s = getrank(Hands[i], 1);
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                    break;
                #endregion
                #region twopair
                case result.TwoPair:
                    max = 0;
                    for (i = 0; i < 4; i++)
                    {
                        for (j = i + 1; j < 5; j++)
                        {
                            if (getrank(Hands[i], 0) == getrank(Hands[j], 0) && max < Ranktoint(getrank(Hands[i], 1)))
                            {
                                max = Ranktoint(getrank(Hands[i], 1));
                                s = getrank(Hands[i], 1);
                                break;
                            }
                        }
                    }
                    break;
                #endregion
                #region onepair
                case result.OnePair:
                    joker = 0;
                    for (i = 0; i < 5; i++)
                    {
                        if (PHands[i] == 53 || PHands[i] == 54)
                        {
                            joker++;
                        }
                    }
                    if (joker == 1)
                    {
                        max = 0;
                        for (i = 0; i < 5; i++)
                        {
                            if (getsuit(Hands[i]) == suit.SUIT_JOKER)
                            {
                                continue;
                            }
                            if (Ranktoint(getrank(Hands[i], 1)) > max)
                            {
                                max = Ranktoint(getrank(Hands[i], 1));
                                s = getrank(Hands[i], 1);
                            }
                        }
                        break;
                    }
                    for (i = 0; i < 4; i++)
                    {
                        for (j = i + 1; j < 5; j++)
                        {
                            if (getrank(Hands[i], 0) == getrank(Hands[j], 0))
                            {
                                max = Ranktoint(getrank(Hands[i], 1));
                                s = getrank(Hands[i], 1);
                                break;
                            }
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
            return s;
        }
        private int Ranktoint(string s)
        {
            int i;
            switch (s)
            {
                case "A":
                    i = 14;
                    break;
                case "K":
                    i = 13;
                    break;
                case "Q":
                    i = 12;
                    break;
                case "J":
                    i = 11;
                    break;
                default:
                    i = int.Parse(s);
                    break;
            }
            return i;
        }
        private void timer1_Tick(object sender, EventArgs e)//点滅する指示の処理
        {
            if (btnStart.Enabled)
            {
                if (lblStart.Visible)
                {
                    lblStart.Visible = false;
                }
                else
                {
                    lblStart.Visible = true;
                }
            }
            else if (btnDraw.Enabled)
            {
                if (lblDraw.Visible)
                {
                    lblDraw.Visible = false;
                }
                else
                {
                    lblDraw.Visible = true;
                }
            }
            else
            {
                if (lblBet.Visible)
                {
                    lblBet.Visible = false;
                }
                else
                {
                    lblBet.Visible = true;
                }
            }
        }
#endregion
        private void ログインLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            csvinit();
            login _login = new login();
            _login.nid = number;
            _login.ShowDialog(this);       
            number = _login.nid;
            readIDlist();
            _login.Dispose();
        }

        private void main_FormClosed(object sender, FormClosedEventArgs e)
        {
            csvinit();
        }
        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (gamemode == mode.MODE_SELECT)
            {
                DialogResult dResult;

                dResult = MessageBox.Show("終了してよろしいですか？", "終了確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                Environment.Exit(0);//プログラムの終了
            }
            else
            {
                新規ゲームNToolStripMenuItem.Enabled = false;
                csvinit();
                forminit();
                e.Cancel = true;
                return;
            }
        }
        private void csvinit()//csvファイルの保存
        {
            //CSV処理
            IDlist.Rows[0][0] = "Guest";
            IDlist.Rows[0][1] = "";
            IDlist.Rows[0][2] = _encrypt.EncryptString("1000", IDlist.Rows[0][0].ToString());
            IDlist.Rows[number][2] = _encrypt.EncryptString(coin.ToString(), IDlist.Rows[number][0].ToString());
            _csv.ConvertDataTableToCsv(IDlist, "coin.csv", false);

        }
        private void readIDlist()//CSVファイルの読み込み
        {
            IDlist.Columns.Clear();
            IDlist.Clear();
            _csv.ReadCSV(IDlist, false, "coin.csv", ",", false);
            ID = IDlist.Rows[number][0].ToString();
            coin = int.Parse(_encrypt.DecryptString(IDlist.Rows[number][2].ToString(), IDlist.Rows[number][0].ToString()));
            lblID.Text = "ようこそ" + IDlist.Rows[number][0].ToString() + "様";
            lblCoin.Text = _encrypt.DecryptString(IDlist.Rows[number][2].ToString(), IDlist.Rows[number][0].ToString()) + "枚";
        }

        private void ハイスコアHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDlist.Rows[number][2] = _encrypt.EncryptString(coin.ToString(), IDlist.Rows[number][0].ToString());
            int mcoin = 0;
            int m = 0;
            for (int i = 1; i < IDlist.Rows.Count; i++)
            {
                int c = int.Parse(_encrypt.DecryptString(IDlist.Rows[i][2].ToString(), IDlist.Rows[i][0].ToString()));
                if (mcoin < c)
                {
                    mcoin = c;
                    m = i;
                }
            }
            MessageBox.Show("ハイスコアは" + IDlist.Rows[m][0].ToString() + "様のコイン" + mcoin.ToString() + "枚です。");

        }
    }
}
