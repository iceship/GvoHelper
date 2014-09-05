using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

namespace GvoHelper
{
    public partial class Form5 : Form
    {
        private GVOCall Call = new GVOCall();
        private ResourceManager skill = new ResourceManager("GvoHelper.Properties.Resources", Assembly.GetExecutingAssembly());

        private Button Attack = new Button();

        private GroupBox EnemyBox;
        private Label EnemyLV, EnemyAttack, EnemyDefense;
        private VistaStyleProgressBar.ProgressBar EnemyHP;

        private GroupBox FriendBox;
        private VistaStyleProgressBar.ProgressBar FriendHP, FriendMotion;

        private int AttackTargetID;

        public Form5()
        {
            InitializeComponent();
        }

        //限制只能輸入數字&空格則為0
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void Enemy_Click(object sender, EventArgs e)
        {
            //點選敵方目標用
            //if (((GroupBox)sender).BackColor != Color.Orange)
                //((GroupBox)sender).BackColor = Color.Orange;
            //TargetID = Enemy[Convert.ToInt32(((GroupBox)sender).Name.Replace("Enemy", ""))].ID;
        }

        private void Shortcuts_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int Shortcuts = Convert.ToInt32((button.Name.Substring(button.Name.IndexOf("F") + 1, 1)));
            int who = Convert.ToInt32((button.Name.Substring(button.Name.IndexOf("_") + 1)));

            for (int User = 0; User < 5; User++)
            {
                if (Form1.UsingProcess[User].Friend[who].ID == Form1.UsingProcess[User].ID)
                {
                    if (Shortcuts >= 5)
                        Call.UseLBItem(Form1.UsingProcess[User].hWnd, Call.GetLBTarget(Form1.UsingProcess[User].hWnd), Shortcuts - 5);
                    else
                        Call.UseLBSkill(Form1.UsingProcess[User].hWnd, Call.GetLBAttackTarget(Form1.UsingProcess[User].hWnd), Shortcuts - 1);
                }
            }

        }

