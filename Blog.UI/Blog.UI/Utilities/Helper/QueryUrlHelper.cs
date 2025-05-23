using Microsoft.AspNetCore.Components;
using System;

namespace Blog.UI.Utilities.Helper
{
    public static class QueryUrlHelper
    {
        public static string BuildPageUrl(NavigationManager navigation, string baseUrl, int pageNumber)
        {
            var baseUri = navigation.BaseUri.TrimEnd('/');

            var relativePath = pageNumber <= 1 ? baseUrl : $"{baseUrl}/page/{pageNumber}";
            if (!relativePath.StartsWith("/"))
                relativePath = "/" + relativePath;


            var fullUrl = baseUri + relativePath;

            var currentUri = new Uri(navigation.Uri);
            var rawQuery = currentUri.Query.StartsWith("?")
                           ? currentUri.Query.Substring(1)
                           : currentUri.Query;
            var queryCollection = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(rawQuery);
            var queryDict = new Dictionary<string, string>();

            foreach (var kvp in queryCollection)
            {
                if (string.Equals(kvp.Key, "page", StringComparison.OrdinalIgnoreCase))
                    continue;
                queryDict[kvp.Key] = kvp.Value.FirstOrDefault() ?? "";
            }

            if (queryDict.Any())
            {
                fullUrl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(fullUrl, queryDict);
            }
            return fullUrl;
        }
    }
}
