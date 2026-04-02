namespace BusinessERP.Helpers;
public static class ExceptionLogging
{
    public static void LogError(Exception ex)
    {
        try
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "Exception: -----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "----------------------------------------------------------------------";
            message += Environment.NewLine;

            string _FilePath = Path.Combine(GetWebEnv().WebRootPath, "upload/ErrorLog.txt");
            using (StreamWriter _StreamWriter = new(_FilePath, true))
            {
                _StreamWriter.WriteLine(message);
                _StreamWriter.Close();
            }
        }
        catch (Exception)
        {
            return;
            throw;
        }       
    }
    public static void GeneralLog(string _LogMessage)
    {
        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        message += Environment.NewLine;
        message += "General Log: -----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", _LogMessage);
        message += Environment.NewLine;
        message += "------------------------------------------------------------------------";
        message += Environment.NewLine;

        string _FilePath = Path.Combine(GetWebEnv().WebRootPath, "upload/ErrorLog.txt");
        using (StreamWriter _StreamWriter = new(_FilePath, true))
        {
            _StreamWriter.WriteLine(message);
            _StreamWriter.Close();
        }
    }
    public static IWebHostEnvironment GetWebEnv()
    {
        try
        {
            var _HttpContextAccessor = new HttpContextAccessor();
            return _HttpContextAccessor.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        }
        catch (Exception)
        {
            return null;
            throw;
        }
    }
}