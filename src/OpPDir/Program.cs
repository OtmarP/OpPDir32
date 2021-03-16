using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpPDir
{
    class Program
    {
        const int _LEN_PATH = 3;
        const int _LEN_P_LEN = 3;
        const int _LEN_FILE = 4;

        public static string VersionNumber = "";

        static void Main( string[] args )
        {
            // History
            // -------------------------------------------------
            // Mi.21.12.2016 09:41:37 -op- "1.0.16.1221" weiter
            // Di.20.12.2016 14:33:00 -op- "1.0.16.1220" Cr. (C:\Users\otmar.pilgerstorfer\Documents\Visual Studio 2015\Projects\OpPDir)
            // -------------------------------------------------
            VersionNumber = "1.0.16.1221";

            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            string moduleName = currentProcess.MainModule.ModuleName;
            bool launchedFromStudio = moduleName.Contains( ".vshost" );

            AddRDToVersion();   // "1.0.16.715.D", "1.0.16.715.R"
            AddDEVToVersion( launchedFromStudio );


            Console.WriteLine( "OpPDir v:" + VersionNumber );
            Console.WriteLine( "-----------------------------------------------------------------------------" );

            // Check Parameter
            // Parameter: keine, oder Pattern (b*.exe)
            string pattern = "";
            if( args.Length == 0 )
            {
                // no Parameter
            }
            else if( args.Length >= 1 )
            {
                pattern = args[0];
            }

#if DEBUG
            // Debug-Test
            //pattern = "";
            //pattern = "sqlc*.exe";
            //pattern = "op*.exe";
            //pattern = "o*.exe";
            //pattern = "r*.exe";
            //pattern = "*.*";
#else
            // Release
#endif

            string curDir = System.IO.Directory.GetCurrentDirectory();
            string sysPath = System.Environment.GetEnvironmentVariable( "path" );

            // Make List of Path + GetCurrentDirectory
            int pathIndex = 0;
            List<Path> pathList = new List<Path>();
            // curDir
            Path path = new Path();
            path.Number = pathIndex;
            path.Name = curDir;
            path.ExpandName = curDir;
            path.IsCurrentDirectory = true;
            path.Exists = false;
            // Check
            //  47  49 = %USERPROFILE%\AppData\Local\Microsoft\WindowsApps

            if( System.IO.Directory.Exists( path.Name ) ){
                path.Exists = true;
            }
            pathList.Add( path );
            pathIndex++;
            // from PATH
            var pathSplit = sysPath.Split( ';' );
            for( int i = 0; i < pathSplit.Length; i++ )
            {
                string onePath = pathSplit[i];

                string expandPath = System.Environment.ExpandEnvironmentVariables( onePath );

                path = new Path();
                path.Number = pathIndex;
                path.Name = onePath;
                path.ExpandName = expandPath;
                path.IsCurrentDirectory = false;
                path.Exists = false;
                if( System.IO.Directory.Exists( path.ExpandName ) )
                {
                    path.Exists = true;
                }
                pathList.Add( path );
                pathIndex++;
            }

            if( pattern == "" )
            {
                // Display:
                // Current    = C:\S
                // PATH       = C:\Windows\system32;C:\Windows;C:\Windows\System32\Wbem;C:\Windows\...
                // Cnt Pos = Path
                // -----------------------------------------------------------------------------
                //   0   0 = C:\S
                //   1  20 = C:\Windows\system32
                //   2  31 = C:\Windows
                // ...
                //   6 150 = c:\PROGRA~1\MICROS~
                //...
                //   8 242 = c:\PROGRA~1\MICROS~
                // -----------------------------------------------------------------------------

                Console.WriteLine( "Current = " + curDir );
                Console.WriteLine( "PATH    = " + sysPath );
                Console.WriteLine( "Cnt Len = Path" );
                Console.WriteLine( "-----------------------------------------------------------------------------" );
                //                  ### ### = xxx...
                //var pathList = sysPath.Split( ';' );
                for( int i = 0; i < pathList.Count; i++ )
                {
                    string disp = "";

                    string cnt = i.ToString();
                    string length = pathList[i].Name.Length.ToString();

                    disp = disp + cnt.PadLeft( _LEN_PATH, ' ' );
                    disp = disp + " ";
                    disp = disp + length.PadLeft( _LEN_P_LEN, ' ' );
                    disp = disp + " = ";
                    disp = disp + pathList[i].Name;
                    if (pathList[i].Name!= pathList[i].ExpandName )
                    {
                        disp = disp + " ";
                        disp = disp + "=> " + pathList[i].ExpandName + "";
                    }
                    if( pathList[i].IsCurrentDirectory ){
                        disp = disp + " (=CurDir)";
                    }
                    if( !pathList[i].Exists ){
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine( disp );
                    Console.ResetColor();
                }
                Console.WriteLine( "-----------------------------------------------------------------------------" );
            }
            else
            {
                //string pattern = args[0];

                // dir with Pattern
                // Search for B*.EXE
                // -----------------------------------------------------------------------------
                //   1 C:\Windows\system32 []
                //                            1 BCDBOOT .EXE     146,944 20.11.10 13:16:56 A....
                //                            2 BCDEDIT .EXE     295,936 11.10.16 15:51:10 A....
                //                            3 BDEUISRV.EXE      41,984 14.07.09  2:14:14 A....
                //                            4 BOOTCFG .EXE      81,408 14.07.09  2:14:14 A....
                //   2 C:\Windows []
                //                            5 BFSVC   .EXE      65,024 20.11.10 13:16:56 A....
                //   6 c:\PROGRA~1\MICROS~3\100\Tools\Binn\ []
                //                            6 BCP     .EXE      89,440  3.04.10 11:58:00 A....
                // -----------------------------------------------------------------------------
                //    13 Files found                           1,450,336
                //
                // Windows
                //                       dd.mm.yyyy  hh:mm 9.999.999.999.999 Namexxxxxxxxxxx.exe
                //                       05.10.2016  14:21         1.341.952 7z64.dll
                //                       06.07.2016  16:44             8.704 PE-HeaderReader.exe
                // -----------------------------------------------------------------------------

                //string curDir = System.IO.Directory.GetCurrentDirectory();
                //string sysPath = System.Environment.GetEnvironmentVariable( "path" );

                int countFoundFiles = 0;
                long sumSizeFoundFiles = 0;

                Console.WriteLine( "Search for " + pattern );
                Console.WriteLine( "-----------------------------------------------------------------------------" );
                //var pathList = sysPath.Split( ';' );
                for( int i = 0; i < pathList.Count; i++ )
                {
                    bool exists = true;

                    string disp = "";

                    string cnt = i.ToString();

                    disp = disp + cnt.PadLeft( _LEN_PATH, ' ' );
                    disp = disp + " ";
                    disp = disp + pathList[i].Name;
                    if( pathList[i].Name != pathList[i].ExpandName ) {
                        disp = disp + " ";
                        disp = disp + "=> " + pathList[i].ExpandName + "";
                    }
                    if( pathList[i].IsCurrentDirectory )
                    {
                        disp = disp + " (=CurDir)";
                    }
                    if( !pathList[i].Exists )
                    {
                        exists = false;
                    }
                    //disp = disp + " ";

                    // search
                    string error = "";
                    List<File> listeFiles = new List<File>();
                    //File
                    string[] fileList = new string[] { };
                    try
                    {
                        if( pathList[i].Exists )
                        {
                            string onePath = pathList[i].Name;
                            onePath = pathList[i].ExpandName;
                            //fileList = System.IO.Directory.GetFiles( onePath, pattern );
                            if( System.IO.Directory.Exists( onePath ) )
                            {
                                fileList = System.IO.Directory.GetFiles( onePath, pattern );

                                for( int j = 0; j < fileList.Length; j++ )
                                {
                                    countFoundFiles++;

                                    string fullFileName = fileList[j];

                                    // Get FileInfo
                                    System.IO.FileInfo fi = new System.IO.FileInfo( fullFileName );

                                    string onlyFileName = fi.Name;

                                    sumSizeFoundFiles = sumSizeFoundFiles + fi.Length;

                                    File file = new File();
                                    file.Number = countFoundFiles;
                                    file.Name = onlyFileName;
                                    file.Size = fi.Length;
                                    file.CrDate = fi.CreationTime;
                                    file.ModDate = fi.LastWriteTime;

                                    listeFiles.Add( file );
                                }
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    catch( Exception ex )
                    {
                        error = "*** Error: " + ex.Message;

                        //Console.WriteLine( disp );
                        //Console.WriteLine( error );
                    }

                    // display Header-Path
                    bool dispLine = false;
                    if( !exists ) {
                        dispLine = true;
                    }
                    if( listeFiles != null && listeFiles.Count > 0 ){
                        dispLine = true;
                    }

                    // Debug-Test
                    //dispLine = true;

                    if( dispLine){
                        if( !exists )
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.WriteLine( disp );
                        Console.ResetColor();
                    }

                    if( listeFiles != null && listeFiles.Count > 0 )
                    {
                        //if( !exists )
                        //{
                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //}
                        ////Console.WriteLine( disp );
                        //Console.ResetColor();

                        for( int j = 0; j < listeFiles.Count; j++ )
                        {
                            File file = listeFiles[j];
                            string onlyFileName = file.Name;
                            int cnt2 = file.Number;
                            DateTime crDate = file.CrDate;
                            DateTime modDate = file.ModDate;
                            long size = file.Size;

                            string dispCnt2 = cnt2.ToString();
                            string dispSize = size.ToString( "#,##0" );

                            string dispFile = "";
                            dispFile = dispFile + "                      ";
                            dispFile = dispFile + dispCnt2.PadLeft( _LEN_FILE, ' ' );
                            dispFile = dispFile + " ";
                            dispFile = dispFile + modDate.ToString();
                            dispFile = dispFile + " ";
                            dispFile = dispFile + dispSize.PadLeft( 15, ' ' );  // 999.999.999.999
                            dispFile = dispFile + " ";
                            dispFile = dispFile + onlyFileName;
                            Console.WriteLine( dispFile );
                        }
                    }
                }
                Console.WriteLine( "-----------------------------------------------------------------------------" );

                string dispCntSum = countFoundFiles.ToString();
                string dispSizeSum = sumSizeFoundFiles.ToString( "#,##0" );

                string summe = "";
                summe = summe + "                      ";
                summe = summe + dispCntSum.PadLeft( _LEN_FILE, ' ' );
                summe = summe + " Files found";
                summe = summe + "         ";
                summe = summe + dispSizeSum.PadLeft( 15, ' ' );
                Console.WriteLine( summe );
            }


            if( launchedFromStudio )
            {
                // Pause
                Console.WriteLine( "" );
                Console.WriteLine( "[DEV] weiter mit einer Taste..." );
                Console.ReadKey();
            }

            // Exit
            System.Environment.Exit( 0 );
        }

        public static void AddRDToVersion()
        {
#if DEBUG
            VersionNumber += ".D";
#else
            VersionNumber += ".R";
#endif
        }

        public static void AddDEVToVersion( bool launchedFromStudio )
        {
            if( launchedFromStudio )
            {
                VersionNumber += ".[DEV]";
            }
        }
    }

    class Path
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string ExpandName { get; set; }
        public bool Exists { get; set; }
        public bool IsCurrentDirectory { get; set; }
    }

    class File
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CrDate { get; set; }
        public DateTime ModDate { get; set; }
    }
}
