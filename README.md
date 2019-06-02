# SgtDeleteLongPaths

SgtDeleteLongPaths is an **advanced delete utility** to delete files and folders on Windows. 

This program handles files and folders with **long paths** (paths longer than 260 characters). It allows you to **preview** exactly what will be deleted before you actually delete it. SgtDeleteLongPaths will delete **hidden files** and folders. And it will **keep going** and delete everything it can, even if a few items can't be deleted.

To see the command-line syntax, run StephenGTuggy.DeleteLongPaths with no arguments. (This will not delete anything.)

Special thanks to the AlphaFS project (https://github.com/alphaleonis/AlphaFS) for the long path support.

## Getting Started / Prerequisites

To get started, you will need **Windows 7 64-bit** or later. You will also need **Visual Studio 2019 Community Edition** or better. .NET Framework 4.7.2 is also required. (The installer will install this for you, if necessary.)

Clone the repo to your local computer using Git on the command line, or the GitHub Desktop application or similar. Double-click StephenGTuggy.DeleteLongPaths.sln to open it in Visual Studio.

Alternatively, open Visual Studio first; go to Team Explorer; use the GUI provided to connect to GitHub; clone SgtDeleteLongPaths that way; and then double-click StephenGTuggy.DeleteLongPaths.sln to open it.

SgtDeleteLongPaths / StephenGTuggy.DeleteLongPaths requires AlphaFS, but Visual Studio will download and install this automatically when you build the project.

## Building

To build SgtDeleteLongPaths / StephenGTuggy.DeleteLongPaths, simply choose the Debug|Any CPU or Release|Any CPU configuration, and select Build Solution from the Build menu. The resulting .exe file will be called StephenGTuggy.DeleteLongPaths.exe, and it will end up either under StephenGTuggy.DeleteLongPaths\bin\Debug or under StephenGTuggy.DeleteLongPaths\bin\Release, depending on which build configuration you chose.

## Running

To run the program, open a command prompt window, such as cmd.exe or Windows PowerShell. Then enter the path to the .exe file that was built in the last step.

You can run the program with no arguments to see the command-line syntax. Or you can use the following arguments:

```
StephenGTuggy.DeleteLongPaths <Actually Delete? (true or false)> <wildcard> <path>
```

For example, on my machine, to preview deleting all files and directories under C:\Windows\Temp, including in subdirectories, I would run:

```
C:\Dev\Repos\GitHub\SgtDeleteLongPaths\StephenGTuggy.DeleteLongPaths\bin\Release\StephenGTuggy.DeleteLongPaths.exe false * C:\Windows\Temp
```

And to actually delete the same set of files and directories, I would run the same command, except that I would change "false" to "true":

```
C:\Dev\Repos\GitHub\SgtDeleteLongPaths\StephenGTuggy.DeleteLongPaths\bin\Release\StephenGTuggy.DeleteLongPaths.exe true * C:\Windows\Temp
```

## Contributing

Contribute? Why would you want to contribute anything? My code is perfect! ;-)

Seriously, pull requests are welcome. Just please try to follow the coding conventions you see in my code.

## Conclusion

That's it! Let me know if you have any further questions.
