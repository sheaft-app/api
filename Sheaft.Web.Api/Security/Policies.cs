namespace Sheaft.Web.Api
{
    public static class Policies
    {
        public const string ANONYMOUS_OR_CONNECTED = "Anything";
        public const string AUTHENTICATED = "Authenticated";
        public const string REGISTERED = "Registered";
        public const string UNREGISTERED = "Unregistered";
        public const string STORE_OR_PRODUCER = "StoreOrProducer";
        public const string STORE_OR_CONSUMER = "StoreOrConsumer";
        public const string OWNER = "Owner";
        public const string PRODUCER = "Producer";
        public const string STORE = "Store";
        public const string CONSUMER = "Consumer";
        public const string HANGFIRE = "Hangfire";
    }
}
