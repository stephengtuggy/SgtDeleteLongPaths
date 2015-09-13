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
    public class FileSystemInfoComparer : Comparer<Alphaleonis.Win32.Filesystem.FileSystemInfo>
    {
        public override int Compare(Alphaleonis.Win32.Filesystem.FileSystemInfo x, Alphaleonis.Win32.Filesystem.FileSystemInfo y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }
            else if ((x == null) && (y != null))
            {
                return -1;
            }
            else if ((y == null) && (x != null))
            {
                return 1;
            }
            else
            {
                return StringComparer.Ordinal.Compare(x.FullName, y.FullName);
            }
        }
    }
}
