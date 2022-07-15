using System;
using System.Runtime.CompilerServices;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BVA.Loader
{
    public class WebRequestLoader : IDataLoader
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly Uri baseAddress;

        public WebRequestLoader(string rootUri)
        {
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            baseAddress = new Uri(rootUri);
        }

        public async Task<Stream> LoadStreamAsync(string gltfFilePath)
        {
            if (gltfFilePath == null)
            {
                throw new ArgumentNullException(nameof(gltfFilePath));
            }

            HttpResponseMessage response;
            try
            {
                var tokenSource = new CancellationTokenSource(30000);
                response = await httpClient.GetAsync(new Uri(baseAddress, gltfFilePath), tokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Connection timeout");
            }

            response.EnsureSuccessStatusCode();
            var result = new MemoryStream((int?)response.Content.Headers.ContentLength ?? 5000);
            await response.Content.CopyToAsync(result);
            response.Dispose();
            return result;
        }
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (errors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }

            return isOk;
        }
    }

    public static class AsyncOperationExtension
    {
        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            return new AsyncOperationAwaiter(asyncOp);
        }
    }

    public class AsyncOperationAwaiter : INotifyCompletion
    {
        private AsyncOperation asyncOp;
        private Action continuation;

        public AsyncOperationAwaiter(AsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }
        public bool IsCompleted { get { return asyncOp.isDone; } }
        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            continuation();
        }
    }

}
