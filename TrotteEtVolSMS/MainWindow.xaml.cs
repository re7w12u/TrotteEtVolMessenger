﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace TrotteEtVolSMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region fields
        RecipientsModel recipients;
        HistoryModel history;
        bool messageSaved;
        #endregion

        #region properties
        private string _selectAllBtnContent;
        public string SelectAllBtnContent
        {
            get
            {
                return _selectAllBtnContent;
            }
            set
            {
                _selectAllBtnContent = value;
                OnPropertyChanged("SelectAllBtnContent");
            }
        }

        private string _characterCount;
        public string CharacterCount {
            get
            {
                return _characterCount;
            }
            set
            {
                _characterCount = value;
                OnPropertyChanged("CharacterCount");
            }
        }
        #endregion

        #region INotifyPropertyChanged 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
               
        public MainWindow()
        {
            InitializeComponent();

            CheckGateway();

            recipients = new RecipientsModel();
            RecipientListBox.DataContext = recipients;
            history = new HistoryModel();
            HistoryListBox.DataContext = history;
            SelectAllBtn.DataContext = this;
            SelectAllBtnContent = "Tout Sélectionner";
            CharacterCountLabel.DataContext = this;
            CharacterCount = String.Empty;

        }

        // send SMS Message
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            List<Recipient> recipients = RecipientListBox.SelectedItems.Cast<Recipient>().ToList();
            string numbers = String.Join(";", recipients.Select(x => x.Phone));

            string ip = ConfigurationManager.AppSettings.Get("ip");
            string port = ConfigurationManager.AppSettings.Get("port");
            string message = MessageBox.Text;
            string url = String.Format("http://{0}:{1}/send.html?smsto={2}&smsbody={3}&smstype=sms",
                ip,
                port,
                numbers,
                message);
            WebRequest request = WebRequest.Create(url);
            request.Method = "get";
            try
            {
                //WebResponse response = request.GetResponse();
                messageSaved = history.SaveMessage(new Message { Recipients = recipients, Body = message, SendDate = DateTime.Now });
            }
            catch (Exception)
            {
                // do something usefull
            }

        }

        // display message selected in history list
        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            RecipientListBox.SelectedItems.Clear();

            Message msg = HistoryListBox.SelectedItem as Message;
            MessageBox.Text = msg.Body;

            IEnumerable<string> phoneList = msg.Recipients.Select(x => x.Phone);
            IEnumerable<Recipient> allRecipients = RecipientListBox.Items.Cast<Recipient>();
            foreach (Recipient r in allRecipients)
            {
                if (phoneList.Contains(r.Phone))
                {
                    RecipientListBox.SelectedItems.Add(r);
                }
            }
            SelectAllBtnContent = "Désélectionner tout";
            MessageBox_KeyUp(sender, null);
        }

        // select all recipients
        private void SelectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RecipientListBox.SelectedItems.Count > 0)
            {
                SelectAllBtnContent = "Tout sélectionner";
                RecipientListBox.SelectedItems.Clear();
            }
            else
            {
                SelectAllBtnContent = "Désélectionner tout";
                RecipientListBox.SelectAll();
            }
        }

        private void MessageBox_KeyUp(object sender, KeyEventArgs e)
        {
            int max = 160;
            int count = MessageBox.Text.Count();
            int smsCount = count / max;
            smsCount++;
            int leftover = count % max;
            int remaining = max - leftover;

            CharacterCount = String.Format("{0}/160 ({1})", remaining, smsCount);
        }

        private void CheckGateway()
        {
            string ip = ConfigurationManager.AppSettings.Get("ip");
            IpLabel.Content = ip;

            string port = ConfigurationManager.AppSettings.Get("port");
            string message = MessageBox.Text;
            Ping ping = new Ping();
            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = ping.Send(ip, timeout, buffer, new PingOptions());
            if (reply.Status == IPStatus.Success)
            {
                PingResult.Content = String.Format("TTL:{0} | RoundTrip:{1}",reply.Options.Ttl, reply.RoundtripTime);
                PingState.Source = new BitmapImage(new Uri(@"/images/ok.png", UriKind.Relative));
            }
            else
            {
                PingResult.Content = String.Empty;
                PingState.Source = new BitmapImage(new Uri(@"/images/ko.png", UriKind.Relative));
            }
        }
    }
}
