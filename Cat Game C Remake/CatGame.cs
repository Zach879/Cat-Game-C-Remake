/* Zachary Reese ~ RELEASED: 9/28/2019 ~ Cat Game Project remade and improved in C#
 * TODO: n/a */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace Cat_Game_C_Remake
{
    public partial class frmCatGame : Form
    {
        int MoveCheck, ShotAccumulator, Hits; //handles user output at the end
        int part = 0; //intro animation segment counter
        bool timerBulletEnabled = false;
        int Health = 5; //player health
        int bulletCount = 20; //bullets user has
        int enemyMIN = 5; //minimum speed
        int enemyMAX = 10; //maximum speed
        int enemyAmount = 1; //controls maximum enemies
        int bulletTimerInterval = 100; //current bullet timer interval
        static double difficulty = 1.0; //difficulty to be multiple onto enemy speed
        double difficultyChange = 1.0; //changes after difficulty to check for change
        bool oneTime = true; //used for running code one time in loops
        bool swing = false; //three booleans used in sword movement
        bool swordPart = true; //|
        const int _formWidth = 1250;
        const int _formHeight = 496;
        public double Difficulty = 1.0; //difficulty static made public
        static int BulletXPos = 170; //Changes bullet position to line up with the character's gun
        static int BulletYPos = 203; //|
        double enemyMoveRate = 1; //Becomes 0.5 when powerup collect to slow enemies
        int swordMoveRate = 1;    //Changes the speed of the sword animation
        double powerUpTimer = 10; //rate of powerups spawn
        double powerUpTimer2 = 0; //duration of collect powerups
        const int _Character1BulletSpeed = 65; //|
        const int _Character2BulletSpeed = 40; //|
        const int _Character3BulletSpeed = 25; //Specific bullet speeds per cat
        const int _Character4BulletSpeed = 32; //|
        const int _Character5BulletSpeed = 30; //|
        const int _Character6BulletSpeed = 20; //|
        int BulletSpeed = _Character1BulletSpeed; //current bullet speed
        bool timerBulletChanged = false;
        bool reloadBoosterActivated = false;
        bool autofire = false; //true when autofire is on; false when autofire is off
        int tag = 0; //1 is bullet pack; 2 is health bag; 3 is faster reload; 4 is slower enemies
        const string _fileLocation1 = "D:\\10-12th Grade\\Cat Game C Remake\\ProgramData.txt"; //Program Data for Restart
        const string _fileLocation2 = "D:\\10-12th Grade\\Cat Game C Remake\\ProgramData2.txt"; //Program Data for checking for Restart
        const string _fileLocation3 = "D:\\10-12th Grade\\Cat Game C Remake\\Highscores.txt"; //Program Data for storing high scores
        const string _CharacterHitSoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\OOFFast.wav"; //Constant file for end hit sound
        const string _EnemyPassBorderSoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\explode.wav"; //Constant file for enemy passes border sound
        const string _PowerUpCollectedSoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\ching.wav"; //Constant file for powerup collected sound

        bool[] WASD = new bool[4]; //checks for movement keydowns
        List<int> enemyspeed = new List<int>(); //gives each character a unique movement speed
        List<int> bulletspeed = new List<int>();

        List<PictureBox> bulletlist = new List<PictureBox>(); //holds all bullet entities
        List<PictureBox> enemylist = new List<PictureBox>(); //holds all enemy entities
        List<PictureBox> powerList = new List<PictureBox>(); //holds all power ups

        frmChangeDifficulty changedifficulty = new frmChangeDifficulty();

        SoundPlayer SP = new SoundPlayer("D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\Sniper_Fire.wav"); //Handles character shoot sound
        SoundPlayer SP2 = new SoundPlayer(_CharacterHitSoundLocation); //Handles enemy hit sound

        private void frmCatGame_Load(object sender, EventArgs e)
        {
            /*string name = "";
            name = Microsoft.VisualBasic.Interaction.InputBox("Enter your name:", "Enter Name", "hoonteir");
            if (name.ToLower() == "hunter")
                enemyAmount = 50;*/


            StreamReader sr2 = new StreamReader(_fileLocation1);
            sr2.ReadLine();
            if (Convert.ToBoolean(sr2.ReadLine()))
            {
                difficulty = Convert.ToDouble(sr2.ReadLine());
                DialogResult dialogResult = MessageBox.Show("Would you like to watch the Cat Game intro animation?", "Intro Animation?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    timerIntro.Enabled = true;
                else
                {
                    StreamReader sr = new StreamReader(_fileLocation1);
                    ChangeCharacterAgain(Convert.ToInt16(sr.ReadLine()));
                    sr.Close();
                    timerMain.Enabled = true;
                    EnemyAction();
                    timerPowerUps.Enabled = true;
                }
            }
            else
            {
                difficulty = Convert.ToDouble(sr2.ReadLine());
                DifficultyConversion();
                StreamReader sr = new StreamReader(_fileLocation1);
                ChangeCharacterAgain(Convert.ToInt16(sr.ReadLine()));
                sr.Close();
                timerMain.Enabled = true;
                EnemyAction();
                timerPowerUps.Enabled = true;
            }
            sr2.Close();
        }
        private void frmCatGame_KeyDown(object sender, KeyEventArgs e)
        {
            bool makebullet = true;
            if (timerIntro.Enabled == false)
            {
                MoveCheck = (int)e.KeyCode;
                switch (MoveCheck)
                {
                    case 38: //up arrow
                    case 87: //W
                        MoveCheck = 87;
                        WASD[0] = true;
                        break;
                    case 40: //down arrow
                    case 83: //S
                        MoveCheck = 83;
                        WASD[1] = true;
                        break;
                    case 37: //left arrow
                    case 65: //A          
                        MoveCheck = 65;
                        WASD[2] = true;
                        break;
                    case 39: //right arrow
                    case 68: //D
                        MoveCheck = 68;
                        WASD[3] = true;
                        break;
                    case 84: //T (toggle autofire)
                        if (autofire == false)
                        {
                            autofire = true;
                            lblAutoFire.Text = "Autofire On (T)";
                        }
                        else
                        {
                            autofire = false;
                            lblAutoFire.Text = "Autofire Off (T)";
                        }
                        break;
                    case 49: //1
                        ChangeCharacterAgain(1);
                        makebullet = false;
                        break;
                    case 50: //2
                        ChangeCharacterAgain(2);
                        makebullet = false;
                        break;
                    case 51: //3
                        ChangeCharacterAgain(3);
                        makebullet = false;
                        break;
                    case 52: //4
                        ChangeCharacterAgain(4);
                        makebullet = false;
                        break;
                    case 53: //5
                        ChangeCharacterAgain(5);
                        makebullet = false;
                        break;
                    case 54: //6
                        ChangeCharacterAgain(6);
                        makebullet = false;
                        break;
                    case 55: //7
                        ChangeCharacterAgain(7);
                        makebullet = false;
                        break;
                } //end switch
                if (BulletSpeed == _Character6BulletSpeed && MoveCheck != 87 && MoveCheck != 83 && MoveCheck != 65 && MoveCheck != 68)
                {
                    swing = true;
                    SP.Play();
                    ShotAccumulator++;
                    makebullet = false;
                    picCount1.BackColor = Color.Red;
                    picCount2.BackColor = Color.Red;
                    picCount3.BackColor = Color.Red;
                    picCount4.BackColor = Color.Red;
                    picCount5.BackColor = Color.Red;
                }

                if (makebullet)
                    PreNewBulletCheck();
            } //end if      
        } //end function
        public void PreNewBulletCheck()
        {
            bool character4Check = true;
            if (BulletSpeed == _Character4BulletSpeed)
                foreach (int bullets in bulletspeed)
                {
                    if (bullets == _Character4BulletSpeed)
                        character4Check = false;
                }

            if (timerBullet.Enabled == false && bulletCount >= 1)
            {
                bool makeBullet = false;
                if (BulletSpeed == _Character1BulletSpeed)
                    makeBullet = true;
                else if (BulletSpeed == _Character2BulletSpeed && MoveCheck != 87 && MoveCheck != 83 && MoveCheck != 65 && MoveCheck != 68)
                    makeBullet = true;
                else if (MakeABullet(_Character3BulletSpeed))
                    makeBullet = true;
                else if (MakeABullet(_Character4BulletSpeed) && character4Check)
                    makeBullet = true;
                else if (MakeABullet(_Character5BulletSpeed))
                    makeBullet = true;
                else if (BulletSpeed == _Character6BulletSpeed && autofire)
                    swing = true;


                if (makeBullet == true)
                    MakeNewBullet(reloadBoosterActivated);
            } //end if
        }
        public bool MakeABullet(int characterSpeed)
        {            
            if (BulletSpeed == characterSpeed && MoveCheck != 87 && MoveCheck != 83)
                return true;
            else
                return false;
        }
        private void frmCatGame_KeyUp(object sender, KeyEventArgs e)
        {
            WASD[0] = false;
            WASD[1] = false;
            WASD[2] = false; //correct movement stop with two keys and one is let go
            WASD[3] = false;
            MoveCheck = 0;
        }
        public frmCatGame()
        {
            InitializeComponent();

        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (bulletCount != 0)
            {
                if (picCount1.BackColor == Color.Red)
                    picCount1.BackColor = Color.Green;
                else if (picCount2.BackColor == Color.Red)
                    picCount2.BackColor = Color.Green;
                else if (picCount3.BackColor == Color.Red)
                    picCount3.BackColor = Color.Green;
                else if (picCount4.BackColor == Color.Red)
                    picCount4.BackColor = Color.Green;
                else if (picCount5.BackColor == Color.Red)
                    picCount5.BackColor = Color.Green;
                else
                    timerBullet.Enabled = false;
            }

            //if (powerUpTimer2 >= 9.8)
            //    timerBullet.Interval = bulletTimerInterval;

        }
        private int rocketExist() //Checks if a rocket exists, and if so returns its index
        {
            int rIndex = 0;
            foreach (int bullets in bulletspeed)
            {
                if (bullets == _Character4BulletSpeed)
                    return rIndex;
                rIndex++;
            }
            return -1;
        }
        private void timer1_Tick(object sender, EventArgs e) //set up movement and collision
        {
            //Moves character
            if (WASD[0] && BulletSpeed != _Character1BulletSpeed) //W
            {
                if (BulletSpeed != _Character4BulletSpeed)
                {
                    if (BulletSpeed == _Character6BulletSpeed)
                    {
                        if (picCharacter.Top > 15 - 125)
                            picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y - 15);
                    }
                    else
                    {
                        //if (picCharacter.Location.Y <= 0)
                        //{
                        //    picCharacter.Location = new Point(picCharacter.Location.X, 1);
                        //    WASD[0] = false;
                        //}
                        //else
                        //    picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y - 15);
                        if (picCharacter.Top > 15 - 50)
                            picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y - 15);
                    }
                }
                else
                {
                    int rocketIndex = rocketExist();
                    if (rocketIndex != -1)
                        bulletlist[rocketIndex].Location = new Point(bulletlist[rocketIndex].Location.X, bulletlist[rocketIndex].Location.Y - 15); //rocket movement
                } //end else
            } //end W-if
            if (WASD[1] && BulletSpeed != _Character1BulletSpeed) //S
            {
                if (BulletSpeed != _Character4BulletSpeed)
                {
                    if (BulletSpeed == _Character3BulletSpeed)
                    {
                        if (picCharacter.Bottom < _formHeight - 15 + 50)
                            picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y + 15);
                    }
                    else
                        if (picCharacter.Bottom < _formHeight)
                        picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y + 15);
                }
                else
                    if (picCharacter.Bottom < _formHeight - 126)
                {
                    int rocketIndex = rocketExist();
                    if (rocketIndex != -1)
                        bulletlist[rocketIndex].Location = new Point(bulletlist[rocketIndex].Location.X, bulletlist[rocketIndex].Location.Y + 15); //rocket movement
                } //end else
            }
            if (WASD[2] && (BulletSpeed == _Character2BulletSpeed || BulletSpeed == _Character6BulletSpeed)) //A
            {
                if (picCharacter.Location.X <= 0)
                {
                    picCharacter.Location = new Point(1, picCharacter.Location.Y);
                    WASD[2] = false;
                }
                else if (picCharacter.Left > 20)
                    picCharacter.Location = new Point(picCharacter.Location.X - 20, picCharacter.Location.Y);
            }
            if (WASD[3] && (BulletSpeed == _Character2BulletSpeed || BulletSpeed == _Character6BulletSpeed)) //D
            {
                if (picCharacter.Right >= picBorder.Left/*picBorder.Location.X - picCharacter.Width*/)
                {
                    picCharacter.Location = new Point(picBorder.Location.X - picCharacter.Width - 1, picCharacter.Location.Y);
                    WASD[3] = false;
                }
                else if (picBorder.Left - picCharacter.Right > 20)
                    picCharacter.Location = new Point(picCharacter.Location.X + 20, picCharacter.Location.Y);
            }

            if (autofire) //Handles autofire
            {
                if (bulletCount == 0)
                {
                    if (swing == false)
                    {
                        autofire = false;
                        lblAutoFire.Text = "Autofire Off";
                    }
                } else
                    PreNewBulletCheck();
            }

            int enemySpot = 0;
            foreach (PictureBox enemies in enemylist) //handles enemy movement & passing border
            {
                enemies.Location = new Point(Convert.ToInt32((enemies.Location.X - enemyspeed[enemySpot] * (enemyMoveRate * difficulty))), Convert.ToInt32(enemies.Location.Y + enemyspeed[enemySpot + 1]
                    * enemyMoveRate));

                if (enemies.Top <= 0)
                {
                    enemyspeed[enemySpot + 1] *= -1;
                    enemies.Location = new Point(enemies.Location.X, 15);
                } else if (enemies.Bottom >= _formHeight)
                {
                    enemyspeed[enemySpot + 1] *= -1;
                    enemies.Location = new Point(enemies.Location.X, enemies.Location.Y - 15);
                }

                enemySpot += 2;

                if (enemies.Location.X <= picBorder.Left) //if enemy passes border
                {
                    enemies.Visible = false;
                    EnemyPass();
                    SP2.SoundLocation = _EnemyPassBorderSoundLocation;
                    SP2.Play();
                }
            }          

                int bulletSpot = 0;
                foreach (PictureBox bullets in bulletlist) //handles bullet movement
                {
                        bullets.Location = new Point(bullets.Location.X + bulletspeed[bulletSpot], bullets.Location.Y);
                        bulletSpot++;
                }
                bulletSpot = 0;
                foreach (PictureBox bullets in bulletlist) //check for bullet collision
                {
                    foreach (PictureBox enemies in enemylist) //check for enemy collision
                    {
                        if (bullets.Top >= enemies.Top - 20 && bullets.Bottom <= enemies.Bottom + 20 && bullets.Right >= enemies.Left && bullets.Left <= enemies.Right) //detects collision
                        {
                            if (BulletSpeed != _Character5BulletSpeed)
                            {
                                bullets.Visible = false;
                            } //end nested if

                            enemies.Visible = false;
                            Hits++;
                            SP2.SoundLocation = _CharacterHitSoundLocation;
                            SP2.Play();
                            EnemyDifficultyChange();

                            Collision(reloadBoosterActivated);
                        } //end if
                    } //end enemy foreach
                    if (bullets.Location.X >= _formWidth) //if bullet leaves form
                    {
                        bullets.Visible = false;
                        if (bulletspeed[bulletSpot] == _Character4BulletSpeed && BulletSpeed == _Character4BulletSpeed)
                            colorGreen();
                    } //end if
                    bulletSpot++;
                } //end bullet foreach
                int bulletSpot2 = 0;
                for (int bulletIndex = 0; bulletIndex <= bulletlist.Count - 1; bulletIndex++)
                {
                    //check if any bullets in bulletlist .Visible = false, if true, then delete
                    if (bulletlist[bulletIndex].Visible == false)
                    {
                        bulletlist.RemoveAt(bulletIndex);
                        bulletspeed.RemoveAt(bulletSpot2);
                    }
                    bulletSpot2++;
                } //end for loop
                if (swing)
                {
                    if (swordPart)
                    {
                        picSword.Location = new Point(picSword.Location.X + _Character6BulletSpeed * swordMoveRate, picCharacter.Location.Y + 140);
                        if (picSword.Left >= picCharacter.Right + 60)
                            swordPart = false;
                    } else if (swordPart == false)
                    {
                        picSword.Location = new Point(picSword.Location.X - _Character6BulletSpeed * swordMoveRate, picCharacter.Location.Y + 140);
                        if (picSword.Left <= picCharacter.Right - 40)
                        {
                            swing = false;
                            swordPart = true;
                            if (autofire == false)
                                colorGreen();
                        } //end nested if
                    } //end else

                    foreach (PictureBox enemy in enemylist) //Handles enemy collision with sword
                    {
                        if (picSword.Right >= enemy.Left && picSword.Top >= enemy.Top && picSword.Bottom <= enemy.Bottom)
                        {
                            //collision
                            enemy.Visible = false;
                            Hits++;                            
                            EnemyDifficultyChange();

                            Collision(reloadBoosterActivated);
                        }
                    }

                } else
                    picSword.Location = new Point(picCharacter.Right - 40, picCharacter.Location.Y + 140);

            bool newEnemy = false;
            for (int enemyIndex = 0; enemyIndex <= enemylist.Count - 1; enemyIndex++)
            {
                //check if any enemies in enemylist .Visible = false, if true, then delete; also removes the corresponding enemy's speed
                if (enemylist[enemyIndex].Visible == false)
                {
                    newEnemy = true;
                    enemyspeed.RemoveAt(enemyIndex + enemyIndex + 1);
                    enemyspeed.RemoveAt(enemyIndex + enemyIndex);

                    enemylist.RemoveAt(enemyIndex);
                }
            }
            if (newEnemy)
                EnemyAction();

        } //end timer function
        public void EnemyAction()
        {
            int EnemyXPos = 1159;
            int EnemyYPos = 300; //bottom
            Random r = new Random();
            switch (r.Next(1, 4))
            {
                case 1: //top
                    EnemyYPos = 60;
                    break;
                case 2: //middle
                    EnemyYPos = 180;
                    break;
            } //end switch

            MakeNewEnemy(EnemyXPos, EnemyYPos);
        }
        public void MakeNewEnemy(int EnemyXPos, int EnemyYPos)
        {                           //EnemyXPos & EnemyYPos both aren't set to any values
            do
            {
                PictureBox newenemy = new PictureBox
                {
                    Name = "picEnemy",
                    Size = new System.Drawing.Size(75, 75),
                    Location = new System.Drawing.Point(EnemyXPos, EnemyYPos),
                    Image = Properties.Resources.catenemy1,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                this.Controls.Add(newenemy);
                newenemy.BringToFront();

                enemylist.Add(newenemy);

                Random r = new Random();
                enemyspeed.Add((r.Next(enemyMIN, enemyMAX + 1))); //difficulty edits enemy movement here
                if (r.Next(1, 3) == 1)
                    enemyspeed.Add(r.Next(enemyMIN, enemyMAX + 1));
                else
                    enemyspeed.Add((r.Next(enemyMIN, enemyMAX + 1)) * -1);
            } while (enemyAmount != enemylist.Count);
        } //end function
        public void EnemyDifficultyChange()
        {
            if (Hits >= 100)
            {
                enemyMAX = 25;
                enemyAmount = 5;
            }
            else if (Hits >= 85)
            {
                enemyAmount = 4;
            } else if (Hits >= 65)
            {
                enemyMIN = 15;
                enemyMAX = 22;
            } else if (Hits >= 50)
            {
                enemyMIN = 12;
                enemyMAX = 18;
            } else if (Hits >= 35)
            {
                enemyAmount = 3;
            } else if (Hits >= 20)
            {
                enemyMIN = 10;
                enemyMAX = 15;
            } else if (Hits >= 10)
            {
                enemyMIN = 8;
                enemyMAX = 12;
                enemyAmount = 2;
            }
        } //end function
        public void EnemyPass() //Handles enemy passing border actions
        {
            Health--;
            switch (Health)
            {
                case 4:
                    picHealth.Image = Properties.Resources.health_4_5_removebg;
                    break;
                case 3:
                    picHealth.Image = Properties.Resources.health_3_5_removebg;
                    break;
                case 2:
                    picHealth.Image = Properties.Resources.health_2_5_removebg;
                    break;
                case 1:
                    picHealth.Image = Properties.Resources.health_1_5_removebg;
                    break;
                case 0:
                    picHealth.Image = Properties.Resources.health_0_5_removebg;
                    timerMain.Enabled = false;
                    timerBullet.Enabled = false;
                    timerPowerUps.Enabled = false;
                    SP.Stop();
                    SP2.Stop();
                    double accuracy;
                    accuracy = (double)Hits / (double)ShotAccumulator;

                    string[] highscores = new string[5];

                    StreamReader sr = new StreamReader(_fileLocation3);

                    highscores[0] = sr.ReadLine();
                    highscores[1] = sr.ReadLine();
                    highscores[2] = sr.ReadLine();
                    highscores[3] = sr.ReadLine();
                    highscores[4] = sr.ReadLine();
                    sr.Close();

                        for (int accumulatorScore = 0; accumulatorScore <= 4; accumulatorScore++)
                        {
                        int temp3 = highscores[accumulatorScore].IndexOf("*") + 1;
                        string temp2 = highscores[accumulatorScore].Substring(temp3);
                            if (Convert.ToInt32(temp2) < Hits)
                            {
                                string temp1;
                                do
                                {
                                    temp1 = Microsoft.VisualBasic.Interaction.InputBox("Enter your name:", "Congrats, You've Gotten a Highscore!", "CatDestroyer99", -1, -1);
                                    if (temp1.IndexOf("*") != -1)
                                        MessageBox.Show("Ouch, trying to murder me I see. No * in your name!");
                                } while (temp1.IndexOf("*") != -1); //nested do-while loop
                                //MessageBox.Show("Accumulator: " + accumulator + Environment.NewLine + "Highscore message: " + highscores[accumulator]);

                                string[] temp = new string[4];
                                temp[0] = highscores[0];
                                temp[1] = highscores[1];
                                temp[2] = highscores[2];
                                temp[3] = highscores[3];
                                switch (accumulatorScore)
                                {
                                    case 0: //#1
                                        highscores[1] = temp[0];
                                        highscores[2] = temp[1];
                                        highscores[3] = temp[2];
                                        highscores[4] = temp[3];
                                        break;
                                    case 1: //#2
                                        highscores[2] = temp[1];
                                        highscores[3] = temp[2];
                                        highscores[4] = temp[3];
                                        break;
                                    case 2: //#3
                                        highscores[3] = temp[2];
                                        highscores[4] = temp[3];
                                        break;
                                    case 3: //#4
                                        highscores[4] = temp[3];
                                        break;
                                        //#5 handles itself below
                                }
                                highscores[accumulatorScore] = temp1 + " *" + Hits;
                                accumulatorScore = 5; //ends for loop
                            }
                        } //end for

                    StreamWriter sw = new StreamWriter(_fileLocation3);
                    sw.WriteLine(highscores[0]);
                    sw.WriteLine(highscores[1]);
                    sw.WriteLine(highscores[2]);
                    sw.WriteLine(highscores[3]);
                    sw.WriteLine(highscores[4]);
                    sw.Close();

                    DialogResult dialogResult = MessageBox.Show("You've hit: " + Hits.ToString("G") + " cats!" + Environment.NewLine + "You had an accuracy of: " + 
                        accuracy.ToString("P2") + Environment.NewLine + "Would you like to play again?" + Environment.NewLine                               //main end message
                        + Environment.NewLine + "Highscore #1: " + highscores[0] + Environment.NewLine + "Highscore #2: " + highscores[1] + Environment.NewLine + "Highscore #3: " + 
                        highscores[2] + Environment.NewLine + "Highscore #4: " + highscores[3] + Environment.NewLine + "Highscore #5: " + highscores[4]     //highscore message
                        , "GAME OVER!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                        Restart();
                    else
                        CloseForm();
                    break;
            } //end switch
        } //end function
        public void CloseForm()
        {
            StreamWriter sw = new StreamWriter(_fileLocation1);
            sw.WriteLine(1);
            sw.WriteLine(System.Boolean.TrueString);
            sw.WriteLine(1);
            sw.Close();
            StreamWriter sw2 = new StreamWriter(_fileLocation2);
            sw2.WriteLine();
            sw2.Close();
            System.Windows.Forms.Application.Exit();
        }
        public void Collision(bool reloadBooster)
        {
            if (bulletCount == 0)
            {
                timerBullet.Enabled = true;
            }
            if (BulletSpeed == _Character4BulletSpeed)
            {
                colorGreen();
            }
            if (reloadBooster == false)
                bulletCount++;
            lblBullets.Text = bulletCount.ToString();
        }
        private void colorGreen()
        {
            picCount1.BackColor = Color.Green;
            picCount2.BackColor = Color.Green;
            picCount3.BackColor = Color.Green;
            picCount4.BackColor = Color.Green;
            picCount5.BackColor = Color.Green;
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerMain.Enabled = false;
            timerPowerUps.Enabled = false;
            bool timer2enabled = false;
            if (timerBullet.Enabled == true)
            {
                timer2enabled = true;
                timerBullet.Enabled = false;
            }
            DialogResult dialogResult = MessageBox.Show("Do you really want to exit Cat Game?", "Exit?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                CloseForm();
            else
            {
                timerMain.Enabled = true;
                timerPowerUps.Enabled = true;
                if (timer2enabled)
                    timerBullet.Enabled = true;
            }
        }
        private void MnuRestart_Click(object sender, EventArgs e)
        {
            timerMain.Enabled = false;
            timerPowerUps.Enabled = false;
            bool timer2enabled = false;
            if (timerBullet.Enabled == true)
            {
                timer2enabled = true;
                timerBullet.Enabled = false;
            }
            DialogResult dialogResult = MessageBox.Show("Do you really want to restart Cat Game?", "Restart?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                Restart();
            else
            {
                timerMain.Enabled = true;
                timerPowerUps.Enabled = true;
                if (timer2enabled)
                    timerBullet.Enabled = true;
            }
        }
        public void ChangeCharacter(object sender, EventArgs args)
        {
            ChangeCharacterAgain(Convert.ToInt16((sender.ToString()).Substring(0, 1)));
        }
        public void ChangeCharacterAgain(int character)
        {
            bool random = false;
            do
            {
                random = false;
                switch (character)
                {
                    case 1:
                        BulletSpeed = _Character1BulletSpeed;
                        picCharacter.Image = Properties.Resources.awpcat2;
                        picCharacter.Height = 180;
                        picCharacter.Width = 254;
                        picCharacter.Location = new Point(0, 150);
                        cboCatSelect.SelectedIndex = 0;
                        BulletXPos = 170;
                        BulletYPos = 203;
                        bulletTimerInterval = 100;
                        picSword.Visible = false;
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\Sniper_Fire.wav";
                        break;
                    case 2:
                        BulletSpeed = _Character2BulletSpeed;
                        picCharacter.Image = Properties.Resources.catbananaguncut;
                        picCharacter.Height = 189;
                        picCharacter.Width = 224;
                        picCharacter.Location = new Point(0, 141);
                        cboCatSelect.SelectedIndex = 1;
                        BulletXPos = 0;
                        BulletYPos = 0;
                        bulletTimerInterval = 60;
                        picSword.Visible = false;
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\M9_Pow.wav";
                        break;
                    case 3:
                        BulletSpeed = _Character3BulletSpeed;
                        picCharacter.Image = Properties.Resources.catsmg;
                        picCharacter.Height = 235;
                        picCharacter.Width = 186;
                        picCharacter.Location = new Point(68, 161);
                        cboCatSelect.SelectedIndex = 2;
                        BulletXPos = 0;
                        BulletYPos = 0;
                        bulletTimerInterval = 35;
                        picSword.Visible = false;
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\Skorpion_Pow.wav";
                        break;
                    case 4:
                        BulletSpeed = _Character4BulletSpeed;
                        picCharacter.Image = Properties.Resources.rpgcat;
                        picCharacter.Height = 143;
                        picCharacter.Width = 202;
                        picCharacter.Location = new Point(0, 185);
                        cboCatSelect.SelectedIndex = 3;
                        BulletXPos = 180;
                        BulletYPos = 240;
                        bulletTimerInterval = 100;
                        picSword.Visible = false;
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\M4A1_Pow.wav";
                        break;
                    case 5:
                        BulletSpeed = _Character5BulletSpeed;
                        picCharacter.Image = Properties.Resources.screamcat;
                        picCharacter.Height = 187;
                        picCharacter.Width = 189;
                        picCharacter.Location = new Point(0, 150);
                        cboCatSelect.SelectedIndex = 4;
                        BulletXPos = 0;
                        BulletYPos = 0;
                        bulletTimerInterval = 150;
                        picSword.Visible = false;
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\Depth_Charge.wav";
                        break;
                    case 6:
                        BulletSpeed = _Character6BulletSpeed;
                        picCharacter.Image = Properties.Resources.SwordCat2;
                        picCharacter.Height = 175;
                        picCharacter.Width = 150;
                        picCharacter.Location = new Point(0, 150);
                        cboCatSelect.SelectedIndex = 5;
                        BulletXPos = 0;
                        BulletYPos = 0;
                        bulletTimerInterval = 150;
                        picSword.Visible = true;
                        picSword.Location = new Point(picCharacter.Right - 40, picCharacter.Location.Y + 140);
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\Sword_Swing.wav";
                        break;
                    case 7:
                        random = true;
                        Random r = new Random();
                        int check;
                        switch (BulletSpeed)
                        {
                            case _Character1BulletSpeed:
                                character = 1;
                                break;
                            case _Character2BulletSpeed:
                                character = 2;
                                break;
                            case _Character3BulletSpeed:
                                character = 3;
                                break;
                            case _Character4BulletSpeed:
                                character = 4;
                                break;
                            case _Character5BulletSpeed:
                                character = 5;
                                break;
                            case _Character6BulletSpeed:
                                character = 6;
                                break;
                        }
                        do
                        {
                            check = r.Next(1, 7);
                        } while (character == check);
                        character = check;
                        break;
                } //end switch
                if (timerBulletChanged && random == false)
                    timerBullet.Interval = bulletTimerInterval / 2;
                else
                    timerBullet.Interval = bulletTimerInterval;

                if (random == false)
                    colorGreen();

            } while (random);

            //foreach (PictureBox bullets in bulletlist)
            //    bullets.Visible = false;

            //bulletlist.Clear();
            MoveCheck = 0;
            WASD[0] = false;
            WASD[1] = false;
            WASD[2] = false;
            WASD[3] = false;
        }
        private void TimerIntro_Tick(object sender, EventArgs e) //Handles intro animation
        {
            switch (part)
            {
                case 0:
                    ChangeCharacterAgain(2);
                    menuStrip1.Visible = false;
                    part++;
                    btnD.BackColor = Color.Silver;
                    btnW.Visible = true;
                    btnS.Visible = true;
                    btnD.Visible = true;
                    btnA.Visible = true;
                    btnSpace.Visible = true;
                    lblAutoFire.Visible = false;
                    picSword.Image = Properties.Resources.bullet;
                    picSword.Height = 16;
                    picSword.Width = 45;
                    picSword.Location = new Point(picCharacter.Location.X + 215, picCharacter.Location.Y + 60);
                    SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\M9_Pow.wav";
                    break;
                case 1:
                    if (picCharacter.Right <= picBorder.Left)
                    picCharacter.Location = new Point(picCharacter.Location.X + 15, picCharacter.Location.Y);
                    else
                    {
                        btnD.BackColor = Color.Gainsboro;
                        btnA.BackColor = Color.Silver;
                        part++;
                    }
                    break;
                case 2:
                    if (picCharacter.Location.X >= 0)
                        picCharacter.Location = new Point(picCharacter.Location.X - 15, picCharacter.Location.Y);
                    else
                    {
                        btnA.BackColor = Color.Gainsboro;
                        btnW.BackColor = Color.Silver;
                        part++;
                    }
                    break;
                case 3:
                    if (picCharacter.Location.Y >= 0)
                        picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y - 15);
                    else
                    {
                        btnW.BackColor = Color.Gainsboro;
                        btnS.BackColor = Color.Silver;
                        part++;
                    }
                    break;
                case 4:
                    if (picCharacter.Location.Y <= 130)
                        picCharacter.Location = new Point(picCharacter.Location.X, picCharacter.Location.Y + 15);
                    else
                    {
                        btnS.BackColor = Color.Gainsboro;
                        btnSpace.BackColor = Color.Silver;
                        part++;
                        picSword.Visible = true;
                        picEnemy.Visible = true;
                        SP.Play();
                    }
                    break;
                case 5:
                    picSword.Location = new Point(picSword.Location.X + 50, picSword.Location.Y);
                    picEnemy.Location = new Point(picEnemy.Location.X - 15, picEnemy.Location.Y);
                    if (picSword.Left <= picEnemy.Right && picSword.Right >= picEnemy.Left)
                    {
                        SP2.SoundLocation = _CharacterHitSoundLocation;
                        SP2.Play();
                        btnSpace.BackColor = Color.Gainsboro;
                        picSword.Visible = false;
                        picEnemy.Visible = true;
                        picEnemy.Location = new Point(1159, 180);
                        Thread.Sleep(1000);
                        part++;
                    }
                    break;
                case 6:
                    if (picEnemy.Left >= picBorder.Right)
                        picEnemy.Location = new Point(picEnemy.Location.X - 25, picEnemy.Location.Y);
                    else if (oneTime && picEnemy.Left <= picBorder.Right + 50)
                    {
                        picHealth.Image = Properties.Resources.health_4_5_removebg;
                        oneTime = false;
                    }
                    else
                    {
                        picEnemy.Visible = false;
                        picCatGame.Visible = true;
                        lblCredits.Visible = true;
                        picCount1.Visible = false;
                        picCount2.Visible = false;
                        picCount3.Visible = false;
                        picCount4.Visible = false;
                        picCount5.Visible = false;
                        btnW.Visible = false;
                        btnS.Visible = false;
                        btnA.Visible = false;
                        btnD.Visible = false;
                        btnSpace.Visible = false;                       
                        picCharacter.Visible = false;
                        picBorder.Visible = false;
                        timerIntro.Interval = 50;
                        SP2.SoundLocation = _EnemyPassBorderSoundLocation;
                        SP2.Play();
                        part++;
                        SP.SoundLocation = "D:\\10-12th Grade\\Cat Game C Remake\\Sound Resources\\Sniper_Fire.wav";
                        Thread.Sleep(500);
                        picHealth.Visible = false;
                    }
                    break;
                case 7:
                    if (picCatGame.Location.X >= 351)
                        picCatGame.Location = new Point(picCatGame.Location.X - 16, picCatGame.Location.Y);
                    else
                    {
                        Thread.Sleep(3500);
                        picCatGame.Visible = false;
                        lblCredits.Visible = false;
                        picHealth.Image = Properties.Resources.health_5_5_removebg;
                        picCharacter.Visible = true;
                        picBorder.Visible = true;
                        picHealth.Visible = true;
                        ChangeCharacterAgain(1);
                        timerIntro.Enabled = false;
                        EnemyAction();
                        menuStrip1.Visible = true;
                        timerMain.Enabled = true;
                        lblAutoFire.Visible = true;
                        picCount1.Visible = true;
                        picCount2.Visible = true;
                        picCount3.Visible = true;
                        picCount4.Visible = true;
                        picCount5.Visible = true;
                        picSword.Image = Properties.Resources.Sword1;
                        picSword.Height = 27;
                        picSword.Width = 104;
                        timerPowerUps.Enabled = true;
                    }
                    break;
            }
        }
        private void FrmCatGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                CloseForm();
        }
        public void GetBulletPos()
        {
            int charX = picCharacter.Location.X;
            int charY = picCharacter.Location.Y;
            switch (BulletSpeed)
            {
                case _Character1BulletSpeed:
                    BulletXPos = 170;
                    BulletYPos = 203;
                break;
                case _Character2BulletSpeed:
                    BulletXPos = charX + 215;
                    BulletYPos = charY + 60;
                    break;
                case _Character3BulletSpeed:
                    BulletXPos = charX + 110;
                    BulletYPos = charY + 45;
                    break;
                case _Character4BulletSpeed:
                    BulletXPos = 180;
                    BulletYPos = 240;
                    break;
                case _Character5BulletSpeed:
                    BulletXPos = charX + 135;
                    BulletYPos = charY + 120;
                    break;
                case _Character6BulletSpeed:
                    BulletXPos = charX + 170;
                    BulletYPos = charY + 35;
                    break;
            }
        }
        public void MakeNewBullet(bool reloadBooster)
        {
            SP.Play();
            GetBulletPos();
            if (reloadBooster == false)
                bulletCount --;
            lblBullets.Text = bulletCount.ToString();
            ShotAccumulator++;

            bulletspeed.Add(BulletSpeed);

            MoveCheck = 0;
            WASD[0] = false;
            WASD[1] = false;
            WASD[2] = false;
            WASD[3] = false;

            picCount1.BackColor = Color.Red;              //prepares timer2 and picCount1-5
            picCount2.BackColor = Color.Red;
            picCount3.BackColor = Color.Red;
            picCount4.BackColor = Color.Red;
            picCount5.BackColor = Color.Red;
            if (BulletSpeed != _Character4BulletSpeed)
            timerBullet.Enabled = true;

            if (BulletSpeed == _Character1BulletSpeed || BulletSpeed == _Character2BulletSpeed)
            {
                PictureBox newbullet = new PictureBox
                {
                    Name = "picBullet",
                    Size = new System.Drawing.Size(45, 16),
                    Location = new System.Drawing.Point(BulletXPos, BulletYPos),
                    Image = Properties.Resources.bullet,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                this.Controls.Add(newbullet);
                newbullet.BringToFront();

                bulletlist.Add(newbullet);
            } else if (BulletSpeed == _Character3BulletSpeed)
            {
                PictureBox newbullet = new PictureBox
                {
                    Name = "picBullet",
                    Size = new System.Drawing.Size(45, 16),
                    Location = new System.Drawing.Point(BulletXPos, BulletYPos),
                    Image = Properties.Resources.smgbullet,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                this.Controls.Add(newbullet);
                newbullet.BringToFront();

                bulletlist.Add(newbullet);
            } else if (BulletSpeed == _Character4BulletSpeed)
            {
                PictureBox newbullet = new PictureBox
                {
                    Name = "picBullet",
                    Size = new System.Drawing.Size(40, 20),
                    Location = new System.Drawing.Point(BulletXPos, BulletYPos),
                    Image = Properties.Resources.rpgbullet,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                this.Controls.Add(newbullet);
                newbullet.BringToFront();

                bulletlist.Add(newbullet);
            } else if (BulletSpeed == _Character5BulletSpeed)
            {
                PictureBox newbullet = new PictureBox
                {
                    Name = "picBullet",
                    Size = new System.Drawing.Size(75, 45),
                    Location = new System.Drawing.Point(BulletXPos, BulletYPos),
                    Image = Properties.Resources.picklebullet,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                this.Controls.Add(newbullet);
                newbullet.BringToFront();

                bulletlist.Add(newbullet);
            }

        }
        private void ChangeDifficultyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changedifficulty.Show();
            timerMain.Enabled = false;
            timerPowerUps.Enabled = false;
            if (timerBullet.Enabled)
                timerBulletEnabled = true;
            timerDifficulty.Enabled = true;
        }
        private void TimerDifficulty_Tick(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(_fileLocation2);
            string value1 = sr.ReadLine();
            sr.Close();
            if (value1 != "")
            {
                if (difficultyChange != difficulty)
                {
                    DifficultyConversion();
                } //end nested if
                StreamWriter sw = new StreamWriter(_fileLocation2);
                sw.WriteLine();
                sw.Close();
                timerMain.Enabled = true;
                timerPowerUps.Enabled = true;
                if (timerBulletEnabled)
                    timerBullet.Enabled = true;
                timerBulletEnabled = false;
            } //end if
            difficultyChange = difficulty;

        } //end function
        public void DifficultyConversion()
        {
            int value = 10;

            if (difficulty == 0.6)
                value = 1;
            else if (difficulty == 0.7)
                value = 2;
            else if (difficulty == 0.8)
                value = 3;
            else if (difficulty == 0.9)
                value = 4;
            else if (difficulty == 1)
                value = 5;
            else if (difficulty == 1.2)
                value = 6;
            else if (difficulty == 1.4)
                value = 7;
            else if (difficulty == 1.6)
                value = 8;
            else if (difficulty == 1.8)
                value = 9;
            else
                value = 10;

            mnuCurrent.Text = "Currently: " + value + "/10";
        }
        private void TimerPowerUps_Tick(object sender, EventArgs e)
        {
            if (powerUpTimer >= 12)
            {
                int rando;
                Random r = new Random();
                do
                {
                    rando = r.Next(1, 5);
                } while (Health == 5 && rando == 2);
                newPowerUp(Convert.ToByte(rando));
                powerUpTimer = 0;
            }
            powerUpTimer += 0.15;
            if (powerUpTimer2 >= 0.15)
            {
                lblPowerUpsCountdown.Text = (10 - powerUpTimer2).ToString("N1");
                powerUpTimer2 += 0.15;
                if (powerUpTimer2 >= 10)
                {
                    powerUpTimer2 = 0;
                    foreach (PictureBox power in powerList)
                    {
                        if (tag == 3)
                        {
                            reloadBoosterActivated = false;
                            timerBulletChanged = false;
                            timerBullet.Interval = bulletTimerInterval;
                            swordMoveRate = 1;
                            lblPowerUp.Visible = false;
                            lblPowerUpsCountdown.Visible = false;
                        }
                        else if (tag == 4)
                        {
                            enemyMoveRate = 1;
                            lblPowerUp.Visible = false;
                            lblPowerUpsCountdown.Visible = false;
                        }

                    } //end foreach
                } //end nested if
            } //end if

            foreach (PictureBox power in powerList)
            {
                power.Location = new Point(power.Location.X - 25, power.Location.Y);
                if (power.Left <= picCharacter.Right/* && power.Right >= picCharacter.Left*/ && power.Top >= picCharacter.Top && power.Bottom <= picCharacter.Bottom) //power ups collision
                {
                    switch (Convert.ToInt32(power.Tag))
                    {
                        case 1:
                            bulletCount += 10;
                            lblBullets.Text = bulletCount.ToString();
                            break;
                        case 2:
                            if (Health != 5)
                                Health++;

                            switch (Health)
                            {
                                case 2:
                                    picHealth.Image = Properties.Resources.health_2_5_removebg;
                                    break;
                                case 3:
                                    picHealth.Image = Properties.Resources.health_3_5_removebg;
                                    break;
                                case 4:
                                    picHealth.Image = Properties.Resources.health_4_5_removebg;
                                    break;
                                case 5:
                                    picHealth.Image = Properties.Resources.health_5_5_removebg;
                                    break;
                            }
                            break;
                        case 3:
                            timerBullet.Interval = bulletTimerInterval / 2;
                            timerBulletChanged = true;
                            swordMoveRate = 2;
                            powerUpTimer2 = 0.15;
                            lblPowerUp.Text = "Fast Reload; Infinite Bullets";
                            reloadBoosterActivated = true;
                            lblPowerUp.Visible = true;
                            lblPowerUpsCountdown.Visible = true;
                            break;
                        case 4:
                            enemyMoveRate = 0.5;
                            powerUpTimer2 = 0.15;
                            lblPowerUp.Text = "Slower Enemies";
                            lblPowerUp.Visible = true;
                            lblPowerUpsCountdown.Visible = true;
                            break;
                    } //end switch
                    SP2.SoundLocation = _PowerUpCollectedSoundLocation;
                    SP2.Play();
                    power.Visible = false;
                }
                else if (power.Right <= 0) //leaves form
                    power.Visible = false;
            } //end foreach

            for (int powerIndex = 0; powerIndex <= powerList.Count - 1; powerIndex++)
            {
                //check if any bullets in bulletlist .Visible = false, if true, then delete
                if (powerList[powerIndex].Visible == false)
                {
                    tag = Convert.ToInt32(powerList[powerIndex].Tag);
                    powerList.RemoveAt(powerIndex);
                }
            } //end for loop

        } //end function

        public void newPowerUp(byte pcase)
        {
            Random r = new Random();
            
            if (pcase == 1) //ammunition box
            {
                PictureBox newPower = new PictureBox
                {
                    Name = "picPowerUp",
                    Size = new System.Drawing.Size(30, 30),
                    Location = new System.Drawing.Point(_formWidth, r.Next(50, _formHeight - 50)),
                    Image = Properties.Resources.Ammunition_Box,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = pcase,
                };
                this.Controls.Add(newPower);
                newPower.BringToFront();
                powerList.Add(newPower);
            } else if (pcase == 2) //extra health
            {
                PictureBox newPower = new PictureBox
                {
                    Name = "picPowerUp",
                    Size = new System.Drawing.Size(30, 30),
                    Location = new System.Drawing.Point(_formWidth, r.Next(50, _formHeight - 50)),
                    Image = Properties.Resources.Medikiyt,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = pcase,
                };
                this.Controls.Add(newPower);
                newPower.BringToFront();
                powerList.Add(newPower);
            } else if (pcase == 3) //fast reload
            {
                PictureBox newPower = new PictureBox
                {
                    Name = "picPowerUp",
                    Size = new System.Drawing.Size(30, 30),
                    Location = new System.Drawing.Point(_formWidth, r.Next(50, _formHeight - 50)),
                    Image = Properties.Resources.Reload,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = pcase,
                };
                this.Controls.Add(newPower);
                newPower.BringToFront();
                powerList.Add(newPower);
            }
            else //slower enemies
            {
                PictureBox newPower = new PictureBox
                {
                    Name = "picPowerUp",
                    Size = new System.Drawing.Size(30, 30),
                    Location = new System.Drawing.Point(_formWidth, r.Next(50, _formHeight - 50)),
                    Image = Properties.Resources.IcePowerup,
                    BackColor = System.Drawing.Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = pcase,
                };
                this.Controls.Add(newPower);
                newPower.BringToFront();
                powerList.Add(newPower);
            }


        } //end function

        private void lblPowerUp_Click(object sender, EventArgs e)
        {

        }

        private void lblPowerUpsCountdown_Click(object sender, EventArgs e)
        {

        }

        private void picCount5_Click(object sender, EventArgs e)
        {

        }

        private void picCount4_Click(object sender, EventArgs e)
        {

        }

        private void picCount3_Click(object sender, EventArgs e)
        {

        }

        private void picCount2_Click(object sender, EventArgs e)
        {

        }

        private void lblBullets_Click(object sender, EventArgs e)
        {

        }

        private void lblBulletInfo_Click(object sender, EventArgs e)
        {

        }

        public void Restart()
        {
            StreamWriter sw = new StreamWriter(_fileLocation1);
            switch (BulletSpeed)
            {
                case _Character1BulletSpeed:
                    sw.WriteLine("1");
                    break;
                case _Character2BulletSpeed:
                    sw.WriteLine("2");
                    break;
                case _Character3BulletSpeed:
                    sw.WriteLine("3");
                    break;
                case _Character4BulletSpeed:
                    sw.WriteLine("4");
                    break;
                case _Character5BulletSpeed:
                    sw.WriteLine("5");
                    break;
                case _Character6BulletSpeed:
                    sw.WriteLine("6");
                    break;
            }
            sw.WriteLine(System.Boolean.FalseString);
            sw.WriteLine(difficulty);
            sw.Close();

            System.Windows.Forms.Application.Restart();
        } //end Restart function

        public static void DifficultyChange(double newDifficulty)
        {
            StreamReader sr = new StreamReader(_fileLocation1);
            string value1 = sr.ReadLine();
            string value2 = sr.ReadLine();
            sr.Close();
            difficulty = newDifficulty;
            StreamWriter sw = new StreamWriter(_fileLocation1);
            sw.WriteLine(value1);
            sw.WriteLine(value2);
            sw.WriteLine(newDifficulty);
            sw.Close();
        }

    } //end class
} //end namespace