﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition; // use for voice chat you must to reference this before you can watch how to do this on youtube
using WMPLib; //used for background sound
using EventEnum = WerewolfClient.WerewolfModel.EventEnum;
using CommandEnum = WerewolfClient.WerewolfCommand.CommandEnum;
using WerewolfAPI.Model;
using Role = WerewolfAPI.Model.Role;

namespace WerewolfClient
{
    public partial class MainForm : Form, View
    {
        private Timer _updateTimer;
        private WerewolfController controller;
        private Game.PeriodEnum _currentPeriod;
        private int _currentDay;
        private int _currentTime;
        private bool _voteActivated;
        private bool _actionActivated;
        private bool _voiceChatActivated; // used for voice chat to people checking
        private string _role_voiceChat; //used for keep role from user voice
        private string _myRole;
        private bool _isDead;
        private bool _isSound; //for sound_background timer
        private bool clickSound; //for sound_background timer
        private List<Player> players = null;
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        WindowsMediaPlayer backGround_sound = new WindowsMediaPlayer();
        private System.Windows.Forms.Timer timer1;
        private int counter = 3; //for timer sound
        private int playerCounter = 0; //for counter player in GameWaiting

        public MainForm()
        {
            InitializeComponent();
            backGround_sound.URL = "rolemu_-_Peek-Door_Quest.mp3";

            backGround_sound.settings.volume = 50;
            _isSound = false;
            backGround_sound.controls.play();
            backGround_sound.settings.setMode("loop", true);

            timer_time();



            foreach (int i in Enumerable.Range(0, 16))
            {
                this.Controls["GBPlayers"].Controls["BtnPlayer" + i].Click += new System.EventHandler(this.BtnPlayerX_Click);
                this.Controls["GBPlayers"].Controls["BtnPlayer" + i].Tag = i;
            }

            _updateTimer = new Timer();
            _voteActivated = false;
            _actionActivated = false;
            _voiceChatActivated = false;
            EnableButton(BtnJoin, true);
            EnableButton(BtnAction, false);
            EnableButton(BtnVote, false);
            _myRole = null;
            _isDead = false;
        }

        private void OnTimerEvent(object sender, EventArgs e)
        {
            WerewolfCommand wcmd = new WerewolfCommand();
            wcmd.Action = CommandEnum.RequestUpdate;
            controller.ActionPerformed(wcmd);
        }

        public void AddChatMessage(string str)
        {
            TbChatBox.AppendText(str + Environment.NewLine);
        }

        public void EnableButton(Button btn, bool state)
        {
            btn.Visible = btn.Enabled = state;
        }

