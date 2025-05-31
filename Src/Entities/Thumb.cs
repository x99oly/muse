
namespace Muse.Src.Entities
{
    public class Thumb
    {
        public string Url { get; }

        public string Width { get; }

        public string Heigth { get; }

        public Thumb(string url, string width, string heigth)
        {
            Url = url;
            Width = width;
            Heigth = heigth;
        }
        public override string ToString()
        {
            return $"Url: {Url}, Width: {Width}, Height: {Heigth}";
        }

    }
}