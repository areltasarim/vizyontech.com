using System;

namespace EticaretWebCoreHelper
{
    public interface IProgressReporterFactory
    {
        IProgress<double> GetLoadingBarReporter();
    }
}