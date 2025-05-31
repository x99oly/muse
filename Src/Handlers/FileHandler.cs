using Muse.Src.Interfaces;
using Muse.Src.Entities;

namespace Muse.Src.Handlers
{
    public class FileHandler
    {
        private readonly string _rootDir;
        public string PublicDir { get; }

        public FileHandler()
        {
            _rootDir = GoToRootDirectory();
            PublicDir = Path.Combine(_rootDir, "public");
        }

        private string GetAbsolutePath(string relativePathFromPublic)
        {
            var combined = Path.Combine(PublicDir, relativePathFromPublic);

            var fullPath = Path.GetFullPath(combined);
            if (!fullPath.StartsWith(PublicDir))
                throw new UnauthorizedAccessException("Access outside public directory is not allowed.");

            return fullPath;
        }

        public void SaveFile(string relativePathFromPublic, string content, bool overwrite)
        {
            string absolutePath = GetAbsolutePath(relativePathFromPublic);
            string? directory = Path.GetDirectoryName(absolutePath);

            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (overwrite || !File.Exists(absolutePath))
            {
                File.WriteAllText(absolutePath, content);
            }
            else
            {
                File.AppendAllText(absolutePath, content + Environment.NewLine);
            }
        }

        // Mesma ideia para os outros métodos — usar caminhos relativos + GetAbsolutePath para montar absoluto

        public void DeleteFile(string relativePathFromPublic)
        {
            string absolutePath = GetAbsolutePath(relativePathFromPublic);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
            else
            {
                throw new FileNotFoundException($"File not found at: {absolutePath}");
            }
        }

        public void CreateDirectory(string relativePathFromPublic)
        {
            string absolutePath = GetAbsolutePath(relativePathFromPublic);

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
        }

        public void DeleteDirectory(string relativePathFromPublic)
        {
            string absolutePath = GetAbsolutePath(relativePathFromPublic);

            if (Directory.Exists(absolutePath))
            {
                Directory.Delete(absolutePath, recursive: true);
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory not found at: {absolutePath}");
            }
        }

        // Função recursiva para buscar o root, com limite e exceção
        private string GoToRootDirectory()
        {
            int count = 10;
            return GoToRootDirectory(Directory.GetCurrentDirectory(), count);
        }

        private string GoToRootDirectory(string dir, int count)
        {
            string absolutPath = Path.GetFullPath(dir);
            if (File.Exists(Path.Combine(absolutPath, ".project.root")))
                return absolutPath;

            if (count < 0)
                throw new FileNotFoundException("The current project doesn't contain the root file.");

            return GoToRootDirectory(Path.Combine(dir, ".."), count - 1);
        }
    }

}
