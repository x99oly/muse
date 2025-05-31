using Muse.Src.Interfaces;
using Muse.Src.Entities;

namespace Muse.Src.Handlers
{
    public class Identifier
    {
        private readonly ILog _logger;

        public Identifier(ILog logger)
        {
            _logger = logger;
        }

        public int GetRandomIntId(int length)
        {
            length = Math.Min(length, 9);

            int min = (int)Math.Pow(10, length - 1);
            int max = (int)Math.Pow(10, length) - 1;

            int id = new Random().Next(min, max + 1);

            return id;
        }
    }
}
