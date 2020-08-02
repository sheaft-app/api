﻿namespace Sheaft.Options
{
    public class DatabaseOptions
    {
        public const string SETTING = "Database";
        public string Url { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get => string.Format(Url, Server, Port, Name, User, Password); }
    }
}
