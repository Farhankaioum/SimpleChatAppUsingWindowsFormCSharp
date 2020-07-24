using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using ChatApp.Security;
using System.Security.Cryptography;

namespace ChatApp
{
    public partial class Form1 : Form
    {
        Socket sck;
        EndPoint epLocal, epRemote;
        byte[] buffer;

        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = Properties.Resources.projectpic;
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            //setup socket
             sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //get user ip
            textLocalIp.Text = GetLocalIp();
            textLocalPort.Text = GetLocalIp();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            //binding socket
            epLocal = new IPEndPoint(IPAddress.Parse(textLocalIp.Text), Convert.ToInt32(textLocalPort.Text));
            sck.Bind(epLocal);

            //connecting to remote ip
            epRemote = new IPEndPoint(IPAddress.Parse(textRemoteIp.Text), Convert.ToInt32(textRemotePort.Text));
            sck.Connect(epRemote);

            //listening the specif port
            buffer = new byte[1500];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(Messagecallback), buffer);
        }

        private string GetLocalIp()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress iP in host.AddressList)
            {
                if (iP.AddressFamily == AddressFamily.InterNetwork)
                    return iP.ToString();
            }
            return "192.68.191.0";
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            //convert string message to byte[]
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEncoding.GetBytes(textMessage.Text);

            // For Encrypted
            var encryptedValue = AESSecurity.doEncryptAES(textMessage.Text, "keiuom");
            sendingMessage = aEncoding.GetBytes(encryptedValue);
            
            //sending the encoded message
            sck.Send(sendingMessage);

            //adding to the listbox
            listMessage.Items.Add("Me:" + textMessage.Text);
            textMessage.Text = "";

        }


        private void Messagecallback(IAsyncResult aResult)
        {
            try
            {
                byte[] receivedData = new byte[120];
                receivedData = (byte[])aResult.AsyncState;

                //converting byte[] to string
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                var receivedValue = aEncoding.GetString(receivedData);
                var receive = receivedValue.Split('\0');
                var receiveMessage = receive[0];


                // decrypted data
                var decryptedMessage = AESSecurity.doDecryptAES(receiveMessage, "keiuom");
               
                //adding message into listbox
                listMessage.Items.Add("Friend:" + decryptedMessage);
                buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(Messagecallback), buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
