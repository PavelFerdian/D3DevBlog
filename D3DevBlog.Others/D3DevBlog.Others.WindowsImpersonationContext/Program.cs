using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.Others.WindowsImpersonationContext
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFile.txt");
            ImpersonationContext.CopyFile(sourceFilePath, @"\\TestServer\Share\TestFile.txt", "User1", "Password1");
        }
    }
}
