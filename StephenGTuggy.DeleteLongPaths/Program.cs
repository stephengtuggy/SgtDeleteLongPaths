/**
 * Copyright (C) 2015 Stephen G. Tuggy (sgt@StephenGTuggy.com).
 * 
 * This program is free software: you can redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published by the Free 
 * Software Foundation, either version 3 of the License, or (at your option) 
 * any later version.
 *
 * StephenGTuggy.FuzzyMatch is distributed in the hope that it will be useful, 
 * but WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General 
 * Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with 
 * StephenGTuggy.FuzzyMatch.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace StephenGTuggy.DeleteLongPaths
{
    public class Program
    {
        public const string DefaultWildcard = "*.*";
        private const System.IO.FileAttributes FileAttributes1 = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
        private const System.IO.FileAttributes FileAttributes2 = ~FileAttributes1;

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
            if (!directoryToDelete.Exists)
            {
                Console.WriteLine(string.Format("Directory \"{0}\" does not exist, or an error occurred trying to find it.", directoryToDelete.FullName));
            }
            else
            {
                // First, recursively delete this dir's child dirs and files that match the wildcard.
                try
                {
                    foreach (var fsi in directoryToDelete.GetFileSystemInfos(wildcard, System.IO.SearchOption.TopDirectoryOnly))
                    {
                        if (fsi is Alphaleonis.Win32.Filesystem.DirectoryInfo)
                        {
                            var di = (Alphaleonis.Win32.Filesystem.DirectoryInfo)fsi;
                            Delete(di, wildcard, true, actuallyDelete);
                        }
                        else
                        {
                            var fi = (Alphaleonis.Win32.Filesystem.FileInfo)fsi;
                            try
                            {
                                if (actuallyDelete)
                                {
                                    fi.Attributes &= FileAttributes2;
                                    fi.Delete();
                                }
                                Console.WriteLine(string.Format("Deleted file \"{0}\".", fi.FullName));
                            }
                            catch (System.IO.IOException ex)
                            {
                                Console.WriteLine(ex);
                            }
                            catch (System.Security.SecurityException ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                catch (System.Security.SecurityException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                // Then, if this dir matches the wildcard itself, recursively delete everything else inside it.
                if (thisDirKnownToMatchWildcard)
                {
                    try
                    {
                        foreach (var fsi2 in directoryToDelete.GetFileSystemInfos())
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
                                    Console.WriteLine(ex);
                                }
                                catch (System.Security.SecurityException ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine(ex.ToString());
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
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.IO.IOException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    try
                    {
                        foreach (var di in directoryToDelete.GetDirectories())
                        {
                            Delete(di, wildcard, false, actuallyDelete);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
