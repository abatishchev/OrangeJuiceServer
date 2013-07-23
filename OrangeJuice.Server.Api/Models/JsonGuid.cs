using System;

namespace OrangeJuice.Server.Api.Models
{
    public struct JsonGuid
    {
        public readonly Guid Value;

        public JsonGuid(Guid guid)
        {
            Value = guid;
        }

        public JsonGuid(string str)
            : this(Guid.Parse(str))
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static explicit operator JsonGuid(Guid guid)
        {
            return new JsonGuid(guid);
        }

        public static explicit operator Guid(JsonGuid guid)
        {
            return guid.Value;
        }

        public static explicit operator JsonGuid(string str)
        {
            return new JsonGuid(Guid.Parse(str));
        }

        public static explicit operator string(JsonGuid guid)
        {
            return guid.ToString();
        }
    }
}