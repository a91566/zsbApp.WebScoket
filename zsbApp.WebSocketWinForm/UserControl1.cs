/*
 * 2018年7月3日 20:48:12 郑少宝
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.WebSockets;
using System.Threading;

namespace zsbApp.WebSocketWinForm
{
    public partial class UserControl1 : UserControl
    {
        private string serverUrl;

        private ClientWebSocket ws;

        public UserControl1(string url, int index)
        {
            InitializeComponent();
            this.txbUser.Text = $"{index}{index}{index}";
            this.serverUrl = url;
            this.init();
            this.btnOnline.PerformClick();
        }

        private async void init()
        {
            this.ws = new ClientWebSocket();
            await this.ws.ConnectAsync(new Uri(this.serverUrl), CancellationToken.None);
            while (ws.State == WebSocketState.Open)
            {
                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                this.appendLog(Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count));
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.send(this.txbSendContent.Text.Trim());
        }

        private void appendLog(string text)
        {
            this.Invoke(new Action(()=> { this.textBox2.AppendText($"{text}{System.Environment.NewLine}"); }));
        }

        private async void send(string text)
        {
            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(text));
            await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void btnOnline_Click(object sender, EventArgs e)
        {
            if (this.ws.State == WebSocketState.Closed)
            {
                this.init();
            }
            if (this.ws.State == WebSocketState.Open)
            {
                this.setEnabled(false);
                this.send(this.txbUser.Text.Trim());
            }
        }

        private void setEnabled(bool b)
        {
            this.btnOnline.Enabled = b;
            this.btnOffline.Enabled = !b;
            this.btnSend.Enabled = !b;
            this.txbSendContent.Enabled = !b;
        }

        private async void btnOffline_Click(object sender, EventArgs e)
        {
            await this.ws.CloseAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
            this.setEnabled(true);
            this.appendLog("您已下线");
        }
    }
}
