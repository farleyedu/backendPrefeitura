using System;

namespace PortalTCMSP.Api.Swagger
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ApiGroupsAttribute : Attribute
    {
        public string[] Groups { get; }
        public ApiGroupsAttribute(params string[] groups) => Groups = groups ?? Array.Empty<string>();
    }
}
