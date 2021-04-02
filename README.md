# OpPDir32
Command-Line-Tool Path-Dir

Lists the Path-Environment-Variable

Search for a File (Program) in the Path-List

# Sample
    C:\>OpPDir32
    OpPDir v:1.0.16.1221.R
    ----------------------------------------------------------------------
    Current = C:\
    PATH    = C:\WINDOWS\system32;C:\WINDOWS;C:\S
    Cnt Len = Path
    ----------------------------------------------------------------------
      0   3 = C:\ (=CurDir)
      1  19 = C:\WINDOWS\system32
      2  10 = C:\WINDOWS
      3   4 = C:\S
    ----------------------------------------------------------------------

Search for cmd.exe

    C:\>OpPDir32 cmd.exe
    OpPDir v:1.0.16.1221.R
    ----------------------------------------------------------------------
    Search for cmd.exe
    ----------------------------------------------------------------------
      1 C:\WINDOWS\system32
                             1 29.10.2014 02:05:25         315 392 cmd.exe
    ----------------------------------------------------------------------
                             1 Files found                 315 392

Search for osql.exe

    C:\>oppdir32 osql.exe
    OpPDir v:1.0.16.1221.R
    -----------------------------------------------------------------------
    Search for osql.exe
    -----------------------------------------------------------------------
    25 C:\Program Files\Microsoft SQL Server\140\Tools\Binn\
                             1 22.08.2017 18:48:04          75 448 OSQL.EXE
    -----------------------------------------------------------------------
                             1 Files found                  75 448
