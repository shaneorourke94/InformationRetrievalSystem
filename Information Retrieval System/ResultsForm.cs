using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Information_Retrieval_System
{
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueryForm.finalResults.Clear();
            QueryForm.timerResult = 0;
            //load query form
            var queryForm = new QueryForm();
            queryForm.Show();
            this.Hide();
        }
    }
}
