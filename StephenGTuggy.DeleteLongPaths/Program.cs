/**
 * Copyright (C) 2015 Stephen G. Tuggy (sgt@StephenGTuggy.com).
 * 
 * This file is part of StephenGTuggy.DeleteLongPaths.
 * 
 * StephenGTuggy.DeleteLongPaths is free software: you can redistribute it 
 * and/or modify it under the terms of the GNU General Public License as 
 * published by the Free Software Foundation, either version 3 of the License, 
 * or (at your option) any later version.
 *
 * StephenGTuggy.DeleteLongPaths is distributed in the hope that it will be 
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General 
 * Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with 
 * StephenGTuggy.DeleteLongPaths.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;

namespace StephenGTuggy.DeleteLongPaths
{
    public class Program
    {
        public const string DefaultWildcard = "*.*";
        private const System.IO.FileAttributes FileAttributes1 = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
        private const System.IO.FileAttributes FileAttributes2 = ~FileAttributes1;
        private static readonly IComparer<Alphaleonis.Win32.Filesystem.FileSystemInfo> FileSystemInfoComparer = new FileSystemInfoComparer();

        public static void Main(string[] args)
        {
            if ((args.Length != 3))
            {
                WriteHelpMessage();
                return;
            }
            else
            {
                string actuallyDeleteStr = args[0];
                bool actuallyDeleteBool = false;
                if (!bool.TryParse(actuallyDeleteStr, out actuallyDeleteBool))
                {
                    WriteHelpMessage();
                    return;
                }
                else
                {
                    string wildcard = args[1];
                    string rootPathToDelete = args[2];
                    var rootDirectoryToDelete = new Alphaleonis.Win32.Filesystem.DirectoryInfo(rootPathToDelete, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);
                    Delete(rootDirectoryToDelete, wildcard, false, actuallyDeleteBool);
                }
            }
        }

        private static void WriteHelpMessage()
        {
            Console.WriteLine("Usage: StephenGTuggy.DeleteLongPaths <Actually Delete? (true or false)> <wildcard> <path>");
        }

        public static void Delete(Alphaleonis.Win32.Filesystem.DirectoryInfo directoryToDelete, string wildcard, bool thisDirKnownToMatchWildcard, bool actuallyDelete)
        {
            SortedSet<Alphaleonis.Win32.Filesystem.FileSystemInfo> fsis1 = null;
            SortedSet<Alphaleonis.Win32.Filesystem.FileSystemInfo> fsis2 = null;
            if (!directoryToDelete.Exists)
            {
                Console.WriteLine(string.Format("Directory \"{0}\" does not exist, or an error occurred trying to find it.", directoryToDelete.FullName));
            }
            else
            {
                // First, recursively delete this dir's child dirs and files that match the wildcard.
                try
                {
                    fsis1 = new SortedSet<Alphaleonis.Win32.Filesystem.FileSystemInfo>(directoryToDelete.GetFileSystemInfos(wildcard, System.IO.SearchOption.TopDirectoryOnly), FileSystemInfoComparer);
                    foreach (var fsi1 in fsis1)
                    {
                        if (fsi1 is Alphaleonis.Win32.Filesystem.DirectoryInfo)
                        {
                            var di1 = (Alphaleonis.Win32.Filesystem.DirectoryInfo)fsi1;
                            Delete(di1, wildcard, true, actuallyDelete);
                        }
                        else
                        {
                            var fi1 = (Alphaleonis.Win32.Filesystem.FileInfo)fsi1;
                            try
                            {
                                if (actuallyDelete)
                                {
                                    fi1.Attributes &= FileAttributes2;
                                    fi1.Delete();
                                }
                                Console.WriteLine(string.Format("Deleted file \"{0}\".", fi1.FullName));
                            }
                            catch (System.IO.IOException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (System.Security.SecurityException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (System.Security.SecurityException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Then, if this dir matches the wildcard itself, recursively delete everything else inside it.
                if (thisDirKnownToMatchWildcard)
                {
                    try
                    {
                        fsis2 = new SortedSet<Alphaleonis.Win32.Filesystem.FileSystemInfo>(directoryToDelete.GetFileSystemInfos(), FileSystemInfoComparer);
                        fsis2.ExceptWith(fsis1);
                        foreach (var fsi2 in fsis2)
                        {
                            if (fsi2 is Alphaleonis.Win32.Filesystem.DirectoryInfo)
                            {
                                var di2 = (Alphaleonis.Win32.Filesystem.DirectoryInfo)fsi2;
                                Delete(di2, wildcard, true, actuallyDelete);
                            }
                            else
                            {
                                var fi2 = (Alphaleonis.Win32.Filesystem.FileInfo)fsi2;
                                try
                                {
                                    if (actuallyDelete)
                                    {
                                        fi2.Attributes &= FileAttributes2;
                                        fi2.Delete();
                                    }
                                    Console.WriteLine(string.Format("Deleted file \"{0}\".", fi2.FullName));
                                }
                                catch (System.IO.IOException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch (System.Security.SecurityException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    // Finally, if this dir matches the wildcard, delete the directory itself.
                    try
                    {
                        if (actuallyDelete)
                        {
                            directoryToDelete.Attributes &= FileAttributes2;
                            directoryToDelete.Delete(false);
                        }
                        Console.WriteLine(string.Format("Deleted directory \"{0}\".", directoryToDelete.FullName));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.IO.IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        var dis3 = new SortedSet<Alphaleonis.Win32.Filesystem.FileSystemInfo>(directoryToDelete.GetDirectories(), FileSystemInfoComparer);
                        dis3.ExceptWith(fsis1);
                        foreach (var di in dis3)
                        {
                            Delete((Alphaleonis.Win32.Filesystem.DirectoryInfo)di, wildcard, false, actuallyDelete);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
