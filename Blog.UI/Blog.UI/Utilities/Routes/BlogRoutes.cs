namespace Blog.UI.Utilities.Routes
{
    public class BlogRoutes
    {
        public const string BlogList = "/blog";
        public const string BlogPage = "/blog/page/{page:int?}";

        //   تگ
        public const string Tag = "/tag/{TagSlug}";
        public const string TagPage = "/tag/{TagSlug}/page/{page:int?}";

        //   دسته‌بندی
        public const string Category = "/category/{categorySlug}";
        public const string CategoryPage = "/category/{categorySlug}/page/{page:int?}";

        // صفحه جستجو
        public const string Search = "/search";
        public const string SearchPage = "/search/page/{page:int?}";
    }
}