        private void Attack_Click(object sender, MouseEventArgs e)
        {
            int User;
            for (User = 0; User < 5; User++)
                if (Form1.UsingProcess[User].IsLandBattle)
                    break;
            
            if (e.Button == MouseButtons.Left)
            {
                int _enemy = Convert.ToInt32(((Button)sender).Name.Replace("Attack", ""));
                AttackTargetID = Form1.UsingProcess[User].Enemy[_enemy].ID;
                for (User = 0; User < 5; User++)
                {
                    if (Form1.UsingProcess[User].IsLandBattle)
                    {
                        Call.LBAttack(Form1.UsingProcess[User].hWnd, AttackTargetID);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                AutoSelectTarget.Checked = false;
                AttackTargetID = 0;
            }
        }

        private void ReadyButton_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (Form1.UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.Ready(Form1.UsingProcess[User].hWnd);
        }

        private void PassButton_Click(object sender, EventArgs e)
        {
            for (int User = 0; User < 5; User++)
                if (Form1.UsingProcess[User].hWnd != IntPtr.Zero)
                    Call.PassPlay(Form1.UsingProcess[User].hWnd);
        }

        private void timer(int User)
        {
            #region timer初始化
            Timer timer;
            Button[] Shortcuts = new Button[7];

            switch (User)
            {
                case 0:
                    timer = timer0;
                    break;
                case 1:
                    timer = timer1;
                    break;
                case 2:
                    timer = timer2;
                    break;
                case 3:
                    timer = timer3;
                    break;
                case 4:
                    timer = timer4;
                    break;
                default:
                    return;
            }
            #endregion

            if (Form1.UsingProcess[User].hWnd != IntPtr.Zero && Call.GetConnectState(Form1.UsingProcess[User].hWnd) && Call.GetUserId(Form1.UsingProcess[User].hWnd) != 0)
            {
                timer.Stop();

                if (Call.GetBattleStatus(Form1.UsingProcess[User].hWnd) >= 5 && Call.GetBattleStatus(Form1.UsingProcess[User].hWnd) <= 6)
                {
                    Form1.UsingProcess[User].IsLandBattle = true;

                    Form1.UsingProcess[User].ArenaGetMission = false;
                    Form1.UsingProcess[User].ArenaReadyMission = false;
                    Form1.UsingProcess[User].ArenaIdleCount = 0;
                    Form1.UsingProcess[User].ArenaPassCount = 0;

                    Form1.UsingProcess[User].DungeonSell = false;
                    Form1.UsingProcess[User].DungeonIdleCount = 0;
                    Form1.UsingProcess[User].DungeonClearCount = 0;

                    Call.GetLBInfo(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Enemy, Form1.UsingProcess[User].Friend);//取得陸戰資料
                }
                else
                {
                    Form1.UsingProcess[User].IsLandBattle = false;

                    Form1.UsingProcess[User].LBCureCount = 0;
                    Form1.UsingProcess[User].LBAttackCount = 0;
                    Form1.UsingProcess[User].LBAttackIdleCount = 0;
                    Form1.UsingProcess[User].LBSkillCount = 0;

                    if (Form1.UsingProcess[User].User_Type == "猜猜看")
                    {
                        #region 陸戰-援軍
                        for (int i = 0; i < 5; i++)
                        {
                            if (Form1.UsingProcess[User].PlaceName != "圓形競技場" && Form1.UsingProcess[User].PartyID[i] != Form1.UsingProcess[User].ID && Call.GetPartyBattleStatus(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].PartyID[i]) >= 5)
                            {
                                if (Form1.UsingProcess[User].TimeCount > 3)
                                {
                                    Call.JoinLandBattle(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].PartyID[i]);
                                    Form1.UsingProcess[User].TimeCount = 0;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region 競技場
                        if (ArenaMode.Checked)
                        {
                            if (Form1.UsingProcess[User].Place == 0xC)
                            {
                                //判斷競技場發呆
                                ++Form1.UsingProcess[User].ArenaIdleCount;
                                if (Form1.UsingProcess[User].ArenaIdleCount > 50)
                                {
                                    Form1.UsingProcess[User].ArenaGetMission = false;
                                    Form1.UsingProcess[User].ArenaReadyMission = false;
                                    Form1.UsingProcess[User].ArenaIdleCount = 0;
                                    Form1.UsingProcess[User].ArenaPassCount = 0;
                                }
                                for (int i = 0; i < 5; i++)
                                {
                                    //檢查提督是否已接任命&完成戰鬥準備
                                    if (Form1.UsingProcess[i].ID == Form1.UsingProcess[i].PartyID[0] && Form1.UsingProcess[i].ArenaGetMission && !Form1.UsingProcess[User].ArenaReadyMission)
                                    {
                                        if (Form1.UsingProcess[User].ID == Form1.UsingProcess[User].PartyID[0] || Form1.UsingProcess[i].ArenaReadyMission)
                                        {
                                            Call.Ready(Form1.UsingProcess[User].hWnd);
                                            Form1.UsingProcess[User].ArenaReadyMission = true;
                                            Console.WriteLine(Form1.UsingProcess[User].Name + " 完成戰鬥準備。");
                                        }
                                    }
                                }
                                //檢查提督接任命
                                if (Form1.UsingProcess[User].ArenaIdleCount > 5 && Form1.UsingProcess[User].ID == Form1.UsingProcess[User].PartyID[0] && !Form1.UsingProcess[User].ArenaGetMission)
                                {
                                    bool CheckPlace = true;
                                    for (int Party = 0; Party < 5; Party++)
                                        if (Call.CheckPartyID(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[Party].ID) && Form1.UsingProcess[Party].Place != 0xC)
                                            CheckPlace = false;
                                    if (CheckPlace)
                                    {
                                        Call.GetArenaMission(Form1.UsingProcess[User].hWnd);
                                        Form1.UsingProcess[User].ArenaGetMission = true;
                                        Console.WriteLine(Form1.UsingProcess[User].Name + " 接取任命。");
                                    }
                                }
                            }
                            else
                            {
                                if (Form1.UsingProcess[User].ArenaReadyMission && !Call.CheckPassPlay(Form1.UsingProcess[User].hWnd))
                                {
                                    ++Form1.UsingProcess[User].ArenaPassCount;
                                    if (Form1.UsingProcess[User].ArenaPassCount > 3)
                                    {
                                        Call.PassPlay(Form1.UsingProcess[User].hWnd);
                                        Form1.UsingProcess[User].ArenaPassCount = 0;
                                        Console.WriteLine(Form1.UsingProcess[User].Name + " 跳過演出。");
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 地下城
                        if (DungeonMode.Checked)
                        {
                            if (Call.InDungeon(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Place))
                            {
                                if (Form1.UsingProcess[User].ID == Call.GetDungeonAdmiral(Form1.UsingProcess[User].hWnd))
                                {
                                    bool NpcClear = true;

                                    #region 開戰
                                    for (int i = 0; i < Form1.UsingProcess[User]._People.Length && NpcClear; i++)
                                    {
                                        if (!Call.CheckPartyID(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID) && Form1.UsingProcess[User]._People[i].Name != "？？？" && Form1.UsingProcess[User]._People[i].Name != "！？" && Form1.UsingProcess[User]._People[i].Name != "寶箱")
                                        {
                                            if (Form1.UsingProcess[User]._People[i].ID != Form1.UsingProcess[User].ID)
                                            {
                                                NpcClear = false;
                                                if (!string.IsNullOrWhiteSpace(Form1.UsingProcess[User]._People[i].Name))
                                                {
                                                    if (Form1.UsingProcess[User].TimeCount > 3)
                                                    {
                                                        Call.StartLandBattle(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID);
                                                        Form1.UsingProcess[User].TimeCount = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (NpcClear)
                                    {
                                        ++Form1.UsingProcess[User].DungeonClearCount;
                                        if (Form1.UsingProcess[User].DungeonClearCount == 1)
                                        {
                                            #region 開寶箱
                                            for (int i = 0; i < Form1.UsingProcess[User]._People.Length; i++)
                                            {
                                                if (Form1.UsingProcess[User]._People[i].Name == "寶箱")
                                                {
                                                    Call.Talk(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID);
                                                    //Call.Delay(750);
                                                    //if (PeopleNum != Call.GetAllPeopleNum(Form1.UsingProcess[User].hWnd))
                                                    //break;
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (Form1.UsingProcess[User].DungeonClearCount > 4)
                                        {
                                            #region  出口
                                            if (Call.GetExitDungeonCode(Form1.UsingProcess[User].hWnd) != 0)
                                            {
                                                Call.ExitDungeon(Form1.UsingProcess[User].hWnd);
                                                Form1.UsingProcess[User].DungeonClearCount = 0;
                                            }
                                            else
                                            {
                                                for (int i = 0; i < Form1.UsingProcess[User]._People.Length && NpcClear; i++)
                                                {
                                                    if (Form1.UsingProcess[User]._People[i].Name == "？？？")
                                                    {
                                                        Call.Talk(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID);
                                                        //Console.WriteLine("點選出口:" + Form1.UsingProcess[User]._People[i].Name);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                        Form1.UsingProcess[User].DungeonClearCount = 0;
                                }
                            }
                            else if (!Call.GetPartyStatus(Form1.UsingProcess[User].hWnd))
                            {
                                if (Form1.UsingProcess[User].Place != 0x8)
                                {

                                }
                            }
                            else
                            {
                                int IntoCount = 0;
                                if (!string.IsNullOrWhiteSpace(IntoBox.Text))
                                    IntoCount = Convert.ToInt32(IntoBox.Text);

                                if (Form1.UsingProcess[User].DungeonIntoCount >= IntoCount)
                                {
                                    if (CheckSellBox.Checked && !Form1.UsingProcess[User].DungeonSell)
                                    {
                                        if (new CityCall().ToTrader(User) && new CityCall().SellTrade(User, "全部"))
                                            Form1.UsingProcess[User].DungeonSell = true;
                                    }
                                    else if (Form1.UsingProcess[User].Money >= 1000)
                                        new CityCall().ToBank(User, 0);
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(research_comboBox.Text))
                                        {
                                            if (new CityCall().Research(User, research_comboBox.Text))
                                                Form1.UsingProcess[User].DungeonIntoCount = 0;
                                        }
                                        else
                                            Form1.UsingProcess[User].DungeonIntoCount = 0;
                                    }
                                }
                                else if (Form1.UsingProcess[User].CityNo != -1 && !string.IsNullOrWhiteSpace(GVOCall.City[Form1.UsingProcess[User].CityNo].Name))
                                {
                                    if ((GVOCall.City[Form1.UsingProcess[User].CityNo].Name == dungeonComboBox.Text && new CityCall().FindScene(User, "教會")) || Form1.UsingProcess[User].Place == 0x1C)
                                    {
                                        #region 進入地下城
                                        if (Call.GetPartyStatus(Form1.UsingProcess[User].hWnd))
                                        {
                                            if (Form1.UsingProcess[User].DungeonIdleCount == 0)
                                            {
                                                if (Call.CheckWindows(Form1.UsingProcess[User].hWnd) == "持有物品")
                                                {
                                                    if (Call.GetItemsInfo(Form1.UsingProcess[User].hWnd, ref Form1.UsingProcess[User]._Item._HaveItemInfo))
                                                    {
                                                        Call.Equip(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Item._HaveItemInfo);
                                                        Call.DropItem(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._Item._HaveItemInfo);
                                                        ++Form1.UsingProcess[User].DungeonIdleCount;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Form1.UsingProcess[User].TimeCount > 3)
                                                    {
                                                        Call.InfoButton(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, "持有物品");
                                                        Form1.UsingProcess[User].TimeCount = 0;
                                                    }
                                                }
                                            }
                                            else if (Call.CheckPartyPlace(Form1.UsingProcess[User].hWnd, 0xC) || Call.CheckPartyPlace(Form1.UsingProcess[User].hWnd, 0x1C))
                                            {
                                                if (Form1.UsingProcess[User].ID == Form1.UsingProcess[User].PartyID[0])
                                                {
                                                    ++Form1.UsingProcess[User].DungeonIdleCount;
                                                    if (Form1.UsingProcess[User].DungeonIdleCount > 10 && Form1.UsingProcess[User].TimeCount > 3)
                                                    {
                                                        if (!haveIntoDungeonItemNum(User) && Form1.UsingProcess[User].DungeonIdleCount > 20)
                                                        {
                                                            Form1.UsingProcess[User].DungeonIdleCount = 0;
                                                            for (int party = 0; party < 5; party++)
                                                                if (Form1.UsingProcess[User].PartyID[party] != 0 && Form1.UsingProcess[User].PartyID[party] != Form1.UsingProcess[User].ID)
                                                                {
                                                                    Call.ChangeAdmiral(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].PartyID[party]);
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                Form1.UsingProcess[User].DungeonIdleCount = 1;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(dungeonComboBox.Text) && GVOCall.City[Form1.UsingProcess[User].CityNo].Name != dungeonComboBox.Text)
                                            new CityCall().ToPier(User);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                
                //陸戰血條&快捷順序
                for (int i = 0; i < 5; i++)
                {
                    #region 初始化元件
                    switch (i)
                    {
                        case 0:
                            Attack = Attack0;

                            EnemyBox = Enemy0;
                            EnemyLV = EnemyLV0;
                            EnemyAttack = EnemyAttack0;
                            EnemyDefense = EnemyDefense0;
                            EnemyHP = EnemyHP0;

                            FriendBox = Friend0;
                            FriendHP = FriendHP0;
                            FriendMotion = FriendMotion0;
                            //技巧
                            Shortcuts[0] = F1_0;
                            Shortcuts[1] = F2_0;
                            Shortcuts[2] = F3_0;
                            //道具
                            Shortcuts[3] = F5_0;
                            Shortcuts[4] = F6_0;
                            Shortcuts[5] = F7_0;
                            Shortcuts[6] = F8_0;
                            break;
                        case 1:
                            Attack = Attack1;

                            EnemyBox = Enemy1;
                            EnemyLV = EnemyLV1;
                            EnemyAttack = EnemyAttack1;
                            EnemyDefense = EnemyDefense1;
                            EnemyHP = EnemyHP1;

                            FriendBox = Friend1;
                            FriendHP = FriendHP1;
                            FriendMotion = FriendMotion1;
                            //技巧
                            Shortcuts[0] = F1_1;
                            Shortcuts[1] = F2_1;
                            Shortcuts[2] = F3_1;
                            //道具
                            Shortcuts[3] = F5_1;
                            Shortcuts[4] = F6_1;
                            Shortcuts[5] = F7_1;
                            Shortcuts[6] = F8_1;
                            break;
                        case 2:
                            Attack = Attack2;

                            EnemyBox = Enemy2;
                            EnemyLV = EnemyLV2;
                            EnemyAttack = EnemyAttack2;
                            EnemyDefense = EnemyDefense2;
                            EnemyHP = EnemyHP2;

                            FriendBox = Friend2;
                            FriendHP = FriendHP2;
                            FriendMotion = FriendMotion2;
                            //技巧
                            Shortcuts[0] = F1_2;
                            Shortcuts[1] = F2_2;
                            Shortcuts[2] = F3_2;
                            //道具
                            Shortcuts[3] = F5_2;
                            Shortcuts[4] = F6_2;
                            Shortcuts[5] = F7_2;
                            Shortcuts[6] = F8_2;
                            break;
                        case 3:
                            Attack = Attack3;

                            EnemyBox = Enemy3;
                            EnemyLV = EnemyLV3;
                            EnemyAttack = EnemyAttack3;
                            EnemyDefense = EnemyDefense3;
                            EnemyHP = EnemyHP3;

                            FriendBox = Friend3;
                            FriendHP = FriendHP3;
                            FriendMotion = FriendMotion3;
                            //技巧
                            Shortcuts[0] = F1_3;
                            Shortcuts[1] = F2_3;
                            Shortcuts[2] = F3_3;
                            //道具
                            Shortcuts[3] = F5_3;
                            Shortcuts[4] = F6_3;
                            Shortcuts[5] = F7_3;
                            Shortcuts[6] = F8_3;
                            break;
                        case 4:
                            Attack = Attack4;

                            EnemyBox = Enemy4;
                            EnemyLV = EnemyLV4;
                            EnemyAttack = EnemyAttack4;
                            EnemyDefense = EnemyDefense4;
                            EnemyHP = EnemyHP4;

                            FriendBox = Friend4;
                            FriendHP = FriendHP4;
                            FriendMotion = FriendMotion4;
                            //技巧
                            Shortcuts[0] = F1_4;
                            Shortcuts[1] = F2_4;
                            Shortcuts[2] = F3_4;
                            //道具
                            Shortcuts[3] = F5_4;
                            Shortcuts[4] = F6_4;
                            Shortcuts[5] = F7_4;
                            Shortcuts[6] = F8_4;
                            break;
                    }
                    #endregion

                    if (Form1.UsingProcess[User].ID == Call.GetDungeonAdmiral(Form1.UsingProcess[User].hWnd))
                    {
                        #region 敵方資料
                        if (Form1.UsingProcess[User].IsLandBattle && Form1.UsingProcess[User].Enemy[i].HP > 0)
                        {
                            if (!EnemyBox.Visible)
                                EnemyBox.Visible = true;
                            if (EnemyBox.Text != Form1.UsingProcess[User].Enemy[i].Name)
                                EnemyBox.Text = Form1.UsingProcess[User].Enemy[i].Name;
                            if (EnemyLV.Text != "等級：" + Form1.UsingProcess[User].Enemy[i].LV.ToString())
                                EnemyLV.Text = "等級：" + Form1.UsingProcess[User].Enemy[i].LV.ToString();
                            if (EnemyAttack.Text != "攻擊力：" + Form1.UsingProcess[User].Enemy[i].Attack.ToString())
                                EnemyAttack.Text = "攻擊力：" + Form1.UsingProcess[User].Enemy[i].Attack.ToString();
                            if (EnemyDefense.Text != "防禦力：" + Form1.UsingProcess[User].Enemy[i].Defense.ToString())
                                EnemyDefense.Text = "防禦力：" + Form1.UsingProcess[User].Enemy[i].Defense.ToString();

                            if (EnemyHP.Value != Form1.UsingProcess[User].Enemy[i].HP)
                                EnemyHP.Value = Form1.UsingProcess[User].Enemy[i].HP;

                            if (Form1.UsingProcess[User].Enemy[i].MaxHP != 0 && EnemyHP.MaxValue != Form1.UsingProcess[User].Enemy[i].MaxHP)
                                EnemyHP.MaxValue = Form1.UsingProcess[User].Enemy[i].MaxHP;

                            if (Form1.UsingProcess[User].Enemy[i].ID == AttackTargetID && Attack.BackColor != Color.Red)
                                Attack.BackColor = Color.Red;
                            if (Form1.UsingProcess[User].Enemy[i].ID != AttackTargetID && Attack.BackColor != Color.Transparent)
                                Attack.BackColor = Color.Transparent;
                            //if (Form1.UsingProcess[User].Enemy[i].ID != TargetID && EnemyBox.BackColor != Color.Transparent)
                            //EnemyBox.BackColor = Color.Transparent;
                            if (Form1.UsingProcess[User].Enemy[i].State == 9 && EnemyAttack.ForeColor != Color.Red)
                                EnemyAttack.ForeColor = Color.Red;
                            if (Form1.UsingProcess[User].Enemy[i].State != 9 && EnemyAttack.ForeColor != Color.Black)
                                EnemyAttack.ForeColor = Color.Black;
                        }
                        else if (Form1.UsingProcess[User].IsLandBattle && Form1.UsingProcess[User].Enemy[i + 5].HP > 0)
                        {
                            if (!EnemyBox.Visible)
                                EnemyBox.Visible = true;
                            if (EnemyBox.Text != Form1.UsingProcess[User].Enemy[i + 5].Name)
                                EnemyBox.Text = Form1.UsingProcess[User].Enemy[i + 5].Name;
                            if (EnemyLV.Text != "等級：" + Form1.UsingProcess[User].Enemy[i + 5].LV.ToString())
                                EnemyLV.Text = "等級：" + Form1.UsingProcess[User].Enemy[i + 5].LV.ToString();
                            if (EnemyAttack.Text != "攻擊力：" + Form1.UsingProcess[User].Enemy[i + 5].Attack.ToString())
                                EnemyAttack.Text = "攻擊力：" + Form1.UsingProcess[User].Enemy[i + 5].Attack.ToString();
                            if (EnemyDefense.Text != "防禦力：" + Form1.UsingProcess[User].Enemy[i + 5].Defense.ToString())
                                EnemyDefense.Text = "防禦力：" + Form1.UsingProcess[User].Enemy[i + 5].Defense.ToString();

                            if (EnemyHP.Value != Form1.UsingProcess[User].Enemy[i + 5].HP)
                                EnemyHP.Value = Form1.UsingProcess[User].Enemy[i + 5].HP;

                            if (Form1.UsingProcess[User].Enemy[i + 5].MaxHP != 0 && EnemyHP.MaxValue != Form1.UsingProcess[User].Enemy[i + 5].MaxHP)
                                EnemyHP.MaxValue = Form1.UsingProcess[User].Enemy[i + 5].MaxHP;

                            if (Form1.UsingProcess[User].Enemy[i + 5].ID == AttackTargetID && Attack.BackColor != Color.Red)
                                Attack.BackColor = Color.Red;
                            if (Form1.UsingProcess[User].Enemy[i + 5].ID != AttackTargetID && Attack.BackColor != Color.Transparent)
                                Attack.BackColor = Color.Transparent;
                            //if (Form1.UsingProcess[User].Enemy[i + 5].ID != TargetID && EnemyBox.BackColor != Color.Transparent)
                            //EnemyBox.BackColor = Color.Transparent;
                            if (Form1.UsingProcess[User].Enemy[i + 5].State == 9 && EnemyAttack.ForeColor != Color.Red)
                                EnemyAttack.ForeColor = Color.Red;
                            if (Form1.UsingProcess[User].Enemy[i + 5].State != 9 && EnemyAttack.ForeColor != Color.Black)
                                EnemyAttack.ForeColor = Color.Black;
                        }
                        else
                            EnemyBox.Visible = false;
                        #endregion

                        #region 友方資料
                        if (Form1.UsingProcess[User].IsLandBattle && Form1.UsingProcess[User].Friend[i].ID != 0)
                        {
                            FriendBox.Visible = true;
                            if (FriendBox.Text != Form1.UsingProcess[User].Friend[i].Name)
                                FriendBox.Text = Form1.UsingProcess[User].Friend[i].Name;

                            if (FriendHP.Value != Form1.UsingProcess[User].Friend[i].HP)
                                FriendHP.Value = Form1.UsingProcess[User].Friend[i].HP;
                            if (Form1.UsingProcess[User].Friend[i].MaxHP != 0 && FriendHP.MaxValue != Form1.UsingProcess[User].Friend[i].MaxHP)
                                FriendHP.MaxValue = Form1.UsingProcess[User].Friend[i].MaxHP;

                            if (FriendMotion.Value != Form1.UsingProcess[User].Friend[i].Motion)
                                FriendMotion.Value = Form1.UsingProcess[User].Friend[i].Motion;
                            if (Form1.UsingProcess[User].Friend[i].MaxMotion != 0 && FriendMotion.MaxValue != Form1.UsingProcess[User].Friend[i].MaxMotion)
                                FriendMotion.MaxValue = Form1.UsingProcess[User].Friend[i].MaxMotion;
                        }
                        else
                        {
                            for (int b = 0; b < 7; b++)
                            {
                                if (Shortcuts[b].BackgroundImage != null)
                                    Shortcuts[b].BackgroundImage = null;
                            }
                            FriendBox.Visible = false;
                        }
                        #endregion

                    }

                    if (Form1.UsingProcess[User].IsLandBattle)
                    {
                        if (Form1.UsingProcess[User].ID == Form1.UsingProcess[User].Friend[i].ID && Form1.UsingProcess[User].Friend[i].HP > 0)
                        {
                            #region 快捷鍵資料
                            //讀取快捷鍵資料
                            Call.GetLBShortcuts(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._LandShortcuts);

                            Pen myPen = new Pen(Color.Red, 2);
                            for (int b = 0; b < 7; b++)
                            {
                                if (b < 3)
                                    Shortcuts[b].BackgroundImage = skill.GetObject("_" + Form1.UsingProcess[User]._LandShortcuts[b].ID.ToString("D4")) as Image;
                                else
                                    Shortcuts[b].BackgroundImage = skill.GetObject("_" + Form1.UsingProcess[User]._LandShortcuts[b].ID.ToString("D8")) as Image;

                                if (Form1.UsingProcess[User]._LandShortcuts[b].ID != 0 && Shortcuts[b].BackgroundImage != null)
                                {
                                    Graphics newGraphics = Graphics.FromImage(Shortcuts[b].BackgroundImage);
                                    if (Form1.UsingProcess[User]._LandShortcuts[b].Num > 0)
                                        newGraphics.DrawString(Form1.UsingProcess[User]._LandShortcuts[b].Num.ToString(), new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), new Point(0, 28));
                                    if (!Form1.UsingProcess[User]._LandShortcuts[b].Use)
                                    {
                                        if (b < 3)
                                        {
                                            newGraphics.DrawLine(myPen, new Point(0, 0), new Point(16, 16));
                                            newGraphics.DrawLine(myPen, new Point(16, 0), new Point(0, 16));
                                        }
                                        else
                                        {
                                            myPen = new Pen(Color.Red, 6);
                                            newGraphics.DrawLine(myPen, new Point(0, 0), new Point(48, 48));
                                            newGraphics.DrawLine(myPen, new Point(48, 0), new Point(0, 48));
                                        }
                                    }
                                    newGraphics.Dispose();
                                }
                            }
                            #endregion

                            int CureHp = 0;
                            if (!string.IsNullOrWhiteSpace(CureBox.Text))
                                CureHp = Convert.ToInt32(CureBox.Text);

                            int SkillMotion = 200;
                            if (!string.IsNullOrWhiteSpace(MotionBox.Text))
                                SkillMotion = Convert.ToInt32(MotionBox.Text);

                            bool CheckCure = false;
                            if (Form1.UsingProcess[User].Friend[i].HP <= Form1.UsingProcess[User].Friend[i].MaxHP * ((float)CureHp / 100))
                            {
                                ++Form1.UsingProcess[User].LBCureCount;
                                if (Form1.UsingProcess[User].LBCureCount > 2)
                                {
                                    if (Form1.UsingProcess[User].User_Type == "猜猜看")
                                    {
                                        #region 自動補血
                                        for (int item_Index = 3; item_Index < 7 && UseCure.Checked && !CheckCure; item_Index++)
                                        {
                                            if (Form1.UsingProcess[User]._LandShortcuts[item_Index].Use)
                                            {
                                                string item_Name = Call.LoadItemInfo(Form1.UsingProcess[User]._LandShortcuts[item_Index].ID);
                                                if (!string.IsNullOrWhiteSpace(item_Name) && (item_Name.Contains("治療") || item_Name.Contains("療傷") || item_Name.Contains("醫神的神藥") || item_Name.Contains("梅迪奇家的祕藥")))  
                                                {
                                                    Call.UseLBItem(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].ID, item_Index - 3);
                                                    Form1.UsingProcess[User].LBCureCount = 1;
                                                    CheckCure = true;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else
                                Form1.UsingProcess[User].LBCureCount = 0;

                            if (!CheckCure)
                            {
                                int MinHP = 999, EnemyIndex = -1;
                                PointF AttackTarget_Coordinate = new PointF();

                                if (Form1.UsingProcess[User].User_Type == "猜猜看")
                                {
                                    #region 發呆時間過長
                                    if (Call.GetLBAttackTarget(Form1.UsingProcess[User].hWnd) > 0 && Call.Distance(Form1.UsingProcess[User].Old_Coordinate, Form1.UsingProcess[User].Coordinate) <= 150)
                                    {
                                        if (Call.GetUserAction(Form1.UsingProcess[User].hWnd) == -1)
                                        {
                                            ++Form1.UsingProcess[User].LBAttackIdleCount;
                                            if (Form1.UsingProcess[User].LBAttackIdleCount > 10)
                                            {
                                                Random rand = new Random();
                                                Call.MoveCoordinate(Form1.UsingProcess[User].hWnd, new PointF(Form1.UsingProcess[User].Coordinate.X + rand.Next(-350, 350), Form1.UsingProcess[User].Coordinate.Y + rand.Next(-350, 350)), true);
                                                //Console.WriteLine(Form1.UsingProcess[User].Name + "發呆時間過長，移動位置。" + Form1.UsingProcess[User].Coordinate);
                                                Form1.UsingProcess[User].LBAttackIdleCount = 0;
                                            }
                                        }
                                        else
                                            Form1.UsingProcess[User].LBAttackIdleCount = 0;
                                    }
                                    #endregion

                                    #region 選擇目標
                                    for (int e = 0; e < Form1.UsingProcess[User].Enemy.Length; e++)
                                    {
                                        if (Form1.UsingProcess[User].Enemy[e].HP > 0)
                                        {
                                            if (passive.Checked)
                                            {
                                                if (Call.Distance(Call.GetTargetCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Enemy[e].ID, 0), Form1.UsingProcess[User].Coordinate) < 1250)
                                                {
                                                    if (Form1.UsingProcess[User].Enemy[e].HP < MinHP)
                                                    {
                                                        AttackTarget_Coordinate = Call.GetTargetCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Enemy[e].ID, 0);
                                                        MinHP = Form1.UsingProcess[User].Enemy[e].HP;
                                                        EnemyIndex = e;
                                                    }
                                                    if (Form1.UsingProcess[User].Enemy[e].State == 9)
                                                        MinHP = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Form1.UsingProcess[User].Enemy[e].State == 9 || Form1.UsingProcess[User].Enemy[e].HP < MinHP)
                                                {
                                                    AttackTarget_Coordinate = Call.GetTargetCoordinate(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Enemy[e].ID, 0);
                                                    MinHP = Form1.UsingProcess[User].Enemy[e].HP;
                                                    EnemyIndex = e;
                                                    if (Form1.UsingProcess[User].Enemy[e].State == 9)
                                                        MinHP = 1;
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (EnemyIndex != -1)
                                    {
                                        if (Form1.UsingProcess[User].Friend[i].State != 1)
                                        {
                                            if (Form1.UsingProcess[User].Friend[i].State != 2)
                                                ++Form1.UsingProcess[User].LBAttackCount;
                                            else
                                                Form1.UsingProcess[User].LBAttackCount = 0;

                                            if (Form1.UsingProcess[User].Friend[i].Motion >= SkillMotion)
                                                ++Form1.UsingProcess[User].LBSkillCount;
                                            else
                                                Form1.UsingProcess[User].LBSkillCount = 0;

                                            if (!Call.CoordinateChange(Form1.UsingProcess[User].hWnd) && Call.GetUserAction(Form1.UsingProcess[User].hWnd) == -1)
                                            {
                                                if (Form1.UsingProcess[User].LBAttackCount > 4 && AutoSelectTarget.Checked)
                                                {
                                                    #region 自動攻擊
                                                    Call.LBAttack(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Enemy[EnemyIndex].ID);
                                                    Form1.UsingProcess[User].LBAttackCount = 0;
                                                    #endregion
                                                }
                                                else if (Form1.UsingProcess[User].LBSkillCount > 3 && AutoUseSkill.Checked)
                                                {
                                                    #region 自動技巧
                                                    for (int s = 0; s < 2; s++)
                                                    {
                                                        if (Form1.UsingProcess[User]._LandShortcuts[s].ID > 0 && Form1.UsingProcess[User]._LandShortcuts[s].Use)
                                                        {
                                                            Call.UseLBSkill(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User].Enemy[EnemyIndex].ID, s);
                                                            Form1.UsingProcess[User].LBSkillCount = 0;
                                                            //Console.WriteLine(Form1.UsingProcess[User].Name + " 使用技巧。");
                                                            break;
                                                        }
                                                    }
                                                    #endregion
                                                }

                                            }
                                        }
                                        else
                                        {
                                            Form1.UsingProcess[User].LBAttackCount = 0;
                                            Form1.UsingProcess[User].LBSkillCount = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                timer.Start();
            }
        }

        private void timer0_Tick(object sender, EventArgs e)
        {
            int User = 0;
            timer(User);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int User = 1;
            timer(User);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int User = 2;
            timer(User);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            int User = 3;
            timer(User);
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            int User = 4;
            timer(User);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int User = 0;
            for (int i = 0; i < Form1.UsingProcess[User]._People.Length; i++)
            {
                bool IsParty = false;
                for (int j = 0; j < 5; j++)
                {
                    if (Form1.UsingProcess[User]._People[i].ID == Form1.UsingProcess[User].PartyID[j] || Form1.UsingProcess[User]._People[i].ID == Form1.UsingProcess[User].ID)
                        IsParty = true;
                }
                if (IsParty)
                    continue;

                if (Form1.UsingProcess[User]._People[i].Name != "？？？" && Form1.UsingProcess[User]._People[i].Name != "！？" && Form1.UsingProcess[User]._People[i].Name != "寶箱")
                {
                    Call.StartLandBattle(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID);
                    Console.WriteLine("開戰目標:" + Form1.UsingProcess[User]._People[i].Name);
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int User = 0;
            for (int i = 0; i < Form1.UsingProcess[User]._People.Length; i++)
            {
                if (Form1.UsingProcess[User]._People[i].Name == "寶箱")
                {
                    Call.Talk(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID);
                    Console.WriteLine("打開目標:" + Form1.UsingProcess[User]._People[i].Name);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int User = 0;
            for (int i = 0; i < Form1.UsingProcess[User]._People.Length; i++)
            {
                if (Form1.UsingProcess[User]._People[i].Name == "？？？")
                {
                    Call.Talk(Form1.UsingProcess[User].hWnd, Form1.UsingProcess[User]._People[i].ID);
                    Console.WriteLine("點選出口:" + Form1.UsingProcess[User]._People[i].Name);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int User = 0;
            Call.IntoDungeon(Form1.UsingProcess[User].hWnd, 0x8);
        }

        private Boolean haveIntoDungeonItemNum(int User)
        {
            int mode = 0;
            int num = 0;
            switch (dungeonComboBox.Text)
            {
                case "紅毛城":
                    mode = 0x6;//上層0x6
                    num = 5;
                    break;
                case "錫拉庫薩":
                    mode = 0x8;//上層0x8
                    num = 1;
                    break;
                case "波爾多":
                    mode = 0x9;//上層0x9
                    num = 5;
                    break;
            }

            for (int item_index = 0; item_index < Form1.UsingProcess[User]._Item._HaveItemInfo.Length; item_index++)
            {
                if (Form1.UsingProcess[User]._Item._HaveItemInfo[item_index].Code == 0)
                    break;
                else if (Form1.UsingProcess[User]._Item._HaveItemInfo[item_index].Name == "探險船船票" && Form1.UsingProcess[User]._Item._HaveItemInfo[item_index].Num >= num)
                {
                    Call.IntoDungeon(Form1.UsingProcess[User].hWnd, mode);
                    Form1.UsingProcess[User].TimeCount = 0;
                    return true;
                }
            }
            return false;
        }
    }
}
