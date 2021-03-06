﻿using IdleClickerEngine;
using System;
using System.Configuration;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Cookie_Clicker
{
    public partial class BossFight : Form
    {
        private readonly BigInteger _bossHp;
        private readonly string _image;
        private BigInteger _remainingHealth;

        private int _timer = 30;

        private BigInteger _countDps;
  
        private readonly MyUnits _units = MyUnits.GetInstance();

        private readonly HealthBar _healthBar
            = new HealthBar(new Rectangle(245, 480, 405, 44));

        public BossFight(BigInteger bossHp,string image)
        {
            InitializeComponent();
            Paint += OnPaint;

            _bossHp = bossHp;
            _image = image;
           
            _remainingHealth = _bossHp;
            RefreshValues();
            LoadImage();
        }

        //Refresh all important values
        private void RefreshValues()
        {
            if (healthLabel != null)
            {
                healthLabel.Text = $@"{_remainingHealth}/{_bossHp}";
            }
            //Count dps of your units
            _countDps = _units.GetDpsOfYourUnits();

            time.Text = _timer + "s remaining";
            YourCoinsAndDps.Text = "Your Dps: " + BigIntegerFormatter.FormatWithSuffix(_countDps) + "\n" +
                                   "Coins: " + BigIntegerFormatter.FormatWithSuffix(MyInfo.Coins);
            Refresh();
        }
        
        /// <summary>
        /// Load image of boss
        /// </summary>
        private void LoadImage()
        { 
            pictureBox.Image = Image.FromFile(ConfigurationManager.AppSettings[_image]);
        }

        /// <summary>
        /// Attack monster by specified damage.
        /// </summary>
        /// <param name="damage"> Damage which was dealt to monster. </param>
        private void AttackMonster(BigInteger damage)
        {
            _remainingHealth -= damage;
        }

        /// <summary>
        /// Check if boss is still alive, if yes then is increased level of game
        /// </summary>
        public void IsMonsterStillAlive()
        {
            if (_remainingHealth > 0) return;
            
            _remainingHealth = 0;
            
            RefreshValues();

            Timer.Enabled = false;

            MessageBox.Show("You Won");
            MyInfo.Level++;

            //If level of game is six is opened the end form(you defeated all bosses)
            IsItTheEnd();
            
            Dispose();
           
        }

        /// <summary>
        /// Check if you defeated final boss
        /// </summary>
        private void IsItTheEnd()
        {
            if (MyInfo.Level == 6)
            {
                Hide();
                TheEnd();
            }
            //If it is not end you will be returned to main cookie form, but with increased level
            Dispose();
            CookieForm newCookieForm = new CookieForm((int)Math.Pow(10, MyInfo.Level + 1), (int)Math.Pow(10, MyInfo.Level - 1));
            newCookieForm.ShowDialog();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            _healthBar.DrawHealth(e, _remainingHealth, _bossHp);
        }

        /// <summary>
        /// Clicking on boss image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BossAttackClick(object sender, EventArgs e)
        {
            AttackMonster(MyInfo.ClickDamage);
            IsMonsterStillAlive();
            RefreshValues();
        }

        /// <summary>
        /// This form is opened when you finish the game
        /// </summary>
        private void TheEnd()
        {
            End end = new End();
            end.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            AttackMonster(_countDps);
            if (_timer <= 0) ElapsedTime();
            
            IsMonsterStillAlive();

            RefreshValues();
            _timer--;
        }

        /// <summary>
        /// If time already elapsed you will be returned to main form with the same level
        /// </summary>
        private void ElapsedTime()
        {
            time.Text = 0 + "s remaining";
            Timer.Enabled = false;
            MessageBox.Show("You Lost!");
            CookieForm newCookieForm = new CookieForm((int) Math.Pow(10, MyInfo.Level), (int) Math.Pow(10, MyInfo.Level - 1));
            Dispose();
            newCookieForm.ShowDialog();
        }
    }
}
