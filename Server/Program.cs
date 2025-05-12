namespace Server;

using Cs.Core.Util;
using System.Net;
using System.Text;

public class Program
{
    // Perforce 서버 설정 (환경에 맞게 수정)
    private static readonly string P4Server = "<server>";
    private static readonly string P4User = "<user>>";
    private static readonly string P4Password = "<password>";
    private static readonly string P4Workspace = "<workspace>";

    static void Main(string[] args)
    {
        // HttpListener 초기화
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/source/");
        listener.Start();
        Console.WriteLine("HTTP Server started at http://localhost:8080/source/");

        try
        {
            while (true)
            {
                // 비동기 요청 처리
                HttpListenerContext context = listener.GetContext();
                ProcessRequest(context);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            listener.Stop();
        }
    }

    static void ProcessRequest(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        try
        {
            // 요청된 경로에서 depot 경로 추출
            string depotPath = request.Url.AbsolutePath.Replace("/source/", "//stream/");
            if (string.IsNullOrEmpty(depotPath))
            {
                SendError(response, HttpStatusCode.BadRequest, "Invalid depot path");
                return;
            }

            // Perforce에서 파일 내용 가져오기
            string fileContent = GetFileFromPerforce(depotPath);
            if (fileContent == null)
            {
                SendError(response, HttpStatusCode.NotFound, $"File not found: {depotPath}");
                return;
            }

            // 응답 작성
            byte[] buffer = Encoding.UTF8.GetBytes(fileContent);
            response.ContentType = "text/plain; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            response.StatusCode = (int)HttpStatusCode.OK;

            using (System.IO.Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
        catch (Exception ex)
        {
            SendError(response, HttpStatusCode.InternalServerError, $"Server error: {ex.Message}");
        }
        finally
        {
            response.Close();
        }
    }

    static string GetFileFromPerforce(string depotPath)
    {
        try
        {
            // 파일 내용 가져오기
            OutProcess.Run("p4", $"print {depotPath}", out string p4Output);
            return p4Output;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"P4 Error: {ex.Message}");
        }

        return string.Empty;
    }

    static void SendError(HttpListenerResponse response, HttpStatusCode statusCode, string message)
    {
        response.StatusCode = (int)statusCode;
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;
        response.ContentType = "text/plain; charset=utf-8";
        using (System.IO.Stream output = response.OutputStream)
        {
            output.Write(buffer, 0, buffer.Length);
        }
    }
}