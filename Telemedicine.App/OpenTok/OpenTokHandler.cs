using OpenTokSDK;
using System;
using System.Collections.Generic;
using System.Text;
using Telemedicine.Utilities;

namespace Telemedicine.App
{
    public static class OpenTokHandler
    {
        public static string CreateSession(string ipAddress)
        {
            OpenTok o = new OpenTok(TokboxConfigurations.API_KEY, TokboxConfigurations.SECRET);
            return o.CreateSession(ipAddress, MediaMode.ROUTED).Id;
        }
        public static string GenerateToken(string sessionId)
        {
            OpenTok o = new OpenTok(TokboxConfigurations.API_KEY, TokboxConfigurations.SECRET);
            double inOneDay = (DateTime.UtcNow.Add(TimeSpan.FromDays(1)).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return o.GenerateToken(sessionId, role: Role.PUBLISHER, expireTime: inOneDay, data: "0");
        }
    }
}
