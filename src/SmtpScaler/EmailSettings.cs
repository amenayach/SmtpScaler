﻿namespace SmtpScaler
{
    public class EmailSettings
    {
        public string Sender
        {
            get;
            set;
        }
        public string ServerIp
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public bool EnableSsl
        {
            get;
            set;
        }
    }
}