using System;

namespace Devbridge.SeleniumTools.NUnitExtensions.Contracts
{
    public interface ISeleniumHubApiContext : IDisposable
    {
        string HubUrl { get; set; }

        void SetSucceded();

        void SetFailed();

        void PutData(object data);
    }
}
