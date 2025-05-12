namespace GetP4Revisions;

using Cs.Core.Util;
using System;

public sealed class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 5)
        {
            Console.WriteLine("Usage: GetP4Revisions <server> <user> <password> <workspace> <file1> [<file2> ...]");
            Environment.Exit(1);
        }

        string p4Server = args[0];
        string p4User = args[1];
        string p4Password = args[2];
        string p4Workspace = args[3];
        string[] files = args[4..];

        try
        {
          

            foreach (string file in files)
            {
                // 파일의 최신 Changelist 번호 가져오기
                string depotPath = GetDepotPath(file);
                if (depotPath != null)
                {
                    string changelist = GetLatestChangelist(depotPath);
                    if (changelist != null)
                    {
                        // 파일 경로와 Changelist 번호를 출력 (MSBuild에서 사용)
                        Console.WriteLine($"{file}.revision={changelist}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }

    static string GetDepotPath(string localPath)
    {
        try
        {
            OutProcess.Run("p4", $"where {localPath}", out string p4Output);
            string[] lines = p4Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 0)
            {
                return lines[^1].Split(" ")[0].Trim();
            }
        }
        catch
        {
            // 오류 무시
        }
        return string.Empty;
    }

    static string GetLatestChangelist(string depotPath)
    {
        try
        {
            OutProcess.Run("p4", $"fstat -T haveRev {depotPath}", out string p4Output);
            var tokens = p4Output.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0)
            {
                return tokens[^1];
            }
        }
        catch
        {
            // 오류 무시
        }
        return string.Empty;
    }
}