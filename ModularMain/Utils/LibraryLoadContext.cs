using System;
using System.Reflection;
using System.Runtime.Loader;

namespace ModularMain.Utils
{
    public class LibraryLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver fResolver;

        public LibraryLoadContext(string BinFolder)
        {
            fResolver = new AssemblyDependencyResolver(BinFolder);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = fResolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string FilePath = fResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (FilePath != null)
            {
                return LoadUnmanagedDllFromPath(FilePath);
            }

            return IntPtr.Zero;
        }
    }
}
