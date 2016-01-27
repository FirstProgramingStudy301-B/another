using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncForm
{
    //全般的には.Net3.5定義済みデリゲート型とラムダ式を使用しています。
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Threadを使用した方法
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Text = "Waite 3Sec";
            this.button1.Enabled = false;
            var thr = new Thread(() =>
            {
                Thread.Sleep(3000);
                this.BeginInvoke((Action)(() =>
                {
                    this.button1.Enabled = true;
                    this.button1.Text = "3秒超過";
                }));
            });
            thr.Start();
        }
        //ThreadPoolを使用した方法　
        //パフォーマンスの観点からこちらを使用すべきである。
        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Text = "Waite 3Sec";
            this.button2.Enabled = false;
            //ThreadPoolを利用しています。
            //本来コールバックをラムダ式Action<object>で表現していますが
            //引数を使用しないためアンダースコアを利用しています。
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(3000);
                this.BeginInvoke((Action)(() =>
                {
                    this.button2.Enabled = true;
                    this.button2.Text = "3秒超過";
                }));
            },null);
            
        }

        #region .N20から有効なEAP(Event Async Pattern)

        private readonly BackgroundWorker worker = new BackgroundWorker();
        private void SetButton3Event()
        {
            this.worker.DoWork -= asyncDoWork;
            this.worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
            this.worker.DoWork += asyncDoWork;
            this.worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }
        //.Net2.0で取り込まれたBackGroundWorkerを使用します。
        private void button3_Click(object sender, EventArgs e)
        {
            SetButton3Event();
            this.button3.Enabled = false;
            this.worker.RunWorkerAsync();//非同期処理開始　Worker_DoWork()起動
        }
        //UI Thread処理
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (string)e.Result;//結果を受け取る
            this.button3.Text = result;
            this.button3.Enabled = true;
        }
        //非同期処理
        private void asyncDoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            e.Result = "3Sec";　//結果を渡す。
        }

        #endregion
        //ThreadPoolで非同期処理実施し戻り値が使用するパターン
        //APM（Asynchronous Programming Model）
        private void button4_Click(object sender, EventArgs e)
        {
            this.button4.Enabled = false;
            var ayncMethod = new Func<string>(() =>
            {
                Thread.Sleep(3000);
                return "3秒経過";
            });
            ayncMethod.BeginInvoke(ar => 
            {
                var res = ayncMethod.EndInvoke(ar);
                this.BeginInvoke((Action)(()=>
                {
                    this.button4.Enabled = true;
                    this.button4.Text = res;
                }));
            },null);
        }

        //TaskBase Asynchronous Pattern .Net4.0
        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            Task.Factory.StartNew(() => Thread.Sleep(3000))
                .ContinueWith(_ => //UI Threadで処理される。引数のTaskSchedulerがみそ
                {
                    this.button5.Enabled = true;
                    this.button5.Text = "経過";
                },TaskScheduler.FromCurrentSynchronizationContext());
        }
        //aysnc/await for .Net4.5
        private async  void button6_Click(object sender, EventArgs e)
        {
            this.button6.Text = "開始";
            this.button6.Enabled = false;
            await Task.Run(() => Thread.Sleep(3000));
            this.button6.Text = "終了";
            this.button6.Enabled = true;
        }


        //.Net3.5から標準ライブラリーとしてReactive Extensions（Rx）が省略
        //Linqと同等のメソッドチェーンで記述できる。今回は割愛

       

    }
}
