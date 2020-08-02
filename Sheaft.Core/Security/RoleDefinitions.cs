namespace Sheaft.Core.Security
{
    public static class RoleNames
    {
        public const string ADMIN = "ADMIN";
        public const string OWNER = "OWNER";
        public const string USER = "USER";
        public const string STORE = "STORE";
        public const string PRODUCER = "PRODUCER";
        public const string CONSUMER = "CONSUMER";
        public const string SUPPORT = "SUPPORT";
        public const string ANONYMOUS = "ANONYMOUS";
    }

    public static class RoleIds
    {
        public const string ADMIN = "b7f40811-679a-4a8d-92c9-2524426b24a5";
        public const string SUPPORT = "0734a31f-f363-445f-8af8-68f2fa4fa92f";
        public const string PRODUCER = "e4a370fe-659b-47a8-a1d4-d5d6600abe7d";
        public const string USER = "3b40b6d5-846e-4296-a480-a5800c460c24";
        public const string CONSUMER = "a3f75e3b-ae1c-483c-814a-89634cbb0ebb";
        public const string OWNER = "2e87eb43-6884-4515-b5d7-60b2d5dd4b91";
        public const string STORE = "f81030de-18db-40d1-8ed2-4c98ccb70a99";
    }

    public static class Policies
    {
        public const string API = "Api";
        public const string AUTHENTICATED = "Authenticated";
        public const string REGISTERED = "Registered";
        public const string UNREGISTERED = "Unregistered";
        public const string STORE_OR_PRODUCER = "StoreOrProducer";
        public const string OWNER = "Owner";
        public const string PRODUCER = "Producer";
        public const string STORE = "Store";
        public const string CONSUMER = "Consumer";
    }
}
