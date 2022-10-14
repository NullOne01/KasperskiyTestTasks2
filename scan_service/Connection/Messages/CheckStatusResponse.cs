using System;
using scan_service.Data;

namespace scan_service.Connection.Messages
{
    [Serializable]
    public class CheckStatusResponse
    {
        public ScanResult ScanResult = null;
    }
}