        private void UpdateAvatar(WerewolfModel wm)
        {
            int i = 0;
            foreach (Player player in wm.Players)
            {

                Controls["GBPlayers"].Controls["BtnPlayer" + i].Text = player.Name;
                if (player.Name == wm.Player.Name || player.Status != Player.StatusEnum.Alive)
                    
                {
                    // FIXME, need to optimize this
                    Image img = Properties.Resources.Icon_villager;
                    string role;
                    if (player.Name == wm.Player.Name)
                    {
                        role = _myRole;
                    }
                    else if (player.Role != null)
                    {
                        role = player.Role.Name;
                    }
                    else
                    {
                        continue;
                    }
                    switch (role)
                    {
                        case WerewolfModel.ROLE_SEER:
                            img = Properties.Resources.Icon_seer;
                            break;
                        case WerewolfModel.ROLE_AURA_SEER:
                            img = Properties.Resources.Icon_aura_seer;
                            break;
                        case WerewolfModel.ROLE_PRIEST:
                            img = Properties.Resources.Icon_priest;
                            break;
                        case WerewolfModel.ROLE_DOCTOR:
                            img = Properties.Resources.Icon_doctor;
                            break;
                        case WerewolfModel.ROLE_WEREWOLF:
                            img = Properties.Resources.Icon_monkey;
                            break;
                        case WerewolfModel.ROLE_WEREWOLF_SEER:
                            img = Properties.Resources.Icon_wolf_seer;
                            break;
                        case WerewolfModel.ROLE_ALPHA_WEREWOLF:
                            img = Properties.Resources.Icon_alpha_werewolf;
                            break;
                        case WerewolfModel.ROLE_WEREWOLF_SHAMAN:
                            img = Properties.Resources.Icon_wolf_shaman;
                            break;
                        case WerewolfModel.ROLE_MEDIUM:
                            img = Properties.Resources.Icon_medium;
                            break;
                        case WerewolfModel.ROLE_BODYGUARD:
                            img = Properties.Resources.Icon_bodyguard;
                            break;
                        case WerewolfModel.ROLE_JAILER:
                            img = Properties.Resources.Icon_jailer;
                            break;
                        case WerewolfModel.ROLE_FOOL:
                            img = Properties.Resources.Icon_fool;
                            break;
                        case WerewolfModel.ROLE_HEAD_HUNTER:
                            img = Properties.Resources.Icon_head_hunter;
                            break;
                        case WerewolfModel.ROLE_SERIAL_KILLER:
                            img = Properties.Resources.Icon_serial_killer;
                            break;
                        case WerewolfModel.ROLE_GUNNER:
                            img = Properties.Resources.Icon_gunner;
                            break;
                    }
                    ((Button)Controls["GBPlayers"].Controls["BtnPlayer" + i]).Image = img;
                }
                i++;
            }
        }
        public void Notify(Model m)
        {
            if (m is WerewolfModel)
            {
                WerewolfModel wm = (WerewolfModel)m;
                switch (wm.Event)
                {
                    case EventEnum.JoinGame:
                        if (wm.EventPayloads["Success"] == WerewolfModel.TRUE)
                        {
                            BtnJoin.Visible = false;
                            AddChatMessage("You're joing the game #" + wm.EventPayloads["Game.Id"] + ", please wait for game start.");
                            _updateTimer.Interval = 1000;
                            _updateTimer.Tick += new EventHandler(OnTimerEvent);
                            _updateTimer.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("You can't join the game, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    case EventEnum.GameStopped:
                        AddChatMessage("Game is finished, outcome is " + wm.EventPayloads["Game.Outcome"]);
                        _updateTimer.Enabled = false;
                        BtnJoin.Visible = true;
                        EnableButton(BtnJoin, true);

                        break;
                    case EventEnum.GameStarted:
                        players = wm.Players;
                        _myRole = wm.EventPayloads["Player.Role.Name"];
                        AddChatMessage("Your role is " + _myRole + ".");
                        _currentPeriod = Game.PeriodEnum.Night;
                        EnableButton(BtnAction, true);
                        switch (_myRole)
                        {
                            case WerewolfModel.ROLE_PRIEST:
                                BtnAction.Text = WerewolfModel.ACTION_HOLYWATER;
                                break;
                            case WerewolfModel.ROLE_GUNNER:
                                BtnAction.Text = WerewolfModel.ACTION_SHOOT;
                                break;
                            case WerewolfModel.ROLE_JAILER:
                                BtnAction.Text = WerewolfModel.ACTION_JAIL;
                                break;
                            case WerewolfModel.ROLE_WEREWOLF_SHAMAN:
                                BtnAction.Text = WerewolfModel.ACTION_ENCHANT;
                                break;
                            case WerewolfModel.ROLE_BODYGUARD:
                                BtnAction.Text = WerewolfModel.ACTION_GUARD;
                                break;
                            case WerewolfModel.ROLE_DOCTOR:
                                BtnAction.Text = WerewolfModel.ACTION_HEAL;
                                break;
                            case WerewolfModel.ROLE_SERIAL_KILLER:
                                BtnAction.Text = WerewolfModel.ACTION_KILL;
                                break;
                            case WerewolfModel.ROLE_SEER:
                            case WerewolfModel.ROLE_WEREWOLF_SEER:
                                BtnAction.Text = WerewolfModel.ACTION_REVEAL;
                                break;
                            case WerewolfModel.ROLE_AURA_SEER:
                                BtnAction.Text = WerewolfModel.ACTION_AURA;
                                break;
                            case WerewolfModel.ROLE_MEDIUM:
                                BtnAction.Text = WerewolfModel.ACTION_REVIVE;
                                break;
                            default:
                                EnableButton(BtnAction, false);
                                break;
                        }
                        EnableButton(BtnVote, true);
                        EnableButton(BtnJoin, false);
                        UpdateAvatar(wm);
                        break;
                    case EventEnum.GameWaiting:
                        if(playerCounter != wm.Players.Count)
                        {
                            foreach (Player player in wm.Players)
                            {
                                AddChatMessage(player.Name + " Join Game !!!");
                            }
                            playerCounter = wm.Players.Count;
                        }
                        break;


                    case EventEnum.SwitchToDayTime:
                        AddChatMessage("Switch to day time of day #" + wm.EventPayloads["Game.Current.Day"] + ".");
                        _currentPeriod = Game.PeriodEnum.Day;
                        LBPeriod.Text = "Day time of";
                        this.BackgroundImage = Properties.Resources.Daytime;
                        break;
                    case EventEnum.SwitchToNightTime:
                        AddChatMessage("Switch to night time of day #" + wm.EventPayloads["Game.Current.Day"] + ".");
                        _currentPeriod = Game.PeriodEnum.Night;
                        LBPeriod.Text = "Night time of";
                        this.BackgroundImage = Properties.Resources.Nighttime;
                        break;
                    case EventEnum.UpdateDay:
                        // TODO  catch parse exception here
                        string tempDay = wm.EventPayloads["Game.Current.Day"];
                        _currentDay = int.Parse(tempDay);
                        LBDay.Text = tempDay;
                        break;
                    case EventEnum.UpdateTime:
                        string tempTime = wm.EventPayloads["Game.Current.Time"];
                        _currentTime = int.Parse(tempTime);
                        LBTime.Text = tempTime;
                        UpdateAvatar(wm);
                        break;
                    case EventEnum.Vote:
                        if (wm.EventPayloads["Success"] == WerewolfModel.TRUE)
                        {
                            AddChatMessage("Your vote is registered.");
                        }
                        else
                        {
                            AddChatMessage("You can't vote now.");
                        }
                        break;
                    case EventEnum.Action:
                        if (wm.EventPayloads["Success"] == WerewolfModel.TRUE)
                        {
                            AddChatMessage("Your action is registered.");
                        }
                        else
                        {
                            AddChatMessage("You can't perform action now.");
                        }
                        break;
                    case EventEnum.YouShotDead:
                        AddChatMessage("You're shot dead by gunner.");
                        _isDead = true;
                        break;
                    case EventEnum.OtherShotDead:
                        AddChatMessage(wm.EventPayloads["Game.Target.Name"] + " was shot dead by gunner.");
                        break;
                    case EventEnum.Alive:
                        AddChatMessage(wm.EventPayloads["Game.Target.Name"] + " has been revived by medium.");
                        if (wm.EventPayloads["Game.Target.Id"] == null)
                        {
                            _isDead = false;
                        }
                        break;
                    case EventEnum.ChatMessage:
                        if (wm.EventPayloads["Success"] == WerewolfModel.TRUE)
                        {
                            for (int i = 0; i < wm.Players.Count; i++)
                        {
                            if (wm.EventPayloads["Game.Chatter"] == wm.Players[i].Name)
                            {
                                switch (i+1)
                                {
                                    case 1:textBox1.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 2:
                                        textBox2.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 3:
                                        textBox3.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 4:
                                        textBox4.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 5:
                                        textBox5.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 6:
                                        textBox6.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 7:
                                        textBox7.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 8:
                                        textBox8.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 9:
                                        textBox9.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 10:
                                        textBox10.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 11:
                                        textBox11.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 12:
                                        textBox12.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 13:
                                        textBox13.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 14:
                                        textBox14.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 15:
                                        textBox15.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;
                                    case 16:
                                        textBox16.Text = wm.EventPayloads["Game.ChatMessage"];
                                        break;

                                }
                            }
                        }
                        
                        
                            AddChatMessage(wm.EventPayloads["Game.Chatter"] + ":" + wm.EventPayloads["Game.ChatMessage"]);
                        }
                        break;
                    case EventEnum.Chat:
                        if (wm.EventPayloads["Success"] == WerewolfModel.FALSE)
                        {
                            switch (wm.EventPayloads["Error"])
                            {
                                case "403":
                                    AddChatMessage("You're not alive, can't talk now.");
                                    break;
                                case "404":
                                    AddChatMessage("You're not existed, can't talk now.");
                                    break;
                                case "405":
                                    AddChatMessage("You're not in a game, can't talk now.");
                                    break;
                                case "406":
                                    AddChatMessage("You're not allow to talk now, go to sleep.");
                                    break;
                            }
                        }
                        break;
                    case EventEnum.SignOut:
                        this.Visible = false;
                        BtnJoin.Visible = true;
                        EnableButton(BtnJoin, true);
                        TbChatBox.Text = "";
                        playerCounter = 0;
                        break;
                    case EventEnum.LeaveGame:
                        BtnJoin.Visible = true;
                        EnableButton(BtnJoin, true);
                        TbChatBox.Text = "";
                        playerCounter = 0;
                        break;
                    case EventEnum.Soundbackground:
                        if(_isSound)
                        {
                            counter = 2;
                            //backGround_sound.settings.volume = 20;
                            //clickSound = !clickSound;
                            _isSound = false;
                            timer_time();
                            
                        }
                        else
                        {

                            counter = 2;
                            //backGround_sound.settings.volume = 0;
                            clickSound = !clickSound;
                            _isSound = true;
                            timer_time();
                        }
                        EnableButton(BtnJoin, true);
                        TbChatBox.Text = "";
                        break;
                }
                // need to reset event
                wm.Event = EventEnum.NOP;
            }
        }

        public void setController(Controller c)
        {
            controller = (WerewolfController)c;
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            WerewolfCommand wcmd = new WerewolfCommand();
            wcmd.Action = CommandEnum.JoinGame;
            controller.ActionPerformed(wcmd);
        }

        private void BtnVote_Click(object sender, EventArgs e)
        {
            if (_voteActivated)
            {
                BtnVote.BackColor = Button.DefaultBackColor;
            }
            else
            {
                BtnVote.BackColor = Color.Red;
            }
            _voteActivated = !_voteActivated;
            if (_actionActivated)
            {
                BtnAction.BackColor = Button.DefaultBackColor;
                _actionActivated = false;
            }
        }

        private void BtnAction_Click(object sender, EventArgs e)
        {
            if (_isDead)
            {
                AddChatMessage("You're dead!!");
                return;
            }
            if (_actionActivated)
            {
                BtnAction.BackColor = Button.DefaultBackColor;
            }
            else
            {
                BtnAction.BackColor = Color.Red;
            }
            _actionActivated = !_actionActivated;
            if (_voteActivated)
            {
                BtnVote.BackColor = Button.DefaultBackColor;
                _voteActivated = false;
            }
        }

        private void BtnPlayerX_Click(object sender, EventArgs e)
        {
            Button btnPlayer = (Button)sender;
            int index = (int) btnPlayer.Tag;
            if (players == null)
            {
                // Nothing to do here;
                return;
            }
            if (_actionActivated)
            {
                _actionActivated = false;
                BtnAction.BackColor = Button.DefaultBackColor;
                AddChatMessage("You perform [" + BtnAction.Text + "] on " + players[index].Name);
                WerewolfCommand wcmd = new WerewolfCommand();
                wcmd.Action = CommandEnum.Action;
                wcmd.Payloads = new Dictionary<string, string>() { { "Target", players[index].Id.ToString() } };
                controller.ActionPerformed(wcmd);
            }
            if (_voteActivated)
            {
                _voteActivated = false;
                BtnVote.BackColor = Button.DefaultBackColor;
                AddChatMessage("You vote on " + players[index].Name);
                WerewolfCommand wcmd = new WerewolfCommand();
                wcmd.Action = CommandEnum.Vote;
                wcmd.Payloads = new Dictionary<string, string>() { { "Target", players[index].Id.ToString() } };
                controller.ActionPerformed(wcmd);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TbChatInput_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && TbChatInput.Text != "")
            {
                WerewolfCommand wcmd = new WerewolfCommand();
                wcmd.Action = CommandEnum.Chat;
                wcmd.Payloads = new Dictionary<string, string>() { { "Message", TbChatInput.Text } };
                TbChatInput.Text = "";
                controller.ActionPerformed(wcmd);
            }
        }

        private void buttonVoice_Click(object sender, EventArgs e)
        {
            try
            {
                recEngine.RecognizeAsync(RecognizeMode.Multiple);
                AddChatMessage("Now, Voice chat is on !!!");
                backGround_sound.controls.pause();
            }
            catch (Exception ex)
            {
                recEngine.RecognizeAsyncStop();
                AddChatMessage("Now, Voice chat is off !!!");
                backGround_sound.controls.play();
            }


        }
        /// <summary>
        /// build for Embedding voice chat are used in MainForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //backGround_sound.controls.play();
            Choices commands = new Choices();
            WinApI.AnimateWindow(this.Handle, 1500, WinApI.BLEND);

            commands.Add(new string[] { "say hello", "print my name", "kill", "vote",
            "player0", "player1", "player2", "player3", "player4" , "player5", "player6", "player7", "player8", "player9",
            "player10", "player11", "player12", "player13", "player14", "player15", "I am wolf", "I am villager", "wolf is "});

            GrammarBuilder gBuilder = new GrammarBuilder();

            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);



            recEngine.LoadGrammarAsync(grammar);

            recEngine.SetInputToDefaultAudioDevice();

            recEngine.SpeechRecognized += recEngine_SpeechRecognized;

        }
        /// <summary>
        /// voice chat command are used in voice button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)

