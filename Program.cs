using CodeMetricsUtility;
class Program
{
    static void Main(string[] args)
    {
        string rootDirectory = "C:\\Nyce\\4.x-server\\NyceLogicServer4.0\\Middleware\\WorkServer\\NyceLogicWorkServer\\Business\\";
        CodeMetricsProcessor calculator = new CodeMetricsProcessor(rootDirectory);
        calculator.CalculateMetrics();
    }
}