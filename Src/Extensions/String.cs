
namespace Muse.Src.Extensions
{
    static class StringExtension
    {
        public static string SanitizeFileName(this string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}