        {
            WerewolfCommand wcmd = new WerewolfCommand();
            switch (e.Result.Text)

            {

                case "say hello":

                    MessageBox.Show("hello Top");
                    wcmd.Action = CommandEnum.Chat;
                    wcmd.Payloads = new Dictionary<string, string>() { { "Message", "say hello" } };
                    controller.ActionPerformed(wcmd);

                    break;

                case "I am wolf":
                    wcmd.Action = CommandEnum.Chat;
                    wcmd.Payloads = new Dictionary<string, string>() { { "Message", "I am wolf" } };
                    controller.ActionPerformed(wcmd);
                    break;
                case "I am villager":
                    wcmd.Action = CommandEnum.Chat;
                    wcmd.Payloads = new Dictionary<string, string>() { { "Message", "I am villager" } };
                    controller.ActionPerformed(wcmd);
                    break;
                case "wolf is":
                    AddChatMessage("And you say which player you think. ex: you can say 'player2' ");
                    _role_voiceChat = "wolf";
                    _voiceChatActivated = true;
                    break;
                case "kill":
                    _voteActivated = false;
                    _actionActivated = true;
                    AddChatMessage("which player you want to kill ? 0-15 from top ex: you can say 'player11' ");
                    break;
                case "vote":
                    _voteActivated = true;
                    _actionActivated = false;
                    AddChatMessage("which player you want to vote ? 1-15 from top ex: you can say 'player11' ");
                    break;
                case "player0":
                    IThink_voiceChat(_role_voiceChat, 0);
                    voteAndActionForVoiceChat(0);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player1":
                    IThink_voiceChat(_role_voiceChat, 1);
                    voteAndActionForVoiceChat(1);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player2":
                    IThink_voiceChat(_role_voiceChat, 2);
                    voteAndActionForVoiceChat(2);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player3":
                    IThink_voiceChat(_role_voiceChat, 3);
                    voteAndActionForVoiceChat(3);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;

                case "player4":
                    IThink_voiceChat(_role_voiceChat, 4);
                    voteAndActionForVoiceChat(4);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player5":
                    IThink_voiceChat(_role_voiceChat, 5);
                    voteAndActionForVoiceChat(5);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player6":
                    IThink_voiceChat(_role_voiceChat, 6);
                    voteAndActionForVoiceChat(6);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player7":
                    IThink_voiceChat(_role_voiceChat, 7);
                    voteAndActionForVoiceChat(7);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player8":
                    IThink_voiceChat(_role_voiceChat, 8);
                    voteAndActionForVoiceChat(8);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player9":
                    IThink_voiceChat(_role_voiceChat, 9);
                    voteAndActionForVoiceChat(9);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player10":
                    IThink_voiceChat(_role_voiceChat, 10);
                    voteAndActionForVoiceChat(10);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player11":
                    IThink_voiceChat(_role_voiceChat, 11);
                    voteAndActionForVoiceChat(11);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player12":
                    IThink_voiceChat(_role_voiceChat, 12);
                    voteAndActionForVoiceChat(12);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player13":
                    IThink_voiceChat(_role_voiceChat, 13);
                    voteAndActionForVoiceChat(13);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player14":
                    IThink_voiceChat(_role_voiceChat, 14);
                    voteAndActionForVoiceChat(14);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;
                case "player15":
                    IThink_voiceChat(_role_voiceChat, 15);
                    voteAndActionForVoiceChat(15);
                    _voteActivated = false;
                    _actionActivated = false;
                    _voiceChatActivated = false;

                    break;

            }

        }
        /// <summary>
        /// use for select to vote or action player on recEngine_SpeechRecognized()
        /// </summary>
        /// <param name="i">number of player 0-15 </param>
        private void voteAndActionForVoiceChat(int i)
        {
            WerewolfCommand wcmd = new WerewolfCommand();
            int index = (int)Controls["GBPlayers"].Controls["BtnPlayer" + i].Tag;
            if (_actionActivated)
            {
                try
                {
                    _actionActivated = false;
                    BtnAction.BackColor = Button.DefaultBackColor;
                    AddChatMessage("You perform [" + BtnAction.Text + "] on " + players[index].Name);
                    wcmd.Action = CommandEnum.Action;
                    wcmd.Payloads = new Dictionary<string, string>() { { "Target", players[index].Id.ToString() } };
                    controller.ActionPerformed(wcmd);
                }
                catch(Exception ex)
                {
                    AddChatMessage("You can't kill on player" + i);
                }

            }
            if (_voteActivated)
            {
                _voteActivated = false;
                BtnVote.BackColor = Button.DefaultBackColor;
                try
                {
                    AddChatMessage("You vote on " + players[index].Name);
                    wcmd.Action = CommandEnum.Vote;
                    wcmd.Payloads = new Dictionary<string, string>() { { "Target", players[index].Id.ToString() } };
                    controller.ActionPerformed(wcmd);
                }
                catch(Exception ex)
                {
                    AddChatMessage("You can't vote on player" + i);
                }

            }

        }
        /// <summary>
        /// Use of chat on voice chat and it are used in recEngine_SpeechRecognized()
        /// </summary>
        /// <param name="role">role of who you think that</param>
        /// <param name="i">player</param>
        private void IThink_voiceChat(string role, int i)
        {
            if(_voiceChatActivated)
            {
                try
                {
                    WerewolfCommand wcmd = new WerewolfCommand();
                    wcmd.Action = CommandEnum.Chat;
                    wcmd.Payloads = new Dictionary<string, string>() { { "Message", "I think " + players[i].Name + " is " + role} };
                    controller.ActionPerformed(wcmd);
                }
                catch(Exception ex)
                {
                    AddChatMessage("You can't think on player" + i);
                }
            }

        }


