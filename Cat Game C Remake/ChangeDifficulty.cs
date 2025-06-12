using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Cat_Game_C_Remake
{
    public partial class frmChangeDifficulty : Form
    {
        public frmChangeDifficulty()
        {
            InitializeComponent();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            ProgressBar1.Value = TrackBar1.Value;
            lblDifficulty.Text = "Change Difficulty (currently " + TrackBar1.Value + ")";
        } //end function

        private void BtnSetDifficulty_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("D:\\10-12th Grade\\Cat Game C Remake\\ProgramData2.txt");
            sw.WriteLine(0);
            sw.Close();

            double difficulty = 2.0;
            switch (TrackBar1.Value)
            {
                case 1:
                    difficulty = 0.6;
                    break;
                case 2:
                    difficulty = 0.7;
                    break;
                case 3:
                    difficulty = 0.8;
                    break;
                case 4:
                    difficulty = 0.9;
                    break;
                case 5:
                    difficulty =  1.0;
                    break;
                case 6:
                    difficulty = 1.2;
                    break;
                case 7:
                    difficulty = 1.4;
                    break;
                case 8:
                    difficulty = 1.6;
                    break;
                case 9:
                    difficulty = 1.8;
                    break;
            } //end switch
            frmCatGame.DifficultyChange(difficulty);

            frmChangeDifficulty.ActiveForm.Visible = false;
        } //end function

        private void FrmChangeDifficulty_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("D:\\10-12th Grade\\Cat Game C Remake\\ProgramData.txt");
            sr.ReadLine();
            sr.ReadLine();
            int value = 10;

            double difficultyConvert = Convert.ToDouble(sr.ReadLine());
            sr.Close();
            if (difficultyConvert == 0.6)
                value = 1;
            else if (difficultyConvert == 0.7)
                value = 2;
            else if (difficultyConvert == 0.8)
                value = 3;
            else if (difficultyConvert == 0.9)
                value = 4;
            else if (difficultyConvert == 1)
                value = 5;
            else if (difficultyConvert == 1.2)
                value = 6;
            else if (difficultyConvert == 1.4)
                value = 7;
            else if (difficultyConvert == 1.6)
                value = 8;
            else if (difficultyConvert == 1.8)
                value = 9;

            ProgressBar1.Value = value;
            TrackBar1.Value = value;
            lblDifficulty.Text = "Change Difficulty (currently " + TrackBar1.Value + ")";
        } //end function

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("D:\\10th Grade\\Cat Game C Remake\\ProgramData2.txt");
            sw.WriteLine(0);
            sw.Close();
            frmChangeDifficulty.ActiveForm.Visible = false;
        }
    } //end class
} //end namespace