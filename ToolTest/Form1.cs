using CodeDesignUtilities;
using System;
using System.Windows.Forms;

namespace ToolTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = "tungvv";
            string token = CryptoUtils.Encode(text);
            text = CryptoUtils.Decode(token);
        }
    }
}
