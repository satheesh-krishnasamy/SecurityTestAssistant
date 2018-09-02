namespace SecurityTestAssistant.Library.Utils
{
    using SecurityTestAssistant.Library.Net;
    using System;
    using System.Collections.Generic;

    public static class CookieParser
    {
        public static IList<HttpCookie> ParseRequestCookie(string cookieHeaderValule)
        {
            var cookiesExtracted = new List<HttpCookie>();
            if (!string.IsNullOrWhiteSpace(cookieHeaderValule))
            {
                var cookies = cookieHeaderValule.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cookieKeyValueJoinedString in cookies)
                {
                    var cookieKeyValueArray = cookieKeyValueJoinedString.Split(new char[] { '=' });
                    var cookieObj = new HttpCookie();
                    cookieObj.Name = cookieKeyValueArray[0];
                    cookieObj.Value = cookieKeyValueArray[1];
                    cookiesExtracted.Add(cookieObj);
                }
            }
            return cookiesExtracted;
        }

        /// <summary>
        /// Parses the response cookie.
        /// </summary>
        /// <param name="setCookieHeaderValue">The set cookie header value.</param>
        /// <returns>List of parsed http cookie</returns>
        public static IList<HttpCookie> ParseResponseCookie(IList<string> setCookieHeaderValue)
        {
            var cookiesExtracted = new List<HttpCookie>();
            if (setCookieHeaderValue != null && setCookieHeaderValue.Count > 0)
            {
                foreach (var cookieString in setCookieHeaderValue)
                {
                    var splittedCookie = cookieString.Split(new char[] { ';' }, StringSplitOptions.None);
                    foreach (var element in splittedCookie)
                    {
                        var cookieObj = ParseFrom(element);

                        if (cookieObj != null &&
                        !(string.IsNullOrWhiteSpace(cookieObj.Name)
                        && string.IsNullOrWhiteSpace(cookieObj.Value)))
                        {
                            cookiesExtracted.Add(cookieObj);
                        }
                    }
                }
            }
            return cookiesExtracted;
        }

        private static HttpCookie ParseFrom(string cookieString)
        {
            HttpCookie cookieObj = null;
            var splittedKeyValues = cookieString.Split(new char[] { '=' });

            if (splittedKeyValues[0] != null)
            {
                cookieObj = new HttpCookie();

                var key = splittedKeyValues[0].Trim();

                switch (key.ToLower())
                {
                    case "expires":
                        if (!string.IsNullOrWhiteSpace(splittedKeyValues[1]))
                        {
                            cookieObj.LifeTime = DateTime.Parse(splittedKeyValues[1].Trim());
                        }
                        break;
                    case "path":
                        cookieObj.Path = splittedKeyValues[1].Trim();
                        break;
                    case "samesite":
                        cookieObj.SameSite = splittedKeyValues[1].Trim();
                        break;
                    case "httponly":
                        cookieObj.HttpOnly = true;
                        break;
                    case "secure":
                        cookieObj.IsSecure = true;
                        break;
                    case "max-age":
                        cookieObj.MaxAge = splittedKeyValues[1].Trim();
                        break;
                    case "domain":
                        cookieObj.MaxAge = splittedKeyValues[1].Trim();
                        break;
                    case "lang":
                        cookieObj.Language = splittedKeyValues[1].Trim();
                        break;
                    default:
                        cookieObj.Name = key;
                        cookieObj.Value = splittedKeyValues[1].Trim();
                        break;

                }
            }

            return cookieObj;
        }
    }
}