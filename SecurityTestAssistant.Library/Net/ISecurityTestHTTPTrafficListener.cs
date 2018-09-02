namespace SecurityTestAssistant.Library.Net
{


    public interface ISecurityTestHTTPTrafficListener
    {
        event HttpUrlRequesting OnHttpUrlRequested;
        event HttpResponseReceived OnHttpResponseReceived;

        void StartListening();
        void StopListening();

        void DoNotListen(string domainName);
        void DoNotListenAllDomains();
        void ListenDomain(string domainName);
    }

    
}
