﻿using NLog;
using System;

namespace InstaHangouts.Log.NLog
{
    public class NLogLogger : ILogger
    {
        private Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
            => _logger.Info(message);

        public void Warn(string message)
            => _logger.Warn(message);

        public void Debug(string message)
            => _logger.Debug(message);

        public void Error(string message)
            => _logger.Error(message);

        public void Error(Exception x)
            => Error(LogUtility.BuildExceptionMessage(x));

        public void Error(string message, Exception x)
            => _logger.Error(x, message);

        public void Fatal(string message)
            => _logger.Fatal(message);

        public void Fatal(Exception x)
            => Fatal(LogUtility.BuildExceptionMessage(x));
    }
}
