using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.Others.WindowsImpersonationContext
{
    public class ImpersonationContext
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);


        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token.
        const int LOGON32_LOGON_INTERACTIVE = 2;

        public SafeTokenHandle Handle { private set; get; }

        private ImpersonationContext(string userName, string password, string domain)
        {
            SafeTokenHandle handle;
            bool ok = LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out handle);
            if (ok)
            {
                Handle = handle;
                return;
            }

            int ret = Marshal.GetLastWin32Error();
            string msg = $"LogonUser failed with error code : {ret}";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }

        public static void Use(string userName, string password, string domain, Action action)
        {
            ImpersonationContext context = new ImpersonationContext(userName, password, domain);
            using (SafeTokenHandle handle = context.Handle)
            {
                using (WindowsIdentity newId = new WindowsIdentity(handle.DangerousGetHandle()))
                {
                    using (System.Security.Principal.WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                    {
                        Console.WriteLine("After impersonation: " + WindowsIdentity.GetCurrent().Name);
                        action();
                    }
                }
            }
        }

        public static void CopyFiles(string[] sourceFiles, string destinationDirectoryPath, string userName, string password, string domain = null)
        {
            foreach (string sourceFilePath in sourceFiles)
            {
                FileInfo fileInfo = new FileInfo(sourceFilePath);
                string targetFilePath = Path.Combine(destinationDirectoryPath, fileInfo.Name);
                CopyFile(sourceFilePath, targetFilePath, userName, password, domain);
            }
        }

        public static void CopyFile(string sourceFilePath, string destinationFilePath, string userName, string password, string domain = null)
        {
            CopyData(File.ReadAllBytes(sourceFilePath), destinationFilePath, userName, password, domain);
        }

        public static void CopyData(byte[] data, string destinationFilePath, string userName, string password, string domain = null)
        {
            ImpersonationContext.Use(userName, password, domain, () =>
            {
                File.WriteAllBytes(destinationFilePath, data);
            });
        }
    }
}