        private void BtnLogout_Click(object sender, EventArgs e)
        {
            WerewolfCommand wcmd = new WerewolfCommand();
            wcmd.Action = CommandEnum.SignOut;
            controller.ActionPerformed(wcmd);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            x
        }

        private void Leave_Click(object sender, EventArgs e)
        {
            WerewolfCommand wcmd = new WerewolfCommand();
            wcmd.Action = CommandEnum.LeaveGame;
            controller.ActionPerformed(wcmd);

        }
        private void timer1_Tick(object sender, EventArgs e)

        {
            counter--;
            System.Console.WriteLine(counter);
            if(!_isSound)
            {
                backGround_sound.settings.volume += 10;
            }
            else
            {
                backGround_sound.settings.volume -= 10;
            }

            
            if (backGround_sound.settings.volume >= 50 || backGround_sound.settings.volume <= 0)
            {
                //if (_isSound)
                //{
                //    backGround_sound.controls.pause();
                //}
                //else
                //{
                //    backGround_sound.controls.play();
                //}
                //_isSound = !_isSound;
                //clickSound = !clickSound;
                timer1.Stop();

            }
                

        }
        private void timer_time()
        {
            //int counter = 6;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);

            timer1.Interval = 1000; // 1 second
            timer1.Start();



        }





        private void htp_click_Click(object sender, EventArgs e)
        {
            HTP htp = new HTP(this);
            this.Visible = false;
            htp.Visible = true;
        }

        private void menu_click_Click(object sender, EventArgs e)
        {
            if (htp_click.Visible == false && Leave.Visible == false)
            {
                htp_click.Visible = true;
                Leave.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                playerInGame.Visible = true;
            }
            else
            {
                htp_click.Visible = false;
                Leave.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                playerInGame.Visible = false;

            }

        }

    }
}
