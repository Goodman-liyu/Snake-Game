using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 谢幕大作
{
    public partial class Form1 : Form
    {
        #region 属性
        Label[,] data;//存储方法
        int rows;//行的数量
        int cols;//列的数量
        int fwidth = 20;
        int fheight = 20;
        Color background = Color.AliceBlue;//方块的背景颜色
        List<Label> snake = new List<Label>();//不要忘记new初始化,建立label类型的顺序表，snake
        Color sbg = Color.Pink;//🐍身颜色
        direction dir;//表示蛇运动的方向
        Label head;//头
        int scoal = 0;
        Label food = new Label();
        Label xj = new Label();
        Color xjc = Color.Black;
        Color foodcolor = Color.Gold;
        bool isfood;
        int x; int y;
        int c = 1;
        #endregion
        public Form1()
        {
            this.Height = 917;
            this.Width = 1500;
            this.BackColor = Color.Yellow;
            InitializeComponent();
        }
        void init()//初始化方块
        {
            rows = this.panel1.Height / fheight;
            cols = this.panel1.Width / fwidth;
            data = new Label[rows, cols];//确定data的大小
            // 给方块的数组进行初始
            for (int i = 0; i < rows; i++)//i在Y轴上0，1，2，3
            {
                for (int j = 0; j < cols; j++)//j在x轴上0，1，2，3
                {
                    this.timer1.Enabled = true;//计时开始

                    data[i, j] = new Label();//为实体化数组中的每个元素
                    data[i, j].Size = new Size(fwidth, fheight);
                    data[i, j].Location = new Point( j*fwidth,i*fheight);
                    data[i, j].BackColor = background; 
                     data[i, j].Tag = new zb(i,j);//为标签新增加了坐标属性
                   data[i, j].BorderStyle = BorderStyle.FixedSingle;//为每个格子添加边框
                    this.panel1.Controls.Add(data[i, j]);
                }
            }
        }
        void initsnake()
        {   for (int i = 0; i < 10; i++)
                addsnake(data[1, i]);
        }
        void addsnake(Label lbl)
        {
            head = lbl;//头部等于每次新添加的
         
            lbl.BackColor = sbg;//改变颜色
            lbl.Image= Image.FromFile(@"D:\C#\谢幕大作\12.jpg");
            snake.Add(lbl);//向顺序列表中添加元素
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }//窗体初始化
        private void timer1_Tick(object sender, EventArgs e)//每隔一个时间间隔做的事情
        {
            move(); 
            if (!isfood)
            {
                creatfood();

            }
            if (sheshen()) gg();
            if (((zb)head.Tag).x == ((zb)xj.Tag).x && ((zb)head.Tag).y == ((zb)xj.Tag).y)
            { gg(); }
            label1.Text = string.Format("此时的速度为：{0}", 250-timer1.Interval);
            label2.Text= string.Format("得分：{0}",scoal);
         

        }
        void remove()//去尾的方法
        {
            snake[0].BackColor = background;//单链表
            snake[0].Image = null;
            snake.RemoveAt(0);

        }
        Random rand=new Random();//生成随机数的类
        void creatxj(ref int x,ref int y)
        {
           x= rand.Next(rows - 1);
            y=rand.Next(cols - 1);
            data[x, y].BackColor = xjc;
            xj = data[x, y];
        }
        void creatfood()
        {
            bool xkk = true;
            int r = rand.Next(rows - 1);
            int l = rand.Next(rows - 1);
            foreach (Label rp in snake)
            {
                if (((zb)rp.Tag).x == r&& l==((zb)rp.Tag).y) xkk = false;
            }
            while ((r == x&&l==y) || !xkk)
            {
                r = rand.Next(rows - 1);
                l = rand.Next(rows - 1);
                xkk = true;
                foreach (Label rp in snake)
                {
                    if (((zb)rp.Tag).x == r && l == ((zb)rp.Tag).y) xkk = false;
                }
            }
            data[r, l].BackColor = foodcolor;
            food = data[r, l];
            isfood = true;//
        }
       bool sheshen()
        { int i = 0;
            foreach (Label rp in snake)
            {
                if (((zb)head.Tag).x==((zb)rp.Tag).x&& ((zb)head.Tag).y == ((zb)rp.Tag).y)
                    i++;
            }
            if(i==2)  return true;
            else return false;
          
        }
        void gg()
        {
            this.panel1.Controls.Clear();
            this.panel1.BackgroundImage =Image.FromFile(@"D:\C#\谢幕大作\gg.jpg");
            this.panel1.BackgroundImageLayout = ImageLayout.Stretch;
            timer1.Enabled = false;




            Label haha = new Label();
            haha.Text = "Game Over";
            haha.Width = this.panel1.Width;
            haha.Height = this.panel1.Height;
            haha.ForeColor = Color.Black;//前景色，字体颜色
            haha.Font = new Font("宋体", 28, FontStyle.Strikeout);
            haha.Location = new Point(200, 200);
            Label ff = new Label();
            ff.Text = string.Format("此时的速度为：{0}", 220-Convert.ToDouble(timer1.Interval));
            ff.Width = this.panel1.Width;
            ff.Height = this.panel1.Height;
            ff.ForeColor = Color.Black;//前景色，字体颜色
            ff.Font = new Font("宋体", 28, FontStyle.Strikeout);
            ff.Location = new Point(30, 30);
          //this.panel1.Controls.Add(ff);
           // this.panel1.Controls.Add(haha);
            
        }
       bool chi()
        {
            if (isfood==true&&((zb)head.Tag).x == ((zb)food.Tag).x && ((zb)head.Tag).y == ((zb)food.Tag).y)
            {    
                scoal = scoal + 5*c;
                c++;
                if (timer1.Interval > 50)
                { timer1.Interval = timer1.Interval - 10; }
        
                return true;
            }
            else return false;

        }
        void move()
        {
            //1.判断方向
            if (dir == direction.right)
            {
                //根据列的关系去判断是否撞墙了
                if (((zb)head.Tag).y < cols - 1)//
                {
                    addsnake(data[((zb)head.Tag).x, ((zb)head.Tag).y + 1]);//添头
                    if (!chi())
                    {
                        remove();
                    }

                    else
                    {
                        isfood = false;
                    }
                }
                else
                {
                    gg();
                }

            }
            else if (dir == direction.left)
            {
                if (((zb)head.Tag).y > 0)
                {
                    addsnake(data[((zb)head.Tag).x, ((zb)head.Tag).y - 1]);//添头
                    if (!chi())
                    {
                        remove();
                    }
                    else
                    {
                        isfood = false;
                    }
                }
                else
                {
                    gg();
                }

            }
            else if (dir == direction.up)
            {
                if (((zb)head.Tag).x > 0)
                {
                    addsnake(data[((zb)head.Tag).x - 1, ((zb)head.Tag).y]);//添头
                    if (!chi())
                    {
                        remove();
                    }
                    else
                    {
                        isfood = false;
                    }

                }
                else
                {
                    gg();
                }


            }
            else if (dir == direction.down)
            {
                if(((zb)head.Tag).x<rows-1)
                {
                    addsnake(data[((zb)head.Tag).x +1, ((zb)head.Tag).y]);//添头
                    if (!chi())
                    {
                        remove();
                    }
                    else
                    {
                        isfood = false;
                    }
                }
                else
                {
                    gg();
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData==Keys.Up&& dir!=direction.down)
            {
                dir = direction.up;
            }
            else if(e.KeyData==Keys.Down&&dir!=direction.up)
             {

                dir = direction.down;

            }
            else if(e.KeyData==Keys.Left&& dir != direction.right)
            {
                dir = direction.left;

            }
            else if (e.KeyData == Keys.Right&& dir != direction.left)
            {
                dir = direction.right;
               

            }
           else if(e.KeyData==Keys.W)
            {
                if (timer1.Interval > 50)
                    timer1.Interval = timer1.Interval - 10;
                else if (timer1.Interval <= 50 && timer1.Interval >= 30) timer1.Interval = timer1.Interval - 10;
                else timer1.Interval = timer1.Interval - 5;
            }
            else if (e.KeyData == Keys.Space)
            {if (timer1.Enabled == true)
                    timer1.Enabled = false;
                else timer1.Enabled = true;
            }
        }  
        private void button1_Click_1(object sender, EventArgs e)
        {//初始化游戏背景
            init();
            //初始化运动方向
            dir = direction.right;
            //初始化蛇身
           initsnake();
            //初始化没有食物
        isfood = false;
            //初始化陷阱
           creatxj(ref x, ref y);
            button1.Enabled = false;

            richTextBox1.Text = "Welcome to Greedy Snake!\n使用↑↓←→来控制移动方向\n按下L对增加移动速度\n按下空格可暂停游戏\n黄色方块为食物\n黑色方块为陷阱\n速度越快，分数越高\n快来体验吧！";
               

            richTextBox1.Font = new Font("楷书", 9, FontStyle.Underline);
            richTextBox1.ForeColor = Color.Orange;
            richTextBox1.Enabled = false;
        }
    }
    public enum direction
    {
        up,down,left,right
    }
    public struct zb
    {
        public int x;
        public int y;
        public zb(int x,int y)
        { this.x = x;
            this.y = y;
        }         

    }
}