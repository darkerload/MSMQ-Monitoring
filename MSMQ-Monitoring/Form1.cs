using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Windows.Forms;

namespace MSMQ_Monitoring
{
    public partial class Form1 : Form
    {
        List<System.Messaging.Message> messageList = new List<System.Messaging.Message>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text;
            messageList.Clear();
            using (MessageQueue messageQueue = new MessageQueue(path))
                messageList.AddRange(messageQueue.GetAllMessages());
            FillList();
        }

        void FillList()
        {
            lstMessage.Items.Clear();
            foreach (var message in messageList)
                lstMessage.Items.Add(message.Id);
        }

        private void lstMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string messageId = lstMessage.GetItemText(lstMessage.SelectedItem);
            var getMessage = messageList.FirstOrDefault(c => c.Id == messageId);
            getMessage.Formatter = new XmlMessageFormatter(new String[] { "System.String, mscorlib" });
            txtMessageRead.Text = getMessage.Body.ToString();
            lblMessageId.Text = getMessage.Id;
            lblMessageLabel.Text = getMessage.Label;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string path = txtMessagePath.Text;
            bool isPathExist = MessageQueue.Exists(path);
            if (!isPathExist)
                MessageQueue.Create(path);
            CreateMessage();
        }

        void CreateMessage()
        {
            var queue = new MessageQueue(txtPath.Text);
            queue.Send(txtMessage.Text, txtLabel.Text);
        }
    }
}
