using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ConestogaVirtualGameStore.Classes
{
    public static class SessionHelper
    {
        public static T GetSessionObject<T>(this ISession session,  string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetSessionObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
