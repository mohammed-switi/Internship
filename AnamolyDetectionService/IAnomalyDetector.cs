namespace AnamolyDetectionService;
using System.Collections.Generic;

using System.Collections.Generic;

public interface IAnomalyDetector
{
    /// <summary>
    /// Detects memory usage anomalies based on current and historical data.
    /// </summary>
    /// <param name="current">Current memory usage value.</param>
    /// <param name="historical">List of recent memory usage values.</param>
    /// <returns>True if an anomaly is detected; otherwise, false.</returns>
    bool DetectMemoryAnomaly(double current, IEnumerable<double> historical);

    /// <summary>
    /// Detects CPU usage anomalies based on current and historical data.
    /// </summary>
    /// <param name="current">Current CPU usage value.</param>
    /// <param name="historical">List of recent CPU usage values.</param>
    /// <returns>True if an anomaly is detected; otherwise, false.</returns>
    bool DetectCpuAnomaly(double current, IEnumerable<double> historical);

    /// <summary>
    /// Checks if the current usage exceeds a defined threshold (i.e., high usage).
    /// </summary>
    /// <param name="current">Current usage value.</param>
    /// <param name="threshold">Threshold value to compare against.</param>
    /// <returns>True if current usage exceeds threshold; otherwise, false.</returns>
    bool IsHighUsage(double current, double threshold);
}
